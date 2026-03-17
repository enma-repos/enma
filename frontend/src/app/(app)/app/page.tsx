"use client";

import { useEffect } from "react";
import { useRouter } from "next/navigation";
import { useOrganizations } from "@/hooks/useOrganizations";
import ProjectsService from "@/services/admin/projectsService";

export default function AppRootPage() {
  const router = useRouter();
  const { organizations, isLoading } = useOrganizations();

  useEffect(() => {
    if (isLoading) return;

    async function redirect() {
      const firstOrg = organizations[0];
      if (!firstOrg) return;

      try {
        const projectsService = new ProjectsService();
        const projects = await projectsService.listByOrg(firstOrg.id);
        const firstProject = projects[0];
        if (firstProject) {
          router.replace(
            `/app/organizations/${firstOrg.id}/projects/${firstProject.id}/analytics/summary`,
          );
          return;
        }
      } catch {
        // fallback
      }
      router.replace(`/app/organizations/${firstOrg.id}/projects`);
    }

    redirect();
  }, [isLoading, organizations, router]);

  return (
    <div className="flex min-h-screen items-center justify-center">
      <div className="text-sm text-zinc-400">Загрузка...</div>
    </div>
  );
}
