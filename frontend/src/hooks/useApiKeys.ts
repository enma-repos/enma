"use client";

import { useMemo, useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { ApiKeyDto, ApiKeyFirstCreationDto } from "@/types/admin.types";
import ApiKeysService from "@/services/admin/apiKeysService";
import { useDebouncedValue } from "@/hooks/useDebouncedValue";

export function useApiKeys(organizationId: string | undefined, projectId: string | undefined, clientId: string | undefined) {
  const queryClient = useQueryClient();
  const apiKeysService = useMemo(() => new ApiKeysService(), []);

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

  const keysQueryKey = useMemo(() => {
    return ["apiKeys", organizationId, projectId, clientId, page, pageSize, debouncedSearch] as const;
  }, [organizationId, projectId, clientId, page, pageSize, debouncedSearch]);

  const keysQuery = useQuery({
    queryKey: keysQueryKey,
    enabled: Boolean(organizationId) && Boolean(projectId) && Boolean(clientId),
    queryFn: () => {
      if (!organizationId || !projectId || !clientId) throw new Error("Missing ids");
      return apiKeysService.listBySdkClient(organizationId, projectId, clientId, page, pageSize, debouncedSearch || undefined);
    },
  });

  const createMutation = useMutation({
    mutationFn: async (): Promise<ApiKeyFirstCreationDto> => {
      if (!organizationId || !projectId || !clientId) throw new Error("Missing ids");
      return apiKeysService.create(organizationId, projectId, clientId);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: keysQueryKey });
    },
  });

  const revokeMutation = useMutation({
    mutationFn: async (apiKeyId: string) => {
      if (!organizationId || !projectId || !clientId) throw new Error("Missing ids");
      return apiKeysService.revoke(organizationId, projectId, clientId, apiKeyId);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: keysQueryKey });
    },
  });

  return {
    apiKeys: (keysQuery.data?.items ?? []) as ApiKeyDto[],
    page,
    pageSize,
    setPage,
    setPageSize: handlePageSizeChange,
    search,
    setSearch: handleSearchChange,
    totalPages: keysQuery.data?.totalPages ?? 0,
    totalCount: keysQuery.data?.totalCount ?? 0,
    isLoading: keysQuery.isLoading,
    error: keysQuery.error,

    createKey: createMutation.mutateAsync,
    isCreatingKey: createMutation.isPending,
    createKeyError: createMutation.error,
    createdKey: createMutation.data as ApiKeyFirstCreationDto | undefined,
    resetCreatedKey: createMutation.reset,

    revokeKey: revokeMutation.mutateAsync,
    isRevoking: revokeMutation.isPending,
  };
}
