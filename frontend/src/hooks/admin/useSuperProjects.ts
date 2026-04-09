"use client";

import { useMemo, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import SuperProjectsService from "@/services/admin/super/superProjectsService";
import { useDebouncedValue } from "@/hooks/useDebouncedValue";
import type { Guid } from "@/types/admin.types";

export function useSuperProjects(initialPageSize = 20) {
  const service = useMemo(() => new SuperProjectsService(), []);

  const [page, setPage] = useState(1);
  const [pageSize, setPageSizeState] = useState(initialPageSize);
  const [search, setSearch] = useState("");
  const [includeDeleted, setIncludeDeleted] = useState(false);
  const [organizationId, setOrganizationId] = useState<Guid | undefined>(undefined);

  const debouncedSearch = useDebouncedValue(search);

  const query = useQuery({
    queryKey: [
      "super",
      "projects",
      page,
      pageSize,
      debouncedSearch,
      includeDeleted,
      organizationId ?? null,
    ] as const,
    queryFn: () =>
      service.list({
        page,
        pageSize,
        search: debouncedSearch || undefined,
        includeDeleted,
        organizationId,
      }),
  });

  const setPageSize = (size: number) => {
    setPageSizeState(size);
    setPage(1);
  };

  const handleSearchChange = (value: string) => {
    setSearch(value);
    setPage(1);
  };

  const handleToggleDeleted = (value: boolean) => {
    setIncludeDeleted(value);
    setPage(1);
  };

  const handleOrganizationIdChange = (value: Guid | undefined) => {
    setOrganizationId(value);
    setPage(1);
  };

  return {
    items: query.data?.items ?? [],
    total: query.data?.totalCount ?? 0,
    totalPages: query.data?.totalPages ?? 0,
    page,
    pageSize,
    search,
    includeDeleted,
    organizationId,
    isLoading: query.isLoading,
    isFetching: query.isFetching,
    error: query.error,
    setPage,
    setPageSize,
    setSearch: handleSearchChange,
    setIncludeDeleted: handleToggleDeleted,
    setOrganizationId: handleOrganizationIdChange,
  };
}
