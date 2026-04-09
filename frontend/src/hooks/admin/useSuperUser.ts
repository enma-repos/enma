"use client";

import { useMemo } from "react";
import { useQuery } from "@tanstack/react-query";
import SuperUsersService from "@/services/admin/super/superUsersService";
import type { Guid } from "@/types/admin.types";

export function useSuperUser(userId: Guid | undefined) {
  const service = useMemo(() => new SuperUsersService(), []);
  return useQuery({
    queryKey: ["super", "users", userId] as const,
    queryFn: () => {
      if (!userId) throw new Error("Missing userId");
      return service.getById(userId);
    },
    enabled: Boolean(userId),
  });
}
