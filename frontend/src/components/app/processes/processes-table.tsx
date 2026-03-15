"use client";

import { ProcessType } from "@/types/admin.types";
import type { ProcessDefinitionDto } from "@/types/admin.types";
import { Button, DataTable, IconTrash } from "@/components/shared";
import type { DataTableColumn } from "@/components/shared";

const processTypeLabels: Record<ProcessType, string> = {
  [ProcessType.Session]: "Сессия",
  [ProcessType.Order]: "Заказ",
  [ProcessType.Checkout]: "Оформление",
  [ProcessType.Ticket]: "Тикет",
  [ProcessType.Custom]: "Пользовательский",
};

function formatDate(value: string) {
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return value;
  return new Intl.DateTimeFormat("ru-RU", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  }).format(date);
}

export type ProcessesTableProps = {
  processes: ProcessDefinitionDto[];
  onSelect: (process: ProcessDefinitionDto) => void;
  onDelete: (process: ProcessDefinitionDto) => void;
};

const columns: DataTableColumn<ProcessDefinitionDto>[] = [
  {
    key: "name",
    header: "Название",
    render: (p) => <span className="font-medium text-zinc-900">{p.name}</span>,
  },
  {
    key: "key",
    header: "Ключ",
    render: (p) => <span className="font-mono text-zinc-500">{p.key}</span>,
  },
  {
    key: "type",
    header: "Тип",
    render: (p) => <span className="text-zinc-500">{processTypeLabels[p.type] ?? "—"}</span>,
  },
  {
    key: "createdAt",
    header: "Дата создания",
    render: (p) => <span className="text-zinc-400">{formatDate(p.createdAt)}</span>,
  },
];

export function ProcessesTable({ processes, onSelect, onDelete }: ProcessesTableProps) {
  return (
    <DataTable
      columns={columns}
      rows={processes}
      getRowKey={(p) => p.id}
      onRowClick={onSelect}
      renderActions={(process) => (
        <button
          type="button"
          className="inline-flex h-8 w-8 items-center justify-center rounded-lg p-0 cursor-pointer text-rose-400 transition-colors hover:bg-rose-50 hover:text-rose-600"
          onClick={() => onDelete(process)}
          aria-label={`Удалить ${process.name}`}
        >
          <IconTrash size={20} />
        </button>
      )}
    />
  );
}
