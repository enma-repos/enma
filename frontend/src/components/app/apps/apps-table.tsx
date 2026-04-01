"use client";

import { SdkClientType } from "@/types/admin.types";
import type { SdkClientDto } from "@/types/admin.types";
import { DataTable } from "@/components/shared";
import type { DataTableColumn } from "@/components/shared";

const clientTypeLabels: Record<SdkClientType, string> = {
  [SdkClientType.WebsiteSdk]: "Website SDK",
  [SdkClientType.ServerToServer]: "Server-to-Server",
  [SdkClientType.MobileSdk]: "Mobile SDK",
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

export type AppsTableProps = {
  apps: SdkClientDto[];
  onSelect: (app: SdkClientDto) => void;
  isLoading?: boolean;
  page: number;
  totalPages: number;
  onPageChange: (page: number) => void;
  pageSize: number;
  onPageSizeChange: (pageSize: number) => void;
  totalCount: number;
  search: string;
  onSearchChange: (value: string) => void;
};

const columns: DataTableColumn<SdkClientDto>[] = [
  {
    key: "name",
    header: "Название",
    render: (a) => <span className="font-medium text-zinc-900">{a.name}</span>,
  },
  {
    key: "type",
    header: "Тип",
    render: (a) => <span className="text-zinc-500">{clientTypeLabels[a.type] ?? "—"}</span>,
  },
  {
    key: "description",
    header: "Описание",
    render: (a) => <span className="text-zinc-500 truncate max-w-xs block">{a.description ?? "—"}</span>,
  },
  {
    key: "createdAt",
    header: "Дата создания",
    render: (a) => <span className="text-zinc-400">{formatDate(a.createdAt)}</span>,
  },
];

export function AppsTable({ apps, onSelect, isLoading, page, totalPages, onPageChange, pageSize, onPageSizeChange, totalCount, search, onSearchChange }: AppsTableProps) {
  return (
    <DataTable
      columns={columns}
      rows={apps}
      getRowKey={(a) => a.id}
      onRowClick={onSelect}
      isLoading={isLoading}
      emptyMessage="Приложения не найдены"
      page={page}
      totalPages={totalPages}
      onPageChange={onPageChange}
      pageSize={pageSize}
      onPageSizeChange={onPageSizeChange}
      totalCount={totalCount}
      search={search}
      onSearchChange={onSearchChange}
    />
  );
}
