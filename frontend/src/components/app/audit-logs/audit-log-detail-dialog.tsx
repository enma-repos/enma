"use client";

import type { AuditLogDto } from "@/types/admin.types";
import { IconX, Modal } from "@/components/shared";

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

function Field({ label, value }: { label: string; value: string | null }) {
  return (
    <div>
      <dt className="text-xs text-zinc-500">{label}</dt>
      <dd className="mt-0.5 text-sm text-zinc-900 font-mono break-all">{value ?? "—"}</dd>
    </div>
  );
}

export type AuditLogDetailDialogProps = {
  log: AuditLogDto | null;
  open: boolean;
  onClose: () => void;
};

export function AuditLogDetailDialog({ log, open, onClose }: AuditLogDetailDialogProps) {
  if (!log) return null;

  return (
    <Modal open={open} onClose={onClose}>
      <div className="p-6">
      <div className="flex items-center justify-between">
        <h2 className="text-lg font-semibold text-zinc-900">Детали записи</h2>
        <button
          type="button"
          onClick={onClose}
          className="inline-flex h-8 w-8 items-center justify-center rounded-lg text-zinc-400 transition-colors hover:bg-zinc-100 hover:text-zinc-600 cursor-pointer"
          aria-label="Закрыть"
        >
          <IconX size={20} />
        </button>
      </div>
      <dl className="space-y-4 mt-4">
        <Field label="Действие" value={log.action} />
        <Field label="Тип ресурса" value={log.resourceType} />
        <Field label="ID ресурса" value={log.resourceId} />
        <Field label="Актор (userId)" value={log.actorUserId} />
        <Field label="IP-адрес" value={log.ip} />
        <Field label="Организация" value={log.organizationId} />
        <Field label="Проект" value={log.projectId} />
        <Field label="Дата" value={formatDateTime(log.createdAt)} />
        {log.payload ? (
          <div>
            <dt className="text-xs text-zinc-500">Payload</dt>
            <dd className="mt-1">
              <pre className="rounded-lg bg-zinc-50 p-3 text-xs text-zinc-700 overflow-x-auto">
                {JSON.stringify(log.payload, null, 2)}
              </pre>
            </dd>
          </div>
        ) : null}
      </dl>
      </div>
    </Modal>
  );
}
