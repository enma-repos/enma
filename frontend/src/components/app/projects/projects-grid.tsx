import { ProjectCard } from "@/components/app/projects/project-card";
import type { ProjectDto } from "@/types/admin.types";

export type ProjectsGridProps = {
  organizationSlug: string;
  projects: ProjectDto[];
};

export function ProjectsGrid({ organizationSlug, projects }: ProjectsGridProps) {
  return (
    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
      {projects.map((project) => (
        <ProjectCard
          key={project.id}
          organizationSlug={organizationSlug}
          project={project}
        />
      ))}
    </div>
  );
}

