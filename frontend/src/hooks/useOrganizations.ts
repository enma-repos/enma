"use client";

import { useMemo, useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { CreateOrganizationDto, OrganizationDto } from "@/types/admin.types";
import OrganizationsService from "@/services/admin/organizationsService";
import { useMe } from "@/hooks/useMe";

function normalizeQuery(value: string) {
  return value.trim().toLowerCase();
}

function filterOrganizations(organizations: OrganizationDto[], query: string) {
  const q = normalizeQuery(query);
  if (!q) return organizations;

  return organizations.filter((org) => {
    return org.name.toLowerCase().includes(q) || org.slug.toLowerCase().includes(q);
  });
}

export function useOrganizations() {
  const queryClient = useQueryClient();
  const [query, setQuery] = useState("");

  const meQuery = useMe();
  const accountId = meQuery.data?.account.id;
  const service = useMemo(() => new OrganizationsService(), []);

  const organizationsQueryKey = useMemo(() => {
    return ["organizations", accountId] as const;
  }, [accountId]);

  const organizationsQuery = useQuery({
    queryKey: organizationsQueryKey,
    enabled: Boolean(accountId),
    queryFn: () => service.listByUser(),
  });

  const createOrganizationMutation = useMutation({
    mutationFn: async (dto: CreateOrganizationDto) => {
      return service.create(dto);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: organizationsQueryKey });
    },
  });

  const setOrganizationNameMutation = useMutation({
    mutationFn: async ({ organizationId, name }: { organizationId: string; name: string }) => {
      await service.setName(organizationId, { name });
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: organizationsQueryKey });
    },
  });

  const setOrganizationOwnerMutation = useMutation({
    mutationFn: async ({
      organizationId,
      ownerUserId,
    }: {
      organizationId: string;
      ownerUserId: string;
    }) => {
      await service.setOwner(organizationId, { ownerUserId });
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: organizationsQueryKey });
    },
  });

  const softDeleteOrganizationMutation = useMutation({
    mutationFn: async (organizationId: string) => {
      await service.softDelete(organizationId);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: organizationsQueryKey });
    },
  });

  const filteredOrganizations = useMemo(() => {
    return filterOrganizations(organizationsQuery.data ?? [], query);
  }, [organizationsQuery.data, query]);

  return {
    me: meQuery.data,
    organizations: (organizationsQuery.data ?? []) as OrganizationDto[],
    filteredOrganizations: filteredOrganizations as OrganizationDto[],
    isLoading: meQuery.isLoading || organizationsQuery.isLoading,
    error: meQuery.error ?? organizationsQuery.error,
    query,
    setQuery,

    createOrganization: createOrganizationMutation.mutateAsync,
    isCreating: createOrganizationMutation.isPending,
    createError: createOrganizationMutation.error,

    getOrganizationById: (organizationId: string) => service.getById(organizationId),
    getOrganizationBySlug: (slug: string) => service.getBySlug(slug),
    setOrganizationName: setOrganizationNameMutation.mutateAsync,
    setOrganizationOwner: setOrganizationOwnerMutation.mutateAsync,
    softDeleteOrganization: softDeleteOrganizationMutation.mutateAsync,
  };
}
