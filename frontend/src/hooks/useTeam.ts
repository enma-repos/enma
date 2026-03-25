"use client";

import { useMemo } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import OrganizationMembersService from "@/services/admin/organizationMembersService";
import OrganizationInvitesService from "@/services/admin/organizationInvitesService";
import ProjectMembersService from "@/services/admin/projectMembersService";
import type {
  Guid,
  OrganizationRole,
  ProjectRole,
} from "@/types/admin.types";

export function useOrgMembers(orgId: Guid | null) {
  const service = useMemo(() => new OrganizationMembersService(), []);

  return useQuery({
    queryKey: ["org-members", orgId],
    queryFn: () => service.list(orgId!),
    enabled: Boolean(orgId),
  });
}

export function useOrgInvites(orgId: Guid | null) {
  const service = useMemo(() => new OrganizationInvitesService(), []);

  return useQuery({
    queryKey: ["org-invites", orgId],
    queryFn: () => service.listActive(orgId!),
    enabled: Boolean(orgId),
  });
}

export function useMemberProjects(orgId: Guid | null, userId: Guid | null) {
  const service = useMemo(() => new ProjectMembersService(), []);

  return useQuery({
    queryKey: ["member-projects", orgId, userId],
    queryFn: async () => {
      // Get all projects in org, then for each check if user is member
      // We need to list projects where this user is a member
      // Since there's no "list projects by user" in ProjectMembersService,
      // we'll use the projects list and filter by membership
      const { default: ProjectsService } = await import("@/services/admin/projectsService");
      const projectsService = new ProjectsService();
      const projects = await projectsService.listByOrg(orgId!);

      const results = await Promise.allSettled(
        projects.map((p) => service.get(orgId!, p.id, userId!)),
      );

      return projects
        .map((project, i) => {
          const result = results[i];
          if (result.status === "fulfilled") {
            return { project, membership: result.value };
          }
          return { project, membership: null };
        });
    },
    enabled: Boolean(orgId) && Boolean(userId),
  });
}

export function useTeamMutations(orgId: Guid | null) {
  const queryClient = useQueryClient();
  const membersService = useMemo(() => new OrganizationMembersService(), []);
  const invitesService = useMemo(() => new OrganizationInvitesService(), []);
  const projectMembersService = useMemo(() => new ProjectMembersService(), []);

  const invalidateAll = () => {
    queryClient.invalidateQueries({ queryKey: ["org-members", orgId] });
    queryClient.invalidateQueries({ queryKey: ["org-invites", orgId] });
    queryClient.invalidateQueries({ queryKey: ["member-projects"] });
  };

  const sendInvite = useMutation({
    mutationFn: (args: { email: string; role: OrganizationRole; createdByUserId: Guid }) =>
      invitesService.create(orgId!, {
        targetEmail: args.email,
        role: args.role,
        createdByUserId: args.createdByUserId,
      }),
    onSuccess: invalidateAll,
  });

  const setMemberRole = useMutation({
    mutationFn: (args: { userId: Guid; role: OrganizationRole }) =>
      membersService.setRole(orgId!, args.userId, { role: args.role }),
    onSuccess: invalidateAll,
  });

  const removeMember = useMutation({
    mutationFn: (userId: Guid) => membersService.remove(orgId!, userId),
    onSuccess: invalidateAll,
  });

  const addToProject = useMutation({
    mutationFn: (args: { projectId: Guid; userId: Guid; role: ProjectRole }) =>
      projectMembersService.add(orgId!, args.projectId, {
        userId: args.userId,
        role: args.role,
      }),
    onSuccess: invalidateAll,
  });

  const removeFromProject = useMutation({
    mutationFn: (args: { projectId: Guid; userId: Guid }) =>
      projectMembersService.remove(orgId!, args.projectId, args.userId),
    onSuccess: invalidateAll,
  });

  const setProjectRole = useMutation({
    mutationFn: (args: { projectId: Guid; userId: Guid; role: ProjectRole }) =>
      projectMembersService.setRole(orgId!, args.projectId, args.userId, { role: args.role }),
    onSuccess: invalidateAll,
  });

  return { sendInvite, setMemberRole, removeMember, addToProject, removeFromProject, setProjectRole };
}
