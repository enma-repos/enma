"use client";

import { useMemo, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import type { AuditLogDto, OrganizationDto, ProjectDto } from "@/types/admin.types";
import type { AuditLogFilters } from "@/services/admin/auditLogsService";
import OrganizationsService from "@/services/admin/organizationsService";
import ProjectsService from "@/services/admin/projectsService";
import AuditLogsService from "@/services/admin/auditLogsService";
import { useMe } from "@/hooks/useMe";
import { useDebouncedValue } from "@/hooks/useDebouncedValue";

function isGuidLike(value: string) {
  return /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i.test(value);
}

export function useAuditLogs(organizationSlug: string, projectKey: string) {
  const meQuery = useMe();

  const organizationsService = useMemo(() => new OrganizationsService(), []);
  const projectsService = useMemo(() => new ProjectsService(), []);
  const auditLogsService = useMemo(() => new AuditLogsService(), []);

  const [search, setSearch] = useState("");
  const debouncedSearch = useDebouncedValue(search);

  const [filters, setFilters] = useState<AuditLogFilters>({
    page: 1,
    pageSize: 10,
  });

  const handleSearchChange = (value: string) => {
    setSearch(value);
    setFilters((prev) => ({ ...prev, page: 1 }));
  };

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

  const logsQueryKey = useMemo(() => {
    return ["auditLogs", organizationId, projectId, filters, debouncedSearch] as const;
  }, [organizationId, projectId, filters, debouncedSearch]);

  const logsQuery = useQuery({
    queryKey: logsQueryKey,
    enabled: Boolean(organizationId) && Boolean(projectId),
    queryFn: () => {
      if (!organizationId || !projectId) throw new Error("Missing ids");
      return auditLogsService.listByProject(organizationId, projectId, { ...filters, search: debouncedSearch || undefined });
    },
  });

  const items = (logsQuery.data?.items ?? []) as AuditLogDto[];
  const total = logsQuery.data?.totalCount ?? 0;
  const totalPages = logsQuery.data?.totalPages ?? 0;
  const page = logsQuery.data?.page ?? 1;

  const setPage = (p: number) => {
    setFilters((prev) => ({ ...prev, page: p }));
  };

  const setPageSize = (newPageSize: number) => {
    setFilters((prev) => ({ ...prev, pageSize: newPageSize, page: 1 }));
  };

  return {
    organization,
    project,
    auditLogs: items,
    total,
    totalCount: total,
    page,
    pageSize: filters.pageSize ?? 10,
    totalPages,
    setPage,
    setPageSize,
    search,
    setSearch: handleSearchChange,
    isLoading: meQuery.isLoading || organizationQuery.isLoading || projectQuery.isLoading || logsQuery.isLoading,
    error: meQuery.error ?? organizationQuery.error ?? projectQuery.error ?? logsQuery.error,
    filters,
    setFilters,
  };
}
