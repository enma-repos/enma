"use client";

import { LogOut, ShieldCheck } from "lucide-react";
import { Avatar } from "@/components/shared";
import { useMe } from "@/hooks/useMe";
import { useLogout } from "@/hooks/useLogout";

function getInitials(name: string | null, email: string | null) {
  const source = name || email || "";
  const parts = source.trim().split(/\s+/).filter(Boolean);
  const first = parts[0]?.[0] ?? "";
  const second = parts.length > 1 ? (parts[1]?.[0] ?? "") : (parts[0]?.[1] ?? "");
  const initials = (first + second).toUpperCase();
  return initials || "U";
}

export function AdminTopbar() {
  const { data } = useMe();
  const logout = useLogout();

  const displayName = data?.user?.displayName ?? null;
  const email = data?.account?.email ?? null;
  const initials = getInitials(displayName, email);

  return (
    <header className="flex h-14 items-center justify-between border-b border-zinc-200 bg-white px-6">
      <div className="flex items-center gap-2 text-sm font-medium text-zinc-700">
        <ShieldCheck className="h-4 w-4 text-zinc-500" />
        <span>Панель суперадмина</span>
      </div>

      <div className="flex items-center gap-3">
        <div className="flex items-center gap-2">
          <Avatar initials={initials} className="h-8 w-8" />
          <div className="flex flex-col leading-tight">
            {displayName ? (
              <span className="text-sm font-medium text-zinc-800">{displayName}</span>
            ) : null}
            {email ? (
              <span className="text-xs text-zinc-500">{email}</span>
            ) : null}
          </div>
        </div>

        <button
          type="button"
          onClick={logout}
          className="inline-flex h-9 w-9 items-center justify-center rounded-lg text-zinc-500 transition-colors hover:bg-zinc-100 hover:text-zinc-800"
          title="Выйти"
        >
          <LogOut className="h-4 w-4" />
        </button>
      </div>
    </header>
  );
}
