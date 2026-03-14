"use client";

import { Button, IconPlus } from "@/components/shared";

export type ProcessesToolbarProps = {
  onCreate: () => void;
};

export function ProcessesToolbar({ onCreate }: ProcessesToolbarProps) {
  return (
    <div className="flex flex-wrap items-center justify-between gap-3">
      <h1 className="text-xl font-semibold text-zinc-900">Процессы</h1>
      <Button variant="primary" className="rounded-xl" onClick={onCreate}>
        <IconPlus className="h-4 w-4" aria-hidden="true" />
        Создать процесс
      </Button>
    </div>
  );
}
