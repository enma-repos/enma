"use client";

import { useState } from "react";
import { useOrganizations } from "@/hooks/useOrganizations";
import { OrganizationsEmpty } from "@/components/app/organizations/organizations-empty";
import { OrganizationsGrid } from "@/components/app/organizations/organizations-grid";
import { OrganizationsSkeleton } from "@/components/app/organizations/organizations-skeleton";
import { OrganizationsToolbar } from "@/components/app/organizations/organizations-toolbar";
import { CreateOrganizationDialog } from "@/components/app/organizations/create-organization-dialog";

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось загрузить организации.";
}

export function OrganizationsScreen() {
  const [isCreateOpen, setIsCreateOpen] = useState(false);
  const {
    isLoading,
    error,
    query,
    setQuery,
    filteredOrganizations,
    createOrganization,
    isCreating,
    createError,
  } = useOrganizations();

  return (
    <div className="mx-auto w-full max-w-6xl mt-6">
      <OrganizationsToolbar
        query={query}
        onQueryChange={setQuery}
        onCreate={() => setIsCreateOpen(true)}
      />

      <div className="mt-6">
        {isLoading ? <OrganizationsSkeleton /> : null}

        {!isLoading && error ? (
          <div className="rounded-2xl border border-red-200 bg-red-50 p-5 text-sm text-red-700">
            {getErrorMessage(error)}
          </div>
        ) : null}

        {!isLoading && !error && filteredOrganizations.length === 0 ? (
          <OrganizationsEmpty />
        ) : null}

        {!isLoading && !error && filteredOrganizations.length > 0 ? (
          <OrganizationsGrid organizations={filteredOrganizations} />
        ) : null}
      </div>

      <CreateOrganizationDialog
        open={isCreateOpen}
        onClose={() => setIsCreateOpen(false)}
        onCreate={createOrganization}
        isCreating={isCreating}
        error={createError}
      />
    </div>
  );
}
