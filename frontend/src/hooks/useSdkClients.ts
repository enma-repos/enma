"use client";

import { useMemo, useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { CreateSdkClientDto, OrganizationDto, ProjectDto, SdkClientDto } from "@/types/admin.types";
import OrganizationsService from "@/services/admin/organizationsService";
import ProjectsService from "@/services/admin/projectsService";
import SdkClientsService from "@/services/admin/sdkClientsService";
import { useDebouncedValue } from "@/hooks/useDebouncedValue";

function isGuidLike(value: string) {
  return /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i.test(value);
}

export function useSdkClients(organizationSlug: string, projectKey: string) {
  const queryClient = useQueryClient();
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);

  const [search, setSearch] = useState("");
  const debouncedSearch = useDebouncedValue(search);

  const handlePageSizeChange = (newPageSize: number) => {
    setPageSize(newPageSize);
    setPage(1);
  };

  const handleSearchChange = (value: string) => {
    setSearch(value);
    setPage(1);
  };

  const organizationsService = useMemo(() => new OrganizationsService(), []);
  const projectsService = useMemo(() => new ProjectsService(), []);
  const sdkClientsService = useMemo(() => new SdkClientsService(), []);

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

  const clientsQueryKey = useMemo(() => {
    return ["sdkClients", organizationId, projectId, page, pageSize, debouncedSearch] as const;
  }, [organizationId, projectId, page, pageSize, debouncedSearch]);

  const clientsQuery = useQuery({
    queryKey: clientsQueryKey,
    enabled: Boolean(organizationId) && Boolean(projectId),
    queryFn: () => {
      if (!organizationId || !projectId) throw new Error("Missing ids");
      return sdkClientsService.listByProject(organizationId, projectId, page, pageSize, debouncedSearch || undefined);
    },
  });

  const createMutation = useMutation({
    mutationFn: async (dto: Omit<CreateSdkClientDto, "projectId">) => {
      if (!organizationId || !projectId) throw new Error("Missing ids");
      return sdkClientsService.create(organizationId, projectId, dto);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: clientsQueryKey });
    },
  });

  const setNameMutation = useMutation({
    mutationFn: async ({ id, name }: { id: string; name: string }) => {
      if (!organizationId || !projectId) throw new Error("Missing ids");
      return sdkClientsService.setName(organizationId, projectId, id, { name });
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: clientsQueryKey });
    },
  });

  const setTypeMutation = useMutation({
    mutationFn: async ({ id, type }: { id: string; type: number }) => {
      if (!organizationId || !projectId) throw new Error("Missing ids");
      return sdkClientsService.setType(organizationId, projectId, id, { type });
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: clientsQueryKey });
    },
  });

  const disableMutation = useMutation({
    mutationFn: async (id: string) => {
      if (!organizationId || !projectId) throw new Error("Missing ids");
      return sdkClientsService.disable(organizationId, projectId, id);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: clientsQueryKey });
    },
  });

  const enableMutation = useMutation({
    mutationFn: async (id: string) => {
      if (!organizationId || !projectId) throw new Error("Missing ids");
      return sdkClientsService.enable(organizationId, projectId, id);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: clientsQueryKey });
    },
  });

  return {
    organization,
    project,
    organizationId,
    projectId,
    sdkClients: (clientsQuery.data?.items ?? []) as SdkClientDto[],
    totalPages: clientsQuery.data?.totalPages ?? 0,
    totalCount: clientsQuery.data?.totalCount ?? 0,
    page,
    pageSize,
    setPage,
    setPageSize: handlePageSizeChange,
    search,
    setSearch: handleSearchChange,
    isLoading: organizationQuery.isLoading || projectQuery.isLoading || clientsQuery.isLoading,
    error: organizationQuery.error ?? projectQuery.error ?? clientsQuery.error,

    createClient: createMutation.mutateAsync,
    isCreating: createMutation.isPending,
    createError: createMutation.error,

    setClientName: setNameMutation.mutateAsync,
    isSavingName: setNameMutation.isPending,

    setClientType: setTypeMutation.mutateAsync,
    isSavingType: setTypeMutation.isPending,

    disableClient: disableMutation.mutateAsync,
    enableClient: enableMutation.mutateAsync,
  };
}
