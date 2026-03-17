"use client";

import { useParams, usePathname, useRouter } from "next/navigation";
import { useMemo, useState } from "react";
import { useMe } from "@/hooks/useMe";
import { useLogout } from "@/hooks/useLogout";
import { useOrganizations } from "@/hooks/useOrganizations";
import { useProjects } from "@/hooks/useProjects";
import { AppTopbar } from "@/components/app/app-topbar";
import { CreateOrganizationDialog } from "@/components/app/organizations/create-organization-dialog";
import { CreateProjectDialog } from "@/components/app/projects/create-project-dialog";
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

  const {
    organizations,
    createOrganization,
    isCreating: isCreatingOrg,
    createError: createOrgError,
    setOrganizationName,
    softDeleteOrganization,
  } = useOrganizations();

  const currentOrgId = params?.organization_id ?? null;
  const currentProjectId = params?.project_id ?? null;

  const currentOrganization = useMemo(
    () => organizations.find((o) => o.id === currentOrgId) ?? null,
    [organizations, currentOrgId],
  );

  const {
    projects,
    createProject,
    isCreating: isCreatingProject,
    createError: createProjectError,
    setProjectName,
    softDeleteProject,
  } = useProjects(currentOrgId ?? "");

  const currentProject = useMemo(
    () => projects.find((p) => p.id === currentProjectId) ?? null,
    [projects, currentProjectId],
  );

  const projectsService = useMemo(() => new ProjectsService(), []);

  const [createOrgDialogOpen, setCreateOrgDialogOpen] = useState(false);
  const [createProjectDialogOpen, setCreateProjectDialogOpen] = useState(false);

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
    setCreateOrgDialogOpen(true);
  };

  const handleCreateProject = () => {
    setCreateProjectDialogOpen(true);
  };

  const handleRenameOrganization = async (org: OrganizationDto, newName: string) => {
    try {
      await setOrganizationName({ organizationId: org.id, name: newName });
    } catch {
      // silently fail — query will refetch
    }
  };

  const handleDeleteOrganization = async (org: OrganizationDto) => {
    try {
      await softDeleteOrganization(org.id);
      // If we deleted the current org, navigate to another one
      if (org.id === currentOrgId) {
        const remaining = organizations.filter((o) => o.id !== org.id);
        if (remaining[0]) {
          const orgProjects = await projectsService.listByOrg(remaining[0].id);
          const firstProject = orgProjects[0];
          if (firstProject) {
            router.push(
              `/app/organizations/${remaining[0].id}/projects/${firstProject.id}/analytics/summary`,
            );
          } else {
            router.push(`/app/organizations/${remaining[0].id}/projects`);
          }
        } else {
          router.push("/app");
        }
      }
    } catch {
      // silently fail
    }
  };

  const handleRenameProject = async (project: ProjectDto, newName: string) => {
    try {
      await setProjectName({ projectId: project.id, name: newName });
    } catch {
      // silently fail
    }
  };

  const handleDeleteProject = async (project: ProjectDto) => {
    try {
      await softDeleteProject(project.id);
      // If we deleted the current project, navigate to another one
      if (project.id === currentProjectId && currentOrgId) {
        const remaining = projects.filter((p) => p.id !== project.id);
        if (remaining[0]) {
          router.push(
            `/app/organizations/${currentOrgId}/projects/${remaining[0].id}${routeTail}`,
          );
        }
      }
    } catch {
      // silently fail
    }
  };

  const handleOrgCreated = async (dto: Parameters<typeof createOrganization>[0]) => {
    const org = await createOrganization(dto);
    // Backend auto-creates a "default" project, fetch it and navigate
    try {
      const orgProjects = await projectsService.listByOrg(org.id);
      const firstProject = orgProjects[0];
      if (firstProject) {
        router.push(
          `/app/organizations/${org.id}/projects/${firstProject.id}/analytics/summary`,
        );
        return;
      }
    } catch {
      // fallback
    }
    router.push(`/app/organizations/${org.id}/projects`);
  };

  const handleProjectCreated = async (dto: Parameters<typeof createProject>[0]) => {
    const project = await createProject(dto);
    if (currentOrgId) {
      router.push(
        `/app/organizations/${currentOrgId}/projects/${project.id}/analytics/summary`,
      );
    }
  };

  return (
    <>
      <AppTopbar
        displayName={displayName}
        onLogout={logout}
        organizations={organizations}
        currentOrganization={currentOrganization}
        onSelectOrganization={handleSelectOrganization}
        onCreateOrganization={handleCreateOrganization}
        onRenameOrganization={handleRenameOrganization}
        onDeleteOrganization={handleDeleteOrganization}
        projects={projects}
        currentProject={currentProject}
        onSelectProject={handleSelectProject}
        onCreateProject={handleCreateProject}
        onRenameProject={handleRenameProject}
        onDeleteProject={handleDeleteProject}
      />

      <CreateOrganizationDialog
        open={createOrgDialogOpen}
        onClose={() => setCreateOrgDialogOpen(false)}
        onCreate={handleOrgCreated}
        isCreating={isCreatingOrg}
        error={createOrgError}
      />

      <CreateProjectDialog
        open={createProjectDialogOpen}
        onClose={() => setCreateProjectDialogOpen(false)}
        organizationSlug={currentOrganization?.slug ?? ""}
        onCreate={handleProjectCreated}
        isCreating={isCreatingProject}
        error={createProjectError}
      />
    </>
  );
}
