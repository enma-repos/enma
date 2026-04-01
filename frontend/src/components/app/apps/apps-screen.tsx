"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import type { SdkClientDto } from "@/types/admin.types";
import { useSdkClients } from "@/hooks/useSdkClients";
import { AppsTable } from "@/components/app/apps/apps-table";
import { AppsToolbar } from "@/components/app/apps/apps-toolbar";
import { CreateAppDialog } from "@/components/app/apps/create-app-dialog";

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось загрузить приложения.";
}

export type AppsScreenProps = {
  organizationSlug: string;
  projectKey: string;
};

export function AppsScreen({ organizationSlug, projectKey }: AppsScreenProps) {
  const router = useRouter();
  const [isCreateOpen, setIsCreateOpen] = useState(false);

  const {
    sdkClients,
    isLoading,
    error,
    page,
    setPage,
    totalPages,
    totalCount,
    createClient,
    isCreating,
    createError,
    pageSize,
    setPageSize,
    search,
    setSearch,
  } = useSdkClients(organizationSlug, projectKey);

  const handleSelect = (app: SdkClientDto) => {
    router.push(`/app/organizations/${organizationSlug}/projects/${projectKey}/apps/${app.id}`);
  };

  return (
    <div className="mx-auto w-full max-w-[90rem] mt-6">
      <AppsToolbar onCreate={() => setIsCreateOpen(true)} />

      <div className="mt-6">
        {error ? (
          <div className="rounded-2xl border border-red-200 bg-red-50 p-5 text-sm text-red-700 mb-4">
            {getErrorMessage(error)}
          </div>
        ) : null}

        <AppsTable
          apps={sdkClients}
          onSelect={handleSelect}
          isLoading={isLoading}
          page={page}
          totalPages={totalPages}
          onPageChange={setPage}
          pageSize={pageSize}
          onPageSizeChange={setPageSize}
          totalCount={totalCount}
          search={search}
          onSearchChange={setSearch}
        />
      </div>

      <CreateAppDialog
        open={isCreateOpen}
        onClose={() => setIsCreateOpen(false)}
        onCreate={createClient}
        isCreating={isCreating}
        error={createError}
      />
    </div>
  );
}
