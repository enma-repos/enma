"use client";

import { useMemo, useState } from "react";
import { useProjects } from "@/hooks/useProjects";
import { ProjectsEmpty } from "@/components/app/projects/projects-empty";
import { ProjectsGrid } from "@/components/app/projects/projects-grid";
import { ProjectsSkeleton } from "@/components/app/projects/projects-skeleton";
import { ProjectsToolbar } from "@/components/app/projects/projects-toolbar";
import { CreateProjectDialog } from "@/components/app/projects/create-project-dialog";

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось загрузить проекты.";
}

export type ProjectsScreenProps = {
  organizationSlug: string;
};

export function ProjectsScreen({ organizationSlug }: ProjectsScreenProps) {
  const [isCreateOpen, setIsCreateOpen] = useState(false);
  const {
    organization,
    isLoading,
    error,
    query,
    setQuery,
    filteredProjects,
    createProject,
    isCreating,
    createError,
  } = useProjects(organizationSlug);

  const title = useMemo(() => {
    const orgLabel = organization?.name ?? organizationSlug;
    return `Проекты · ${orgLabel}`;
  }, [organization?.name, organizationSlug]);

  return (
    <div className="mx-auto w-full max-w-6xl mt-6">
      <ProjectsToolbar
        title={title}
        organizationSlug={organizationSlug}
        query={query}
        onQueryChange={setQuery}
        onCreate={() => setIsCreateOpen(true)}
      />

      <div className="mt-6">
        {isLoading ? <ProjectsSkeleton /> : null}

        {!isLoading && error ? (
          <div className="rounded-2xl border border-red-200 bg-red-50 p-5 text-sm text-red-700">
            {getErrorMessage(error)}
          </div>
        ) : null}

        {!isLoading && !error && filteredProjects.length === 0 ? (
          <ProjectsEmpty />
        ) : null}

        {!isLoading && !error && filteredProjects.length > 0 ? (
          <ProjectsGrid organizationSlug={organizationSlug} projects={filteredProjects} />
        ) : null}
      </div>

      <CreateProjectDialog
        open={isCreateOpen}
        onClose={() => setIsCreateOpen(false)}
        organizationSlug={organizationSlug}
        onCreate={createProject}
        isCreating={isCreating}
        error={createError}
      />
    </div>
  );
}

