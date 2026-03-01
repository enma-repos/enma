 "use client";

import { Avatar, Button, IconBell, IconChevronDown, IconGrid, IconMoon, IconSearch, Input } from "@/components/shared";

export type AppTopbarProps = {
  displayName: string | null;
};

function getInitials(displayName: string | null) {
  if (!displayName) return "U";
  const parts = displayName.trim().split(/\s+/).filter(Boolean);
  const first = parts[0]?.[0] ?? "";
  const second = parts.length > 1 ? parts[1]?.[0] ?? "" : (parts[0]?.[1] ?? "");
  const initials = (first + second).toUpperCase();
  return initials || "U";
}

export function AppTopbar({ displayName }: AppTopbarProps) {
  const initials = getInitials(displayName);

  return (
    <header className="sticky top-0 z-30 border-b border-zinc-200 bg-zinc-50/80 backdrop-blur">
      <div className="flex h-16 items-center gap-4 px-6">
        <Button
          variant="ghost"
          size="sm"
          className="h-10 w-10 rounded-xl p-0"
          aria-label="Open menu"
        >
          <IconGrid className="h-5 w-5" />
        </Button>

        <div className="relative w-full max-w-md">
          <IconSearch className="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-zinc-400" />
          <Input placeholder="Type to search" className="pl-9" />
        </div>

        <div className="ml-auto flex items-center gap-1">
          <Button variant="ghost" size="sm" className="rounded-xl">
            <span className="text-sm font-semibold tabular-nums">3482$</span>
            <IconChevronDown className="h-4 w-4 text-zinc-500" />
          </Button>

          {displayName ? (
              <Button variant="ghost" size="sm" className="max-w-[220px] rounded-xl">
              <span className="truncate text-sm font-medium text-zinc-700">
                {displayName}
              </span>
                <IconChevronDown className="h-4 w-4 text-zinc-500" aria-hidden="true" />
              </Button>
          ) : null}

          <Button
              variant="ghost"
              size="sm"
              className="h-10 w-10 rounded-xl p-0"
              aria-label="Notifications"
          >
            <IconBell className="h-5 w-5" />
          </Button>



          <Avatar initials={initials} aria-label="User menu" />
        </div>
      </div>
    </header>
  );
}
