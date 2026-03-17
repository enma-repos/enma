"use client";

import { useEffect } from "react";
import { useParams, useRouter } from "next/navigation";
import { useProjects } from "@/hooks/useProjects";

export default function ProjectsPage() {
  const router = useRouter();
  const params = useParams<{ organization_id: string }>();
  const orgId = params?.organization_id ?? "";
  const { projects, isLoading } = useProjects(orgId);

  useEffect(() => {
    if (isLoading) return;

    const firstProject = projects[0];
    if (firstProject) {
      router.replace(
        `/app/organizations/${orgId}/projects/${firstProject.id}/analytics/summary`,
      );
    }
  }, [isLoading, projects, orgId, router]);

  return (
    <div className="flex min-h-screen items-center justify-center">
      <div className="text-sm text-zinc-400">Загрузка...</div>
    </div>
  );
}
