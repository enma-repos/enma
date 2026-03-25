"use client";

import { useMemo } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { CreateEventDefinitionDto, OrganizationDto, EventDefinitionDto, ProjectDto } from "@/types/admin.types";
import OrganizationsService from "@/services/admin/organizationsService";
import ProjectsService from "@/services/admin/projectsService";
import EventDefinitionsService from "@/services/admin/eventDefinitionsService";
import { useMe } from "@/hooks/useMe";

function isGuidLike(value: string) {
  return /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i.test(value);
}

export function useEventDefinitions(organizationSlug: string, projectKey: string) {
  const queryClient = useQueryClient();

  const meQuery = useMe();

  const organizationsService = useMemo(() => new OrganizationsService(), []);
  const projectsService = useMemo(() => new ProjectsService(), []);
  const eventDefinitionsService = useMemo(() => new EventDefinitionsService(), []);

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

  const eventsQueryKey = useMemo(() => {
    return ["eventDefinitions", organizationId, projectId] as const;
  }, [organizationId, projectId]);

  const eventsQuery = useQuery({
    queryKey: eventsQueryKey,
    enabled: Boolean(organizationId) && Boolean(projectId),
    queryFn: () => {
      if (!organizationId || !projectId) throw new Error("Missing ids");
      return eventDefinitionsService.listByProject(organizationId, projectId);
    },
  });

  const createMutation = useMutation({
    mutationFn: async (dto: Omit<CreateEventDefinitionDto, "projectId">) => {
      if (!organizationId || !projectId) throw new Error("Missing ids");
      return eventDefinitionsService.create(organizationId, projectId, dto);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: eventsQueryKey });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: async (id: string) => {
      if (!organizationId || !projectId) throw new Error("Missing ids");
      return eventDefinitionsService.delete(organizationId, projectId, id);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: eventsQueryKey });
    },
  });

  const setDescriptionMutation = useMutation({
    mutationFn: async ({ id, description }: { id: string; description: string | null }) => {
      if (!organizationId || !projectId) throw new Error("Missing ids");
      return eventDefinitionsService.setDescription(organizationId, projectId, id, { description });
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: eventsQueryKey });
    },
  });

  return {
    organization,
    project,
    eventDefinitions: (eventsQuery.data ?? []) as EventDefinitionDto[],
    isLoading: meQuery.isLoading || organizationQuery.isLoading || projectQuery.isLoading || eventsQuery.isLoading,
    error: meQuery.error ?? organizationQuery.error ?? projectQuery.error ?? eventsQuery.error,

    createEvent: createMutation.mutateAsync,
    isCreating: createMutation.isPending,
    createError: createMutation.error,

    deleteEvent: deleteMutation.mutateAsync,
    isDeleting: deleteMutation.isPending,

    setEventDescription: setDescriptionMutation.mutateAsync,
    isSavingDescription: setDescriptionMutation.isPending,
  };
}
