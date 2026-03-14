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
        <Button
          variant="ghost"
          size="sm"
          className="h-8 w-8 p-0 text-zinc-400 hover:text-red-600"
          onClick={() => onDelete(process)}
          aria-label={`Удалить ${process.name}`}
        >
          <IconTrash className="h-4 w-4" />
        </Button>
      )}
    />
  );
}
