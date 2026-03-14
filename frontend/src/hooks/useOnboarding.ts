"use client";

import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useRouter } from "next/navigation";
import AuthService from "@/services/auth/authService";
import OrganizationsService from "@/services/admin/organizationsService";

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

      const { user } = await authService.completeOnboarding({
        displayName: params.displayName,
        avatarUrl: params.avatarUrl,
        locale: params.locale,
        timezone: params.timezone,
      });

      const org = await orgsService.create({
        name: params.organizationName,
        slug: params.organizationSlug,
        createdByUserId: user.accountId,
      });

      return org;
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ["me"] });
      router.push("/app/organizations");
    },
  });

  return {
    complete: mutation.mutateAsync,
    isPending: mutation.isPending,
    error: mutation.error,
  };
}
