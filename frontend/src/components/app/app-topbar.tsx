"use client";

import { useCallback, useEffect, useRef, useState } from "react";
import {
  Avatar,
  Button,
  IconBell,
  IconChevronDown,
  IconPlus,
} from "@/components/shared";
import type { OrganizationDto, ProjectDto } from "@/types/admin.types";
import { Building2, FolderKanban } from "lucide-react";

export type AppTopbarProps = {
  displayName: string | null;
  onLogout: () => void;

  organizations: OrganizationDto[];
  currentOrganization: OrganizationDto | null;
  onSelectOrganization: (org: OrganizationDto) => void;
  onCreateOrganization: () => void;

  projects: ProjectDto[];
  currentProject: ProjectDto | null;
  onSelectProject: (project: ProjectDto) => void;
  onCreateProject: () => void;
};

function getInitials(displayName: string | null) {
  if (!displayName) return "U";
  const parts = displayName.trim().split(/\s+/).filter(Boolean);
  const first = parts[0]?.[0] ?? "";
  const second =
    parts.length > 1 ? parts[1]?.[0] ?? "" : (parts[0]?.[1] ?? "");
  const initials = (first + second).toUpperCase();
  return initials || "U";
}

function useDropdown() {
  const [open, setOpen] = useState(false);
  const ref = useRef<HTMLDivElement>(null);

  const toggle = useCallback(() => setOpen((p) => !p), []);
  const close = useCallback(() => setOpen(false), []);

  useEffect(() => {
    if (!open) return;
    function handleClick(e: MouseEvent) {
      if (ref.current && !ref.current.contains(e.target as Node)) {
        setOpen(false);
      }
    }
    document.addEventListener("mousedown", handleClick);
    return () => document.removeEventListener("mousedown", handleClick);
  }, [open]);

  return { open, toggle, close, ref };
}

export function AppTopbar({
  displayName,
  onLogout,
  organizations,
  currentOrganization,
  onSelectOrganization,
  onCreateOrganization,
  projects,
  currentProject,
  onSelectProject,
  onCreateProject,
}: AppTopbarProps) {
  const initials = getInitials(displayName);

  const orgDropdown = useDropdown();
  const projectDropdown = useDropdown();
  const userDropdown = useDropdown();

  return (
    <header className="sticky top-0 z-30 border-b border-zinc-200 bg-white">
      <div className="flex h-16 items-center gap-4 px-6">
        {/* Organization selector */}
        <div className="relative" ref={orgDropdown.ref}>
          <button
            type="button"
            className="flex cursor-pointer items-center gap-2 rounded-lg px-3 py-2 text-sm font-medium text-zinc-700 hover:bg-zinc-100 transition-colors"
            onClick={orgDropdown.toggle}
          >
            <Building2 size={16} className="text-zinc-400" />
            <span className="max-w-[180px] truncate">
              {currentOrganization?.name ?? "Организация"}
            </span>
            <IconChevronDown className="h-3.5 w-3.5 text-zinc-400" />
          </button>

          {orgDropdown.open && (
            <div className="absolute left-0 top-full mt-1 min-w-[220px] rounded-xl border border-zinc-200 bg-white py-1 shadow-lg">
              {organizations.map((org) => (
                <button
                  key={org.id}
                  type="button"
                  className={`w-full cursor-pointer px-4 py-2 text-left text-sm hover:bg-zinc-100 ${
                    org.id === currentOrganization?.id
                      ? "font-medium text-zinc-900 bg-zinc-50"
                      : "text-zinc-700"
                  }`}
                  onClick={() => {
                    orgDropdown.close();
                    onSelectOrganization(org);
                  }}
                >
                  {org.name}
                </button>
              ))}
              <div className="border-t border-zinc-100 mt-1 pt-1">
                <button
                  type="button"
                  className="flex w-full cursor-pointer items-center gap-2 px-4 py-2 text-left text-sm text-zinc-500 hover:bg-zinc-100"
                  onClick={() => {
                    orgDropdown.close();
                    onCreateOrganization();
                  }}
                >
                  <IconPlus className="h-3.5 w-3.5" />
                  Создать организацию
                </button>
              </div>
            </div>
          )}
        </div>

        <span className="text-zinc-300">/</span>

        {/* Project selector */}
        <div className="relative" ref={projectDropdown.ref}>
          <button
            type="button"
            className="flex cursor-pointer items-center gap-2 rounded-lg px-3 py-2 text-sm font-medium text-zinc-700 hover:bg-zinc-100 transition-colors"
            onClick={projectDropdown.toggle}
          >
            <FolderKanban size={16} className="text-zinc-400" />
            <span className="max-w-[180px] truncate">
              {currentProject?.name ?? "Проект"}
            </span>
            <IconChevronDown className="h-3.5 w-3.5 text-zinc-400" />
          </button>

          {projectDropdown.open && (
            <div className="absolute left-0 top-full mt-1 min-w-[220px] rounded-xl border border-zinc-200 bg-white py-1 shadow-lg">
              {projects.map((project) => (
                <button
                  key={project.id}
                  type="button"
                  className={`w-full cursor-pointer px-4 py-2 text-left text-sm hover:bg-zinc-100 ${
                    project.id === currentProject?.id
                      ? "font-medium text-zinc-900 bg-zinc-50"
                      : "text-zinc-700"
                  }`}
                  onClick={() => {
                    projectDropdown.close();
                    onSelectProject(project);
                  }}
                >
                  {project.name}
                </button>
              ))}
              <div className="border-t border-zinc-100 mt-1 pt-1">
                <button
                  type="button"
                  className="flex w-full cursor-pointer items-center gap-2 px-4 py-2 text-left text-sm text-zinc-500 hover:bg-zinc-100"
                  onClick={() => {
                    projectDropdown.close();
                    onCreateProject();
                  }}
                >
                  <IconPlus className="h-3.5 w-3.5" />
                  Создать проект
                </button>
              </div>
            </div>
          )}
        </div>

        {/* Right side: user menu, notifications, avatar */}
        <div className="ml-auto flex items-center gap-1">
          {displayName ? (
            <div className="relative" ref={userDropdown.ref}>
              <Button
                variant="ghost"
                size="sm"
                className="max-w-[220px] cursor-pointer rounded-xl"
                onClick={userDropdown.toggle}
              >
                <span className="truncate text-sm font-medium text-zinc-700">
                  {displayName}
                </span>
                <IconChevronDown
                  className="h-4 w-4 text-zinc-500"
                  aria-hidden="true"
                />
              </Button>

              {userDropdown.open && (
                <div className="absolute right-0 top-full mt-1 min-w-[160px] rounded-xl border border-zinc-200 bg-white py-1 shadow-lg">
                  <button
                    type="button"
                    className="w-full cursor-pointer px-4 py-2 text-left text-sm text-zinc-700 hover:bg-zinc-100"
                    onClick={() => {
                      userDropdown.close();
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
