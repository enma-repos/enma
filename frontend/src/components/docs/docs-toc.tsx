"use client";

import { cn } from "@/components/shared/cn";

export type TocItem = {
  id: string;
  title: string;
  level: number;
};

type DocsTocProps = {
  items: TocItem[];
  activeId?: string;
};

export function DocsToc({ items, activeId }: DocsTocProps) {
  if (items.length === 0) return null;

  return (
    <aside className="hidden w-56 shrink-0 xl:block">
      <div className="sticky top-14 py-6 pr-4">
        <h4 className="mb-3 text-xs font-semibold uppercase tracking-wider text-zinc-500">
          На этой странице
        </h4>
        <ul className="space-y-1.5 text-sm">
          {items.map((item) => (
            <li key={item.id}>
              <a
                href={`#${item.id}`}
                className={cn(
                  "block transition-colors",
                  item.level > 2 && "pl-3",
                  activeId === item.id
                    ? "font-medium text-zinc-900"
                    : "text-zinc-500 hover:text-zinc-900",
                )}
              >
                {item.title}
              </a>
            </li>
          ))}
        </ul>
      </div>
    </aside>
  );
}
