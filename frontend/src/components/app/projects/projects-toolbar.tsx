"use client";

import Link from "next/link";
import { Badge, Button, IconChevronDown, IconPlus, IconSearch, Input, cn } from "@/components/shared";

export type ProjectsToolbarProps = {
  title: string;
  organizationSlug: string;
  query: string;
  onQueryChange: (value: string) => void;
  onCreate: () => void;
};

export function ProjectsToolbar({
  title,
  organizationSlug,
  query,
  onQueryChange,
  onCreate,
}: ProjectsToolbarProps) {
  return (
    <div>
      <div className="flex flex-wrap items-center justify-between gap-3">
        <div className="min-w-0">
          <div className="flex flex-wrap items-center gap-2">
            <h1 className="truncate text-xl font-semibold text-zinc-900">
              {title}
            </h1>
            <Badge className="max-w-full truncate">
              <span className="font-mono">{organizationSlug}</span>
            </Badge>
          </div>
          <div className="mt-2 flex flex-wrap items-center gap-2 text-sm text-zinc-500">
            <Link
              href="/app/organizations"
              className={cn(
                "inline-flex items-center gap-1 rounded-lg px-2 py-1 hover:bg-zinc-100 hover:text-zinc-900",
              )}
            >
              Организации
              <IconChevronDown className="h-4 w-4 -rotate-90" aria-hidden="true" />
            </Link>
            <span className="text-zinc-300" aria-hidden="true">
              /
            </span>
            <span className="font-medium text-zinc-700">Проекты</span>
          </div>
        </div>

        <Button variant="primary" className="rounded-xl" onClick={onCreate}>
          <IconPlus className="h-4 w-4" aria-hidden="true" />
          Создать проект
        </Button>
      </div>

      <div className="relative mt-4 w-full max-w-sm">
        <IconSearch className="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-zinc-400" />
        <Input
          value={query}
          onChange={(e) => onQueryChange(e.currentTarget.value)}
          placeholder="Поиск по названию или ключу"
          className="pl-9"
        />
      </div>
    </div>
  );
}

