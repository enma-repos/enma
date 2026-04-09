"use client";

import { useMemo, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import SuperAuditLogsService from "@/services/admin/super/superAuditLogsService";
import { useDebouncedValue } from "@/hooks/useDebouncedValue";
import type { SuperAuditLogsQuery } from "@/types/super-admin.types";

type Filters = Omit<SuperAuditLogsQuery, "page" | "pageSize" | "search">;

export function useSuperAuditLogs(initialPageSize = 20) {
  const service = useMemo(() => new SuperAuditLogsService(), []);

  const [page, setPage] = useState(1);
  const [pageSize, setPageSizeState] = useState(initialPageSize);
  const [search, setSearch] = useState("");
  const [filters, setFiltersState] = useState<Filters>({});

  const debouncedSearch = useDebouncedValue(search);

  const query = useQuery({
    queryKey: ["super", "auditLogs", page, pageSize, debouncedSearch, filters] as const,
    queryFn: () =>
      service.list({
        page,
        pageSize,
        search: debouncedSearch || undefined,
        ...filters,
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

  const setFilters = (value: Filters) => {
    setFiltersState(value);
    setPage(1);
  };

  return {
    items: query.data?.items ?? [],
    total: query.data?.totalCount ?? 0,
    totalPages: query.data?.totalPages ?? 0,
    page,
    pageSize,
    search,
    filters,
    isLoading: query.isLoading,
    isFetching: query.isFetching,
    error: query.error,
    setPage,
    setPageSize,
    setSearch: handleSearchChange,
    setFilters,
  };
}
