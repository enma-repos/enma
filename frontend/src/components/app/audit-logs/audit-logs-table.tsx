"use client";

import type { AuditLogDto } from "@/types/admin.types";
import { DataTable } from "@/components/shared";
import type { DataTableColumn } from "@/components/shared";

function formatDateTime(value: string) {
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return value;
  return new Intl.DateTimeFormat("ru-RU", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
    hour: "2-digit",
    minute: "2-digit",
    second: "2-digit",
  }).format(date);
}

const ACTION_COLORS: Record<string, string> = {
  create: "bg-emerald-50 text-emerald-700",
  delete: "bg-rose-50 text-rose-700",
  remove: "bg-rose-50 text-rose-700",
  revoke: "bg-amber-50 text-amber-700",
  archive: "bg-zinc-100 text-zinc-600",
  disable: "bg-amber-50 text-amber-700",
};

function actionColor(action: string): string {
  const base = action.split(".")[0];
  return ACTION_COLORS[base] ?? "bg-zinc-100 text-zinc-700";
}

function truncateId(id: string) {
  if (id.length <= 12) return id;
  return `${id.slice(0, 8)}…`;
}

export type AuditLogsTableProps = {
  logs: AuditLogDto[];
  onSelect: (log: AuditLogDto) => void;
};

const columns: DataTableColumn<AuditLogDto>[] = [
  {
    key: "action",
    header: "Действие",
    render: (log) => (
      <span
        className={`inline-block rounded-md px-2 py-0.5 text-xs font-medium ${actionColor(log.action)}`}
      >
        {log.action}
      </span>
    ),
  },
  {
    key: "resourceType",
    header: "Ресурс",
    render: (log) => (
      <span className="text-zinc-700">
        {log.resourceType}{" "}
        <span className="text-zinc-400 font-mono text-xs">{truncateId(log.resourceId)}</span>
      </span>
    ),
  },
  {
    key: "actorUserId",
    header: "Актор",
    render: (log) => (
      <span className="text-zinc-500 font-mono text-xs">
        {log.actorUserId ? truncateId(log.actorUserId) : "—"}
      </span>
    ),
  },
  {
    key: "ip",
    header: "IP",
    render: (log) => (
      <span className="text-zinc-400 text-xs">{log.ip ?? "—"}</span>
    ),
  },
  {
    key: "createdAt",
    header: "Дата",
    render: (log) => (
      <span className="text-zinc-400 text-xs whitespace-nowrap">{formatDateTime(log.createdAt)}</span>
    ),
  },
];

export function AuditLogsTable({ logs, onSelect }: AuditLogsTableProps) {
  return (
    <DataTable
      columns={columns}
      rows={logs}
      getRowKey={(log) => log.id}
      onRowClick={onSelect}
    />
  );
}
