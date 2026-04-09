"use client";

import { useMemo } from "react";
import { useQuery } from "@tanstack/react-query";
import SuperStatsService from "@/services/admin/super/superStatsService";

export function useSuperStats() {
  const service = useMemo(() => new SuperStatsService(), []);
  return useQuery({
    queryKey: ["super", "stats", "overview"] as const,
    queryFn: () => service.getOverview(),
  });
}
