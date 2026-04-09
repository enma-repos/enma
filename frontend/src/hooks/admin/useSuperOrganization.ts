"use client";

import { useMemo } from "react";
import { useQuery } from "@tanstack/react-query";
import SuperOrganizationsService from "@/services/admin/super/superOrganizationsService";
import type { Guid } from "@/types/admin.types";

export function useSuperOrganization(organizationId: Guid | undefined) {
  const service = useMemo(() => new SuperOrganizationsService(), []);
  return useQuery({
    queryKey: ["super", "organizations", organizationId] as const,
    queryFn: () => {
      if (!organizationId) throw new Error("Missing organizationId");
      return service.getById(organizationId);
    },
    enabled: Boolean(organizationId),
  });
}
