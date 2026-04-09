"use client";

import type { ReactNode } from "react";

export type AdminToolbarProps = {
  title: string;
  description?: string;
  total?: number;
  includeDeleted: boolean;
  onToggleIncludeDeleted: (value: boolean) => void;
  extra?: ReactNode;
};

export function AdminToolbar({
  title,
  description,
  total,
  includeDeleted,
  onToggleIncludeDeleted,
  extra,
}: AdminToolbarProps) {
  return (
    <div className="mb-5 flex flex-col gap-3 sm:flex-row sm:items-start sm:justify-between">
      <div>
        <h1 className="text-xl font-semibold text-zinc-900">
          {title}
          {typeof total === "number" ? (
            <span className="ml-2 text-sm font-normal text-zinc-400">
              {total.toLocaleString("ru-RU")}
            </span>
          ) : null}
        </h1>
        {description ? (
          <p className="mt-1 text-sm text-zinc-500">{description}</p>
        ) : null}
      </div>

      <div className="flex items-center gap-3">
        {extra}
        <label className="inline-flex cursor-pointer items-center gap-2 text-sm text-zinc-600">
          <input
            type="checkbox"
            checked={includeDeleted}
            onChange={(e) => onToggleIncludeDeleted(e.target.checked)}
            className="h-4 w-4 rounded border-zinc-300 text-zinc-900 focus:ring-zinc-500"
          />
          Показать удалённых
        </label>
      </div>
    </div>
  );
}
