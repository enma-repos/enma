"use client";

import { useMemo, useState } from "react";
import type { AuditLogDto } from "@/types/admin.types";
import { DEFAULT_DATE_RANGE } from "@/components/app/analytics/date-range-picker";
import type { DateRange } from "@/components/app/analytics/date-range-picker";
import { useAuditLogs } from "@/hooks/useAuditLogs";
import { AuditLogsToolbar } from "@/components/app/audit-logs/audit-logs-toolbar";
import { AuditLogsTable } from "@/components/app/audit-logs/audit-logs-table";
import { AuditLogsEmpty } from "@/components/app/audit-logs/audit-logs-empty";
import { AuditLogsSkeleton } from "@/components/app/audit-logs/audit-logs-skeleton";
import { AuditLogDetailDialog } from "@/components/app/audit-logs/audit-log-detail-dialog";
import { AuditLogsPagination } from "@/components/app/audit-logs/audit-logs-pagination";

const RESOURCE_TYPES = [
  "Organization",
  "Project",
  "ApiKey",
  "SdkClient",
  "OrganizationMember",
  "ProjectMember",
  "OrganizationInvite",
  "ProcessDefinition",
  "EventDefinition",
];

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось загрузить журнал действий.";
}

export type AuditLogsScreenProps = {
  organizationSlug: string;
  projectKey: string;
};

export function AuditLogsScreen({ organizationSlug, projectKey }: AuditLogsScreenProps) {
  const [dateRange, setDateRange] = useState<DateRange>(DEFAULT_DATE_RANGE);
  const [resourceType, setResourceType] = useState("");
  const [selectedLog, setSelectedLog] = useState<AuditLogDto | null>(null);

  const {
    auditLogs,
    total,
    isLoading,
    error,
    filters,
    setFilters,
  } = useAuditLogs(organizationSlug, projectKey);

  const handleDateRangeChange = (range: DateRange) => {
    setDateRange(range);
    setFilters((prev) => ({ ...prev, from: range.from, to: range.to, offset: 0 }));
  };

  const handleResourceTypeChange = (value: string) => {
    setResourceType(value);
    setFilters((prev) => ({ ...prev, resourceType: value || null, offset: 0 }));
  };

  const handleOffsetChange = (offset: number) => {
    setFilters((prev) => ({ ...prev, offset }));
  };

  return (
    <div className="mx-auto w-full max-w-[90rem] mt-6">
      <AuditLogsToolbar
        dateRange={dateRange}
        onDateRangeChange={handleDateRangeChange}
        resourceType={resourceType}
        onResourceTypeChange={handleResourceTypeChange}
        resourceTypes={RESOURCE_TYPES}
      />

      <div className="mt-6">
        {isLoading ? <AuditLogsSkeleton /> : null}

        {!isLoading && error ? (
          <div className="rounded-2xl border border-red-200 bg-red-50 p-5 text-sm text-red-700">
            {getErrorMessage(error)}
          </div>
        ) : null}

        {!isLoading && !error && auditLogs.length === 0 ? (
          <AuditLogsEmpty />
        ) : null}

        {!isLoading && !error && auditLogs.length > 0 ? (
          <>
            <AuditLogsTable logs={auditLogs} onSelect={setSelectedLog} />
            <AuditLogsPagination
              offset={filters.offset ?? 0}
              limit={filters.limit ?? 50}
              total={total}
              onOffsetChange={handleOffsetChange}
            />
          </>
        ) : null}
      </div>

      <AuditLogDetailDialog
        log={selectedLog}
        open={selectedLog !== null}
        onClose={() => setSelectedLog(null)}
      />
    </div>
  );
}
