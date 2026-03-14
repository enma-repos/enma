import Link from "next/link";
import { Badge, Card, IconGrid, cn } from "@/components/shared";
import type { ProjectDto } from "@/types/admin.types";

function formatDate(value: string) {
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return value;
  return new Intl.DateTimeFormat("ru-RU", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  }).format(date);
}

export type ProjectCardProps = {
  organizationSlug: string;
  project: ProjectDto;
};

export function ProjectCard({ organizationSlug, project }: ProjectCardProps) {
  const isArchived = Boolean(project.archivedAt);
  const href = `/app/organizations/${encodeURIComponent(organizationSlug)}/projects/${encodeURIComponent(project.key)}`;

  return (
    <Card className="transition-shadow hover:shadow-md">
      <Link
        href={href}
        className={cn(
          "block rounded-2xl p-5 focus:outline-none focus:ring-2 focus:ring-zinc-900/10",
        )}
        aria-label={`Open project ${project.name}`}
      >
        <div className="flex items-start justify-between gap-4">
          <div className="grid h-12 w-12 place-items-center rounded-xl bg-blue-50 text-blue-600">
            <IconGrid className="h-6 w-6" aria-hidden="true" />
          </div>
          <div className="flex items-center gap-2">
            {isArchived ? <Badge>архив</Badge> : null}
            <div className="text-xs text-zinc-400">
              <span className="font-mono">{project.key}</span>
            </div>
          </div>
        </div>

        <div className="mt-4">
          <div className="truncate text-base font-semibold text-zinc-900">
            {project.name}
          </div>
          <div className="mt-1 line-clamp-2 text-sm text-zinc-500">
            {project.description || "Без описания"}
          </div>
          <div className="mt-3 text-xs text-zinc-400">
            {formatDate(project.createdAt)}
          </div>
        </div>
      </Link>
    </Card>
  );
}

