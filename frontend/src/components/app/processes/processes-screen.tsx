"use client";

import { useState } from "react";
import type { ProcessDefinitionDto } from "@/types/admin.types";
import { useProcessDefinitions } from "@/hooks/useProcessDefinitions";
import { ConfirmDialog } from "@/components/shared";
import { ProcessesTable } from "@/components/app/processes/processes-table";
import { ProcessesToolbar } from "@/components/app/processes/processes-toolbar";
import { CreateProcessDialog } from "@/components/app/processes/create-process-dialog";
import { ProcessDetailDialog } from "@/components/app/processes/process-detail-dialog";

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось загрузить процессы.";
}

export type ProcessesScreenProps = {
  organizationSlug: string;
  projectKey: string;
};

export function ProcessesScreen({ organizationSlug, projectKey }: ProcessesScreenProps) {
  const [isCreateOpen, setIsCreateOpen] = useState(false);
  const [selectedProcess, setSelectedProcess] = useState<ProcessDefinitionDto | null>(null);
  const [processToDelete, setProcessToDelete] = useState<ProcessDefinitionDto | null>(null);

  const {
    processDefinitions,
    page,
    setPage,
    totalPages,
    totalCount,
    isLoading,
    error,
    createProcess,
    isCreating,
    createError,
    deleteProcess,
    isDeleting,
    setProcessName,
    isSavingName,
    setProcessDescription,
    isSavingDescription,
    pageSize,
    setPageSize,
    search,
    setSearch,
  } = useProcessDefinitions(organizationSlug, projectKey);

  const handleDelete = async () => {
    if (!processToDelete) return;
    try {
      await deleteProcess(processToDelete.id);
      setProcessToDelete(null);
    } catch {
      // error stays visible via isDeleting state
    }
  };

  return (
    <div className="mx-auto w-full max-w-[90rem] mt-6">
      <ProcessesToolbar onCreate={() => setIsCreateOpen(true)} />

      <div className="mt-6">
        {error ? (
          <div className="rounded-2xl border border-red-200 bg-red-50 p-5 text-sm text-red-700 mb-4">
            {getErrorMessage(error)}
          </div>
        ) : null}

        <ProcessesTable
          processes={processDefinitions}
          onSelect={setSelectedProcess}
          onDelete={setProcessToDelete}
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

      <CreateProcessDialog
        open={isCreateOpen}
        onClose={() => setIsCreateOpen(false)}
        onCreate={createProcess}
        isCreating={isCreating}
        error={createError}
      />

      <ProcessDetailDialog
        process={selectedProcess}
        open={selectedProcess !== null}
        onClose={() => setSelectedProcess(null)}
        onSetName={setProcessName}
        onSetDescription={setProcessDescription}
        isSavingName={isSavingName}
        isSavingDescription={isSavingDescription}
      />

      <ConfirmDialog
        open={processToDelete !== null}
        onClose={() => setProcessToDelete(null)}
        onConfirm={handleDelete}
        title="Удалить процесс"
        description={`Вы уверены, что хотите удалить процесс «${processToDelete?.name ?? ""}»? Это действие нельзя отменить.`}
        confirmLabel="Удалить"
        isConfirming={isDeleting}
      />
    </div>
  );
}
