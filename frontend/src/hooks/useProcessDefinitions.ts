"use client";

import { useMemo } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { CreateProcessDefinitionDto, OrganizationDto, ProcessDefinitionDto, ProjectDto } from "@/types/admin.types";
import OrganizationsService from "@/services/admin/organizationsService";
import ProjectsService from "@/services/admin/projectsService";
import ProcessDefinitionsService from "@/services/admin/processDefinitionsService";
import { useMe } from "@/hooks/useMe";

function isGuidLike(value: string) {
  return /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i.test(value);
}

export function useProcessDefinitions(organizationSlug: string, projectKey: string) {
  const queryClient = useQueryClient();

  const meQuery = useMe();

  const organizationsService = useMemo(() => new OrganizationsService(), []);
  const projectsService = useMemo(() => new ProjectsService(), []);
  const processDefinitionsService = useMemo(() => new ProcessDefinitionsService(), []);

  const organizationQuery = useQuery({
    queryKey: ["organization", organizationSlug],
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

  const projectQuery = useQuery({
    queryKey: ["project", organizationId, projectKey],
    enabled: Boolean(organizationId) && Boolean(projectKey),
    queryFn: () => {
      if (!organizationId) throw new Error("Missing organizationId");
      if (isGuidLike(projectKey)) {
        return projectsService.getById(organizationId, projectKey);
      }
      return projectsService.getByOrgAndKey(organizationId, projectKey);
    },
  });

  const project = projectQuery.data as ProjectDto | undefined;
  const projectId = project?.id;

  const processesQueryKey = useMemo(() => {
    return ["processDefinitions", organizationId, projectId] as const;
  }, [organizationId, projectId]);

  const processesQuery = useQuery({
    queryKey: processesQueryKey,
    enabled: Boolean(organizationId) && Boolean(projectId),
    queryFn: () => {
      if (!organizationId || !projectId) throw new Error("Missing ids");
      return processDefinitionsService.listByProject(organizationId, projectId);
    },
  });

  const createMutation = useMutation({
    mutationFn: async (dto: Omit<CreateProcessDefinitionDto, "projectId">) => {
      if (!organizationId || !projectId) throw new Error("Missing ids");
      return processDefinitionsService.create(organizationId, projectId, dto);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: processesQueryKey });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: async (id: string) => {
      if (!organizationId || !projectId) throw new Error("Missing ids");
      return processDefinitionsService.delete(organizationId, projectId, id);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: processesQueryKey });
    },
  });

  const setNameMutation = useMutation({
    mutationFn: async ({ id, name }: { id: string; name: string }) => {
      if (!organizationId || !projectId) throw new Error("Missing ids");
      return processDefinitionsService.setName(organizationId, projectId, id, { name });
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: processesQueryKey });
    },
  });

  const setDescriptionMutation = useMutation({
    mutationFn: async ({ id, description }: { id: string; description: string | null }) => {
      if (!organizationId || !projectId) throw new Error("Missing ids");
      return processDefinitionsService.setDescription(organizationId, projectId, id, { description });
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: processesQueryKey });
    },
  });

  return {
    organization,
    project,
    processDefinitions: (processesQuery.data ?? []) as ProcessDefinitionDto[],
    isLoading: meQuery.isLoading || organizationQuery.isLoading || projectQuery.isLoading || processesQuery.isLoading,
    error: meQuery.error ?? organizationQuery.error ?? projectQuery.error ?? processesQuery.error,

    createProcess: createMutation.mutateAsync,
    isCreating: createMutation.isPending,
    createError: createMutation.error,

    deleteProcess: deleteMutation.mutateAsync,
    isDeleting: deleteMutation.isPending,

    setProcessName: setNameMutation.mutateAsync,
    isSavingName: setNameMutation.isPending,

    setProcessDescription: setDescriptionMutation.mutateAsync,
    isSavingDescription: setDescriptionMutation.isPending,
  };
}
