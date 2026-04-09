"use client";

import { useSuperOrganizations } from "@/hooks/admin/useSuperOrganizations";
import { AdminToolbar } from "@/components/app/admin/admin-toolbar";
import { AdminOrganizationsTable } from "@/components/app/admin/organizations/admin-organizations-table";

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось загрузить список организаций.";
}

export function AdminOrganizationsScreen() {
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
  } = useSuperOrganizations();

  return (
    <div className="mx-auto w-full max-w-[90rem]">
      <AdminToolbar
        title="Организации"
        description="Все организации платформы."
        total={total}
        includeDeleted={includeDeleted}
        onToggleIncludeDeleted={setIncludeDeleted}
      />

      {error ? (
        <div className="mb-4 rounded-2xl border border-red-200 bg-red-50 p-5 text-sm text-red-700">
          {getErrorMessage(error)}
        </div>
      ) : null}

      <AdminOrganizationsTable
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
