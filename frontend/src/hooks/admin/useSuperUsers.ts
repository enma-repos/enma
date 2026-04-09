"use client";

import { useMemo, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import SuperUsersService from "@/services/admin/super/superUsersService";
import { useDebouncedValue } from "@/hooks/useDebouncedValue";

export function useSuperUsers(initialPageSize = 20) {
  const service = useMemo(() => new SuperUsersService(), []);

  const [page, setPage] = useState(1);
  const [pageSize, setPageSizeState] = useState(initialPageSize);
  const [search, setSearch] = useState("");
  const [includeDeleted, setIncludeDeleted] = useState(false);

  const debouncedSearch = useDebouncedValue(search);

  const query = useQuery({
    queryKey: ["super", "users", page, pageSize, debouncedSearch, includeDeleted] as const,
    queryFn: () =>
      service.list({
        page,
        pageSize,
        search: debouncedSearch || undefined,
        includeDeleted,
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

  return {
    items: query.data?.items ?? [],
    total: query.data?.totalCount ?? 0,
    totalPages: query.data?.totalPages ?? 0,
    page,
    pageSize,
    search,
    includeDeleted,
    isLoading: query.isLoading,
    isFetching: query.isFetching,
    error: query.error,
    setPage,
    setPageSize,
    setSearch: handleSearchChange,
    setIncludeDeleted: handleToggleDeleted,
  };
}
