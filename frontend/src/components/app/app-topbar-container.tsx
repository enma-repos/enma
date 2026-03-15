"use client";

import { useParams, usePathname, useRouter } from "next/navigation";
import { useMemo } from "react";
import { useMe } from "@/hooks/useMe";
import { useLogout } from "@/hooks/useLogout";
import { useOrganizations } from "@/hooks/useOrganizations";
import { useProjects } from "@/hooks/useProjects";
import { AppTopbar } from "@/components/app/app-topbar";
import type { OrganizationDto, ProjectDto } from "@/types/admin.types";
import ProjectsService from "@/services/admin/projectsService";

export function AppTopbarContainer() {
  const router = useRouter();
  const pathname = usePathname();
  const params = useParams<{
    organization_id?: string;
    project_id?: string;
  }>();

  const meQuery = useMe();
  const displayName = meQuery.data?.user?.displayName ?? null;
  const logout = useLogout();

  const { organizations } = useOrganizations();

  const currentOrgId = params?.organization_id ?? null;
  const currentProjectId = params?.project_id ?? null;

  const currentOrganization = useMemo(
    () => organizations.find((o) => o.id === currentOrgId) ?? null,
    [organizations, currentOrgId],
  );

  const { projects } = useProjects(currentOrgId ?? "");

  const currentProject = useMemo(
    () => projects.find((p) => p.id === currentProjectId) ?? null,
    [projects, currentProjectId],
  );

  const projectsService = useMemo(() => new ProjectsService(), []);

  // Extract the path tail after /projects/{project_id}/...
  const routeTail = useMemo(() => {
    if (!currentOrgId || !currentProjectId) return "/analytics/summary";
    const prefix = `/app/organizations/${currentOrgId}/projects/${currentProjectId}`;
    const tail = pathname.startsWith(prefix)
      ? pathname.slice(prefix.length)
      : "";
    return tail || "/analytics/summary";
  }, [pathname, currentOrgId, currentProjectId]);

  const handleSelectOrganization = async (org: OrganizationDto) => {
    if (org.id === currentOrgId) return;
    try {
      const orgProjects = await projectsService.listByOrg(org.id);
      const firstProject = orgProjects[0];
      if (firstProject) {
        router.push(
          `/app/organizations/${org.id}/projects/${firstProject.id}${routeTail}`,
        );
      } else {
        router.push(`/app/organizations/${org.id}/projects`);
      }
    } catch {
      router.push(`/app/organizations/${org.id}/projects`);
    }
  };

  const handleSelectProject = (project: ProjectDto) => {
    if (project.id === currentProjectId) return;
    router.push(
      `/app/organizations/${currentOrgId}/projects/${project.id}${routeTail}`,
    );
  };

  const handleCreateOrganization = () => {
    // Navigate to org creation page or open a modal in the future
    router.push("/app/organizations");
  };

  const handleCreateProject = () => {
    if (currentOrgId) {
      router.push(`/app/organizations/${currentOrgId}/projects`);
    }
  };

  return (
    <AppTopbar
      displayName={displayName}
      onLogout={logout}
      organizations={organizations}
      currentOrganization={currentOrganization}
      onSelectOrganization={handleSelectOrganization}
      onCreateOrganization={handleCreateOrganization}
      projects={projects}
      currentProject={currentProject}
      onSelectProject={handleSelectProject}
      onCreateProject={handleCreateProject}
    />
  );
}
