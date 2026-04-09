"use client";

import { useRouter } from "next/navigation";
import { Badge, DataTable } from "@/components/shared";
import type { DataTableColumn } from "@/components/shared";
import type { SuperOrganizationListItemDto } from "@/types/super-admin.types";
import { formatDate } from "@/components/app/admin/format";

export type AdminOrganizationsTableProps = {
  items: SuperOrganizationListItemDto[];
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

const columns: DataTableColumn<SuperOrganizationListItemDto>[] = [
  {
    key: "name",
    header: "Название",
    render: (o) => (
      <div className="flex flex-col">
        <span className="text-zinc-900">{o.name}</span>
        <span className="text-xs text-zinc-400">{o.slug}</span>
      </div>
    ),
  },
  {
    key: "owner",
    header: "Владелец",
    render: (o) => (
      <div className="flex flex-col">
        <span className="text-xs text-zinc-600">{o.ownerEmail ?? "—"}</span>
        {o.ownerDisplayName ? (
          <span className="text-xs text-zinc-400">{o.ownerDisplayName}</span>
        ) : null}
      </div>
    ),
  },
  {
    key: "members",
    header: "Участников",
    render: (o) => (
      <span className="text-zinc-600 tabular-nums">{o.memberCount}</span>
    ),
  },
  {
    key: "projects",
    header: "Проектов",
    render: (o) => (
      <span className="text-zinc-600 tabular-nums">{o.projectCount}</span>
    ),
  },
  {
    key: "createdAt",
    header: "Создана",
    render: (o) => (
      <span className="text-xs text-zinc-400">{formatDate(o.createdAt)}</span>
    ),
  },
  {
    key: "status",
    header: "Статус",
    render: (o) =>
      o.deletedAt ? (
        <Badge className="bg-rose-50 text-rose-700 ring-rose-200">Удалена</Badge>
      ) : (
        <Badge tone="positive">Активна</Badge>
      ),
  },
];

export function AdminOrganizationsTable(props: AdminOrganizationsTableProps) {
  const router = useRouter();

  return (
    <DataTable
      columns={columns}
      rows={props.items}
      getRowKey={(o) => o.id}
      onRowClick={(o) => router.push(`/admin/organizations/${o.id}`)}
      isLoading={props.isLoading}
      emptyMessage="Организации не найдены"
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
