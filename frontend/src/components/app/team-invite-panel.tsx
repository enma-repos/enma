"use client";

import { useState } from "react";
import { Input, Button } from "@/components/shared";
import { OrganizationRole, type Guid, type ProjectDto } from "@/types/admin.types";

const INVITE_ROLES = [
  { value: OrganizationRole.Admin, label: "Admin" },
  { value: OrganizationRole.Developer, label: "Developer" },
  { value: OrganizationRole.Analyst, label: "Analyst" },
  { value: OrganizationRole.Viewer, label: "Viewer" },
];

export function TeamInvitePanel({
  projects,
  onSendInvite,
  isSending,
}: {
  projects: ProjectDto[];
  onSendInvite: (email: string, role: OrganizationRole, projectIds: Guid[]) => void;
  isSending: boolean;
}) {
  const [email, setEmail] = useState("");
  const [role, setRole] = useState<OrganizationRole>(OrganizationRole.Developer);
  const [selectedProjects, setSelectedProjects] = useState<Guid[]>([]);

  const toggleProject = (projectId: Guid) => {
    setSelectedProjects((prev) =>
      prev.includes(projectId) ? prev.filter((id) => id !== projectId) : [...prev, projectId],
    );
  };

  const handleSubmit = () => {
    if (!email.trim()) return;
    onSendInvite(email.trim(), role, selectedProjects);
    setEmail("");
    setSelectedProjects([]);
  };

  return (
    <div className="flex flex-col gap-3 p-4">
      <div>
        <label className="mb-1 block text-[11px] font-medium uppercase tracking-wide text-zinc-500">
          Email
        </label>
        <Input
          type="email"
          placeholder="user@example.com"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          onKeyDown={(e) => e.key === "Enter" && handleSubmit()}
        />
      </div>

      <div>
        <label className="mb-1 block text-[11px] font-medium uppercase tracking-wide text-zinc-500">
          Роль в организации
        </label>
        <select
          className="h-10 w-full rounded-lg border border-zinc-200 bg-white px-3 text-sm text-zinc-900 outline-none focus:ring-2 focus:ring-zinc-900/10"
          value={role}
          onChange={(e) => setRole(Number(e.target.value) as OrganizationRole)}
        >
          {INVITE_ROLES.map((r) => (
            <option key={r.value} value={r.value}>
              {r.label}
            </option>
          ))}
        </select>
      </div>

      <div>
        <label className="mb-1 block text-[11px] font-medium uppercase tracking-wide text-zinc-500">
          Проекты
        </label>
        <div className="flex flex-wrap gap-1.5">
          {projects.map((p) => {
            const selected = selectedProjects.includes(p.id);
            return (
              <button
                key={p.id}
                type="button"
                className={`cursor-pointer rounded-md border px-2.5 py-1 text-xs transition-colors ${
                  selected
                    ? "border-zinc-900 bg-zinc-900 text-white"
                    : "border-zinc-200 text-zinc-600 hover:border-zinc-400"
                }`}
                onClick={() => toggleProject(p.id)}
              >
                {p.name}
              </button>
            );
          })}
          {projects.length === 0 && (
            <span className="text-xs text-zinc-400">Нет проектов</span>
          )}
        </div>
      </div>

      <Button
        className="mt-1 w-full"
        onClick={handleSubmit}
        disabled={!email.trim() || isSending}
      >
        {isSending ? "Отправка..." : "Отправить приглашение"}
      </Button>
    </div>
  );
}
