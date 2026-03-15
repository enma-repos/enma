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
        <Button
          variant="ghost"
          size="sm"
          className="h-8 w-8 p-0 text-zinc-400 hover:text-red-600"
          onClick={() => onDelete(event)}
          aria-label={`Удалить ${event.name}`}
        >
          <IconTrash className="h-4 w-4" />
        </Button>
      )}
    />
  );
}
