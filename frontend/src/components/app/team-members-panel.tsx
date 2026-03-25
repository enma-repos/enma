"use client";

import { useState } from "react";
import { Avatar, Input } from "@/components/shared";
import { OrganizationRole, type OrganizationMemberDto } from "@/types/admin.types";

const ROLE_LABELS: Record<OrganizationRole, string> = {
  [OrganizationRole.Owner]: "Owner",
  [OrganizationRole.Admin]: "Admin",
  [OrganizationRole.Analyst]: "Analyst",
  [OrganizationRole.Developer]: "Developer",
  [OrganizationRole.Viewer]: "Viewer",
};

function getInitials(name: string) {
  const parts = name.trim().split(/\s+/).filter(Boolean);
  const first = parts[0]?.[0] ?? "";
  const second = parts.length > 1 ? parts[1]?.[0] ?? "" : (parts[0]?.[1] ?? "");
  return (first + second).toUpperCase() || "U";
}

export function TeamMembersPanel({
  members,
  isLoading,
  onSelectMember,
}: {
  members: OrganizationMemberDto[];
  isLoading: boolean;
  onSelectMember: (userId: string) => void;
}) {
  const [search, setSearch] = useState("");

  const filtered = members.filter((m) => {
    if (!search.trim()) return true;
    const q = search.trim().toLowerCase();
    return m.displayName.toLowerCase().includes(q) || m.email.toLowerCase().includes(q);
  });

  return (
    <div className="flex flex-col overflow-hidden" style={{ maxHeight: 400 }}>
      <div className="border-b border-zinc-100 px-3 py-2">
        <Input
          placeholder="Поиск..."
          className="h-8 text-xs"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
      </div>
      <div className="flex-1 overflow-y-auto">
        {isLoading ? (
          <p className="px-4 py-6 text-center text-xs text-zinc-400">Загрузка...</p>
        ) : filtered.length === 0 ? (
          <p className="px-4 py-6 text-center text-xs text-zinc-400">Нет участников</p>
        ) : (
          filtered.map((m) => (
            <button
              key={m.userId}
              type="button"
              className="flex w-full cursor-pointer items-center gap-3 px-4 py-2.5 text-left transition-colors hover:bg-zinc-50"
              onClick={() => onSelectMember(m.userId)}
            >
              <Avatar initials={getInitials(m.displayName)} className="h-8 w-8 text-[10px]" />
              <div className="min-w-0 flex-1">
                <div className="truncate text-sm font-medium text-zinc-900">{m.displayName}</div>
                <div className="truncate text-[11px] text-zinc-400">{m.email}</div>
              </div>
              <span className="rounded bg-zinc-100 px-2 py-0.5 text-[11px] text-zinc-500">
                {ROLE_LABELS[m.role]}
              </span>
            </button>
          ))
        )}
      </div>
    </div>
  );
}
