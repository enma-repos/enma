"use client";

import { useRouter } from "next/navigation";
import { Badge, DataTable } from "@/components/shared";
import type { DataTableColumn } from "@/components/shared";
import type { SuperProjectListItemDto } from "@/types/super-admin.types";
import { formatDate } from "@/components/app/admin/format";

export type AdminProjectsTableProps = {
  items: SuperProjectListItemDto[];
  isLoading?: boolean;
  page: number;
  totalPages: number;
  totalCount: number;
  pageSize: number;
  search: string;
  onPageChange: (page: number) => void;
  onPageSizeChange: (pageSize: number) => void;
  onSearchChange: (value: string) => void;
};

const columns: DataTableColumn<SuperProjectListItemDto>[] = [
  {
    key: "name",
    header: "Название",
    render: (p) => (
      <div className="flex flex-col">
        <span className="text-zinc-900">{p.name}</span>
        <span className="text-xs text-zinc-400">{p.key}</span>
      </div>
    ),
  },
  {
    key: "organization",
    header: "Организация",
    render: (p) => (
      <div className="flex flex-col">
        <span className="text-xs text-zinc-600">{p.organizationName}</span>
        <span className="text-xs text-zinc-400">{p.organizationSlug}</span>
      </div>
    ),
  },
  {
    key: "members",
    header: "Участников",
    render: (p) => (
      <span className="text-zinc-600 tabular-nums">{p.memberCount}</span>
    ),
  },
  {
    key: "createdAt",
    header: "Создан",
    render: (p) => (
      <span className="text-xs text-zinc-400">{formatDate(p.createdAt)}</span>
    ),
  },
  {
    key: "status",
    header: "Статус",
    render: (p) =>
      p.deletedAt ? (
        <Badge className="bg-rose-50 text-rose-700 ring-rose-200">Удалён</Badge>
      ) : p.archivedAt ? (
        <Badge className="bg-zinc-100 text-zinc-600 ring-zinc-200">Архив</Badge>
      ) : (
        <Badge tone="positive">Активен</Badge>
      ),
  },
];

export function AdminProjectsTable(props: AdminProjectsTableProps) {
  const router = useRouter();

  return (
    <DataTable
      columns={columns}
      rows={props.items}
      getRowKey={(p) => p.id}
      onRowClick={(p) => router.push(`/admin/projects/${p.id}`)}
      isLoading={props.isLoading}
      emptyMessage="Проекты не найдены"
      page={props.page}
      totalPages={props.totalPages}
      totalCount={props.totalCount}
      pageSize={props.pageSize}
      onPageChange={props.onPageChange}
      onPageSizeChange={props.onPageSizeChange}
      search={props.search}
      onSearchChange={props.onSearchChange}
    />
  );
}
