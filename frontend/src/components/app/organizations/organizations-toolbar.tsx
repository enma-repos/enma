"use client";

import { Button, IconPlus, IconSearch, Input } from "@/components/shared";

export type OrganizationsToolbarProps = {
  query: string;
  onQueryChange: (value: string) => void;
  onCreate: () => void;
};

export function OrganizationsToolbar({
  query,
  onQueryChange,
  onCreate,
}: OrganizationsToolbarProps) {
  return (
    <div>
      <div className="flex flex-wrap items-center justify-between gap-3">
        <h1 className="text-xl font-semibold text-zinc-900">Организации</h1>
        <Button variant="primary" className="rounded-xl" onClick={onCreate}>
          <IconPlus className="h-4 w-4" aria-hidden="true" />
          Создать организацию
        </Button>
      </div>

      <div className="relative mt-4 w-full max-w-sm">
        <IconSearch className="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-zinc-400" />
        <Input
          value={query}
          onChange={(e) => onQueryChange(e.currentTarget.value)}
          placeholder="Поиск"
          className="pl-9"
        />
      </div>
    </div>
  );
}
