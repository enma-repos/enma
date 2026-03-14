"use client";

import { useMemo } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { ApiKeyDto, ApiKeyFirstCreationDto } from "@/types/admin.types";
import ApiKeysService from "@/services/admin/apiKeysService";

export function useApiKeys(organizationId: string | undefined, projectId: string | undefined, clientId: string | undefined) {
  const queryClient = useQueryClient();
  const apiKeysService = useMemo(() => new ApiKeysService(), []);

  const keysQueryKey = useMemo(() => {
    return ["apiKeys", organizationId, projectId, clientId] as const;
  }, [organizationId, projectId, clientId]);

  const keysQuery = useQuery({
    queryKey: keysQueryKey,
    enabled: Boolean(organizationId) && Boolean(projectId) && Boolean(clientId),
    queryFn: () => {
      if (!organizationId || !projectId || !clientId) throw new Error("Missing ids");
      return apiKeysService.listBySdkClient(organizationId, projectId, clientId);
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
    apiKeys: (keysQuery.data ?? []) as ApiKeyDto[],
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
