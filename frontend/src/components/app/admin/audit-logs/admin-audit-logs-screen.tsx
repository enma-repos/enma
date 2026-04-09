"use client";

import { useState } from "react";
import type { AuditLogDto } from "@/types/admin.types";
import { useSuperAuditLogs } from "@/hooks/admin/useSuperAuditLogs";
import { AuditLogDetailDialog } from "@/components/app/audit-logs/audit-log-detail-dialog";
import { AdminAuditLogsTable } from "@/components/app/admin/audit-logs/admin-audit-logs-table";

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось загрузить журнал действий.";
}

export function AdminAuditLogsScreen() {
  const [selectedLog, setSelectedLog] = useState<AuditLogDto | null>(null);

  const {
    items,
    total,
    totalPages,
    page,
    pageSize,
    search,
    isLoading,
    error,
    setPage,
    setPageSize,
    setSearch,
  } = useSuperAuditLogs();

  return (
    <div className="mx-auto w-full max-w-[90rem]">
      <div className="mb-5">
        <h1 className="text-xl font-semibold text-zinc-900">
          Аудит-логи
          <span className="ml-2 text-sm font-normal text-zinc-400">
            {total.toLocaleString("ru-RU")}
          </span>
        </h1>
        <p className="mt-1 text-sm text-zinc-500">Все события платформы.</p>
      </div>

      {error ? (
        <div className="mb-4 rounded-2xl border border-red-200 bg-red-50 p-5 text-sm text-red-700">
          {getErrorMessage(error)}
        </div>
      ) : null}

      <AdminAuditLogsTable
        logs={items}
        onSelect={setSelectedLog}
        isLoading={isLoading}
        page={page}
        totalPages={totalPages}
        onPageChange={setPage}
        pageSize={pageSize}
        onPageSizeChange={setPageSize}
        totalCount={total}
        search={search}
        onSearchChange={setSearch}
      />

      <AuditLogDetailDialog
        log={selectedLog}
        open={selectedLog !== null}
        onClose={() => setSelectedLog(null)}
      />
    </div>
  );
}
