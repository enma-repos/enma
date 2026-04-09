"use client";

import { useSuperUsers } from "@/hooks/admin/useSuperUsers";
import { AdminToolbar } from "@/components/app/admin/admin-toolbar";
import { AdminUsersTable } from "@/components/app/admin/users/admin-users-table";

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось загрузить список пользователей.";
}

export function AdminUsersScreen() {
  const {
    items,
    total,
    totalPages,
    page,
    pageSize,
    search,
    includeDeleted,
    isLoading,
    error,
    setPage,
    setPageSize,
    setSearch,
    setIncludeDeleted,
  } = useSuperUsers();

  return (
    <div className="mx-auto w-full max-w-[90rem]">
      <AdminToolbar
        title="Пользователи"
        description="Все учётные записи платформы."
        total={total}
        includeDeleted={includeDeleted}
        onToggleIncludeDeleted={setIncludeDeleted}
      />

      {error ? (
        <div className="mb-4 rounded-2xl border border-red-200 bg-red-50 p-5 text-sm text-red-700">
          {getErrorMessage(error)}
        </div>
      ) : null}

      <AdminUsersTable
        items={items}
        isLoading={isLoading}
        page={page}
        totalPages={totalPages}
        totalCount={total}
        pageSize={pageSize}
        search={search}
        onPageChange={setPage}
        onPageSizeChange={setPageSize}
        onSearchChange={setSearch}
      />
    </div>
  );
}
