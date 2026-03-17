"use client";

import { useMemo, useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { CreateProjectDto, OrganizationDto, ProjectDto } from "@/types/admin.types";
import OrganizationsService from "@/services/admin/organizationsService";
import ProjectsService from "@/services/admin/projectsService";
import { useMe } from "@/hooks/useMe";

function normalizeQuery(value: string) {
  return value.trim().toLowerCase();
}

function filterProjects(projects: ProjectDto[], query: string) {
  const q = normalizeQuery(query);
  if (!q) return projects;

  return projects.filter((project) => {
    return project.name.toLowerCase().includes(q) || project.key.toLowerCase().includes(q);
  });
}

function isGuidLike(value: string) {
  return /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i.test(value);
}

export function useProjects(organizationSlug: string) {
  const queryClient = useQueryClient();
  const [query, setQuery] = useState("");

  const meQuery = useMe();
  const accountId = meQuery.data?.account.id;

  const organizationsService = useMemo(() => new OrganizationsService(), []);
  const projectsService = useMemo(() => new ProjectsService(), []);

  const organizationQueryKey = useMemo(() => {
    return ["organization", organizationSlug] as const;
  }, [organizationSlug]);

  const organizationQuery = useQuery({
    queryKey: organizationQueryKey,
    enabled: Boolean(organizationSlug),
    queryFn: () => {
      if (isGuidLike(organizationSlug)) {
        return organizationsService.getById(organizationSlug);
      }
      return organizationsService.getBySlug(organizationSlug);
    },
  });

  const organization = organizationQuery.data as OrganizationDto | undefined;
  const organizationId = organization?.id;

  const projectsQueryKey = useMemo(() => {
    return ["projects", organizationId] as const;
  }, [organizationId]);

  const projectsQuery = useQuery({
    queryKey: projectsQueryKey,
    enabled: Boolean(organizationId),
    queryFn: () => {
      if (!organizationId) throw new Error("Missing organizationId");
      return projectsService.listByOrg(organizationId);
    },
  });

  const createProjectMutation = useMutation({
    mutationFn: async (dto: Omit<CreateProjectDto, "organizationId" | "createdByUserId">) => {
      if (!organizationId) throw new Error("Missing organizationId");
      if (!accountId) throw new Error("Missing accountId");
      return projectsService.create(organizationId, { ...dto, createdByUserId: accountId });
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: projectsQueryKey });
    },
  });

  const setProjectNameMutation = useMutation({
    mutationFn: async ({ projectId, name }: { projectId: string; name: string }) => {
      if (!organizationId) throw new Error("Missing organizationId");
      await projectsService.setName(organizationId, projectId, { name });
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: projectsQueryKey });
    },
  });

  const softDeleteProjectMutation = useMutation({
    mutationFn: async (projectId: string) => {
      if (!organizationId) throw new Error("Missing organizationId");
      await projectsService.softDelete(organizationId, projectId);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: projectsQueryKey });
    },
  });

  const filteredProjects = useMemo(() => {
    return filterProjects(projectsQuery.data ?? [], query);
  }, [projectsQuery.data, query]);

  return {
    me: meQuery.data,
    organization,
    projects: (projectsQuery.data ?? []) as ProjectDto[],
    filteredProjects: filteredProjects as ProjectDto[],
    isLoading: meQuery.isLoading || organizationQuery.isLoading || projectsQuery.isLoading,
    error: meQuery.error ?? organizationQuery.error ?? projectsQuery.error,
    query,
    setQuery,

    createProject: createProjectMutation.mutateAsync,
    isCreating: createProjectMutation.isPending,
    createError: createProjectMutation.error,

    setProjectName: setProjectNameMutation.mutateAsync,
    softDeleteProject: softDeleteProjectMutation.mutateAsync,
  };
}
