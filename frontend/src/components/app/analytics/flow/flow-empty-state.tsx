"use client";

import { IconChart } from "@/components/shared";

export function FlowEmptyState({ message }: { message?: string }) {
  return (
    <div className="grid h-full min-h-[400px] place-items-center rounded-2xl border border-dashed border-zinc-200 bg-zinc-50">
      <div className="flex flex-col items-center gap-3 text-zinc-400">
        <IconChart size={32} />
        <p className="text-sm">{message ?? "Нет данных для отображения графа"}</p>
      </div>
    </div>
  );
}
