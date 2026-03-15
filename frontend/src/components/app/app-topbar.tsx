 "use client";

import { useCallback, useEffect, useRef, useState } from "react";
import { Avatar, Button, IconBell, IconChevronDown, IconGrid, IconSearch, Input } from "@/components/shared";

export type AppTopbarProps = {
  displayName: string | null;
  onLogout: () => void;
};

function getInitials(displayName: string | null) {
  if (!displayName) return "U";
  const parts = displayName.trim().split(/\s+/).filter(Boolean);
  const first = parts[0]?.[0] ?? "";
  const second = parts.length > 1 ? parts[1]?.[0] ?? "" : (parts[0]?.[1] ?? "");
  const initials = (first + second).toUpperCase();
  return initials || "U";
}

export function AppTopbar({ displayName, onLogout }: AppTopbarProps) {
  const initials = getInitials(displayName);
  const [menuOpen, setMenuOpen] = useState(false);
  const menuRef = useRef<HTMLDivElement>(null);

  const toggleMenu = useCallback(() => setMenuOpen((prev) => !prev), []);

  useEffect(() => {
    if (!menuOpen) return;
    function handleClickOutside(e: MouseEvent) {
      if (menuRef.current && !menuRef.current.contains(e.target as Node)) {
        setMenuOpen(false);
      }
    }
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, [menuOpen]);

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
          {displayName ? (
            <div className="relative" ref={menuRef}>
              <Button
                variant="ghost"
                size="sm"
                className="max-w-[220px] cursor-pointer rounded-xl"
                onClick={toggleMenu}
              >
                <span className="truncate text-sm font-medium text-zinc-700">
                  {displayName}
                </span>
                <IconChevronDown className="h-4 w-4 text-zinc-500" aria-hidden="true" />
              </Button>

              {menuOpen && (
                <div className="absolute right-0 top-full mt-1 min-w-[160px] rounded-xl border border-zinc-200 bg-white py-1 shadow-lg">
                  <button
                    type="button"
                    className="w-full cursor-pointer px-4 py-2 text-left text-sm text-zinc-700 hover:bg-zinc-100"
                    onClick={() => {
                      setMenuOpen(false);
                      onLogout();
                    }}
                  >
                    Logout
                  </button>
                </div>
              )}
            </div>
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
