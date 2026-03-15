"use client";

import type { EventDefinitionDto } from "@/types/admin.types";
import { Button, DataTable, IconTrash } from "@/components/shared";
import type { DataTableColumn } from "@/components/shared";

function formatDate(value: string) {
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return value;
  return new Intl.DateTimeFormat("ru-RU", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  }).format(date);
}

export type EventsTableProps = {
  events: EventDefinitionDto[];
  onSelect: (event: EventDefinitionDto) => void;
  onDelete: (event: EventDefinitionDto) => void;
};

const columns: DataTableColumn<EventDefinitionDto>[] = [
  {
    key: "name",
    header: "Название",
    render: (e) => <span className="font-medium text-zinc-900">{e.name}</span>,
  },
  {
    key: "description",
    header: "Описание",
    render: (e) => <span className="text-zinc-500">{e.description ?? "—"}</span>,
  },
  {
    key: "createdAt",
    header: "Дата создания",
    render: (e) => <span className="text-zinc-400">{formatDate(e.createdAt)}</span>,
  },
];

export function EventsTable({ events, onSelect, onDelete }: EventsTableProps) {
  return (
    <DataTable
      columns={columns}
      rows={events}
      getRowKey={(e) => e.id}
      onRowClick={onSelect}
      renderActions={(event) => (
        <button
          type="button"
          className="inline-flex h-8 w-8 items-center justify-center rounded-lg p-0 cursor-pointer text-rose-400 transition-colors hover:bg-rose-50 hover:text-rose-600"
          onClick={() => onDelete(event)}
          aria-label={`Удалить ${event.name}`}
        >
          <IconTrash size={20} />
        </button>
      )}
    />
  );
}
