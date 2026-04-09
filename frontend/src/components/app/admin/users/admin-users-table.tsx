"use client";

import { useRouter } from "next/navigation";
import { Badge, DataTable } from "@/components/shared";
import type { DataTableColumn } from "@/components/shared";
import type { SuperUserListItemDto } from "@/types/super-admin.types";
import { formatDate } from "@/components/app/admin/format";

export type AdminUsersTableProps = {
  items: SuperUserListItemDto[];
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

const columns: DataTableColumn<SuperUserListItemDto>[] = [
  {
    key: "email",
    header: "Email",
    render: (u) => (
      <div className="flex flex-col">
        <span className="text-zinc-900">{u.email}</span>
        <span className="text-xs text-zinc-400">{u.displayName}</span>
      </div>
    ),
  },
  {
    key: "orgs",
    header: "Организаций",
    render: (u) => (
      <span className="text-zinc-600 tabular-nums">{u.organizationCount}</span>
    ),
  },
  {
    key: "projects",
    header: "Проектов",
    render: (u) => (
      <span className="text-zinc-600 tabular-nums">{u.projectCount}</span>
    ),
  },
  {
    key: "createdAt",
    header: "Создан",
    render: (u) => (
      <span className="text-xs text-zinc-400">{formatDate(u.createdAt)}</span>
    ),
  },
  {
    key: "status",
    header: "Статус",
    render: (u) =>
      u.deletedAt ? (
        <Badge className="bg-rose-50 text-rose-700 ring-rose-200">Удалён</Badge>
      ) : (
        <Badge tone="positive">Активен</Badge>
      ),
  },
];

export function AdminUsersTable(props: AdminUsersTableProps) {
  const router = useRouter();

  return (
    <DataTable
      columns={columns}
      rows={props.items}
      getRowKey={(u) => u.id}
      onRowClick={(u) => router.push(`/admin/users/${u.id}`)}
      isLoading={props.isLoading}
      emptyMessage="Пользователи не найдены"
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
