"use client";

import { Avatar } from "@/components/shared";
import {
  OrganizationRole,
  ProjectRole,
  type Guid,
  type OrganizationMemberDto,
  type ProjectDto,
  type ProjectMemberDto,
} from "@/types/admin.types";

const ORG_ROLES = [
  { value: OrganizationRole.Admin, label: "Admin" },
  { value: OrganizationRole.Developer, label: "Developer" },
  { value: OrganizationRole.Analyst, label: "Analyst" },
  { value: OrganizationRole.Viewer, label: "Viewer" },
];

const PROJECT_ROLES = [
  { value: ProjectRole.Admin, label: "Admin" },
  { value: ProjectRole.Developer, label: "Developer" },
  { value: ProjectRole.Analyst, label: "Analyst" },
  { value: ProjectRole.Viewer, label: "Viewer" },
];

function getInitials(name: string) {
  const parts = name.trim().split(/\s+/).filter(Boolean);
  const first = parts[0]?.[0] ?? "";
  const second = parts.length > 1 ? parts[1]?.[0] ?? "" : (parts[0]?.[1] ?? "");
  return (first + second).toUpperCase() || "U";
}

export function TeamMemberDetail({
  member,
  projectsData,
  isLoadingProjects,
  onBack,
  onSetOrgRole,
  onRemoveMember,
  onAddToProject,
  onRemoveFromProject,
  onSetProjectRole,
}: {
  member: OrganizationMemberDto;
  projectsData: { project: ProjectDto; membership: ProjectMemberDto | null }[] | undefined;
  isLoadingProjects: boolean;
  onBack: () => void;
  onSetOrgRole: (role: OrganizationRole) => void;
  onRemoveMember: () => void;
  onAddToProject: (projectId: Guid, role: ProjectRole) => void;
  onRemoveFromProject: (projectId: Guid) => void;
  onSetProjectRole: (projectId: Guid, role: ProjectRole) => void;
}) {
  const isOwner = member.role === OrganizationRole.Owner;
  const memberProjects = projectsData?.filter((p) => p.membership !== null) ?? [];
  const availableProjects = projectsData?.filter((p) => p.membership === null) ?? [];

  return (
    <div className="flex flex-col overflow-hidden" style={{ maxHeight: 440 }}>
      {/* Back */}
      <button
        type="button"
        className="flex cursor-pointer items-center gap-2 border-b border-zinc-100 px-4 py-2.5 text-xs text-zinc-400 hover:text-zinc-700"
        onClick={onBack}
      >
        <span>&larr;</span> Участники
      </button>

      <div className="flex-1 overflow-y-auto">
        {/* Header */}
        <div className="flex items-center gap-3 border-b border-zinc-100 px-4 py-4">
          <Avatar initials={getInitials(member.displayName)} className="h-10 w-10 text-xs" />
          <div className="min-w-0 flex-1">
            <div className="truncate text-sm font-semibold text-zinc-900">{member.displayName}</div>
            <div className="truncate text-[11px] text-zinc-400">{member.email}</div>
          </div>
        </div>

        {/* Org role */}
        <div className="px-4 py-3">
          <div className="mb-2 text-[11px] font-medium uppercase tracking-wide text-zinc-400">
            Роль в организации
          </div>
          {isOwner ? (
            <span className="text-sm text-zinc-500">Owner</span>
          ) : (
            <select
              className="h-9 w-full rounded-lg border border-zinc-200 bg-white px-3 text-sm text-zinc-900 outline-none"
              value={member.role}
              onChange={(e) => onSetOrgRole(Number(e.target.value) as OrganizationRole)}
            >
              {ORG_ROLES.map((r) => (
                <option key={r.value} value={r.value}>{r.label}</option>
              ))}
            </select>
          )}
        </div>

        {/* Projects */}
        <div className="px-4 pb-3">
          <div className="mb-2 flex items-center justify-between">
            <span className="text-[11px] font-medium uppercase tracking-wide text-zinc-400">
              Проекты
            </span>
            {availableProjects.length > 0 && (
              <select
                className="cursor-pointer rounded border border-zinc-200 px-2 py-0.5 text-[11px] text-zinc-500 outline-none"
                value=""
                onChange={(e) => {
                  if (e.target.value) {
                    onAddToProject(e.target.value, ProjectRole.Viewer);
                    e.target.value = "";
                  }
                }}
              >
                <option value="">+ Добавить</option>
                {availableProjects.map((p) => (
                  <option key={p.project.id} value={p.project.id}>{p.project.name}</option>
                ))}
              </select>
            )}
          </div>

          {isLoadingProjects ? (
            <p className="py-3 text-center text-xs text-zinc-400">Загрузка...</p>
          ) : memberProjects.length === 0 ? (
            <p className="py-3 text-center text-xs text-zinc-400">Нет проектов</p>
          ) : (
            <div className="flex flex-col gap-1">
              {memberProjects.map(({ project, membership }) => (
                <div
                  key={project.id}
                  className="flex items-center justify-between rounded-lg border border-zinc-100 px-3 py-2"
                >
                  <span className="truncate text-sm text-zinc-700">{project.name}</span>
                  <div className="flex items-center gap-1.5">
                    <select
                      className="cursor-pointer rounded border border-zinc-100 px-1.5 py-0.5 text-[11px] text-zinc-500 outline-none"
                      value={membership!.role}
                      onChange={(e) =>
                        onSetProjectRole(project.id, Number(e.target.value) as ProjectRole)
                      }
                    >
                      {PROJECT_ROLES.map((r) => (
                        <option key={r.value} value={r.value}>{r.label}</option>
                      ))}
                    </select>
                    <button
                      type="button"
                      className="cursor-pointer rounded p-0.5 text-xs text-zinc-400 hover:text-red-500"
                      onClick={() => onRemoveFromProject(project.id)}
                    >
                      ✕
                    </button>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>

        {/* Kick */}
        {!isOwner && (
          <div className="border-t border-zinc-100 px-4 py-3">
            <button
              type="button"
              className="w-full cursor-pointer rounded-lg border border-red-200 py-2 text-sm text-red-500 hover:bg-red-50"
              onClick={onRemoveMember}
            >
              Исключить из организации
            </button>
          </div>
        )}
      </div>
    </div>
  );
}
