import { useQuery } from "@tanstack/react-query";
import type { TopEventsDto } from "@/types/analytics.types";
import AnalyticsService from "@/services/analytics/analyticsService";

const analyticsService = new AnalyticsService();

export function useTopEvents(
  organizationId: string | undefined,
  projectId: string | undefined,
  processDefinitionIds: string[],
  from: string,
  to: string,
  limit = 20,
) {
  return useQuery<TopEventsDto>({
    queryKey: ["top-events-project", organizationId, projectId, processDefinitionIds, from, to, limit],
    queryFn: () =>
      analyticsService.getProjectTopEvents(
        organizationId!,
        projectId!,
        processDefinitionIds.length > 0 ? processDefinitionIds : null,
        from,
        to,
        "visits",
        limit,
      ),
    enabled: !!organizationId && !!projectId,
  });
}
