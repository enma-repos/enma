"use client";

import { useMemo } from "react";
import { useQuery } from "@tanstack/react-query";
import SuperProjectsService from "@/services/admin/super/superProjectsService";
import type { Guid } from "@/types/admin.types";

export function useSuperProject(projectId: Guid | undefined) {
  const service = useMemo(() => new SuperProjectsService(), []);
  return useQuery({
    queryKey: ["super", "projects", projectId] as const,
    queryFn: () => {
      if (!projectId) throw new Error("Missing projectId");
      return service.getById(projectId);
    },
    enabled: Boolean(projectId),
  });
}
