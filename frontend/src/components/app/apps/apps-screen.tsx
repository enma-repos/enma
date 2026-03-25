"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import type { SdkClientDto } from "@/types/admin.types";
import { useSdkClients } from "@/hooks/useSdkClients";
import { AppsEmpty } from "@/components/app/apps/apps-empty";
import { AppsSkeleton } from "@/components/app/apps/apps-skeleton";
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
    createClient,
    isCreating,
    createError,
  } = useSdkClients(organizationSlug, projectKey);

  const handleSelect = (app: SdkClientDto) => {
    router.push(`/app/organizations/${organizationSlug}/projects/${projectKey}/apps/${app.id}`);
  };

  return (
    <div className="mx-auto w-full max-w-[90rem] mt-6">
      <AppsToolbar onCreate={() => setIsCreateOpen(true)} />

      <div className="mt-6">
        {isLoading ? <AppsSkeleton /> : null}

        {!isLoading && error ? (
          <div className="rounded-2xl border border-red-200 bg-red-50 p-5 text-sm text-red-700">
            {getErrorMessage(error)}
          </div>
        ) : null}

        {!isLoading && !error && sdkClients.length === 0 ? (
          <AppsEmpty />
        ) : null}

        {!isLoading && !error && sdkClients.length > 0 ? (
          <AppsTable
            apps={sdkClients}
            onSelect={handleSelect}
          />
        ) : null}
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
