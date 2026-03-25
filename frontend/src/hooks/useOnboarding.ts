"use client";

import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useRouter } from "next/navigation";
import AuthService from "@/services/auth/authService";
import OrganizationsService from "@/services/admin/organizationsService";
import ProjectsService from "@/services/admin/projectsService";

export interface OnboardingParams {
  displayName: string;
  avatarUrl: string | null;
  locale: string | null;
  timezone: string | null;
  organizationName: string;
  organizationSlug: string;
}

export function useOnboarding() {
  const queryClient = useQueryClient();
  const router = useRouter();

  const mutation = useMutation({
    mutationFn: async (params: OnboardingParams) => {
      const authService = new AuthService();
      const orgsService = new OrganizationsService();
      const projectsService = new ProjectsService();

      const { user } = await authService.completeOnboarding({
        displayName: params.displayName,
        avatarUrl: params.avatarUrl,
        locale: params.locale,
        timezone: params.timezone,
      });

      const org = await orgsService.create({
        name: params.organizationName,
        slug: params.organizationSlug,
      });

      // Backend auto-creates a "default" project; fetch it for navigation
      let firstProjectId: string | null = null;
      try {
        const projects = await projectsService.listByOrg(org.id);
        firstProjectId = projects[0]?.id ?? null;
      } catch {
        // fallback handled below
      }

      return { org, firstProjectId };
    },
    onSuccess: async (data) => {
      await queryClient.invalidateQueries({ queryKey: ["me"] });
      if (data.firstProjectId) {
        router.push(
          `/app/organizations/${data.org.id}/projects/${data.firstProjectId}/analytics/summary`,
        );
      } else {
        router.push(`/app/organizations/${data.org.id}/projects`);
      }
    },
  });

  return {
    complete: mutation.mutateAsync,
    isPending: mutation.isPending,
    error: mutation.error,
  };
}
