import { useQuery } from "@tanstack/react-query";
import type { EventDetailDto } from "@/types/analytics.types";
import AnalyticsService from "@/services/analytics/analyticsService";

const analyticsService = new AnalyticsService();

/**
 * Fetches event detail when a node is selected.
 */
export function useEventDetail(
  organizationId: string,
  projectId: string,
  processDefinitionId: string | null,
  eventName: string | null,
  from: string,
  to: string,
) {
  return useQuery<EventDetailDto>({
    queryKey: [
      "event-detail",
      organizationId,
      projectId,
      processDefinitionId,
      eventName,
      from,
      to,
    ],
    queryFn: () =>
      analyticsService.getEventDetail(
        organizationId,
        projectId,
        processDefinitionId!,
        eventName!,
        from,
        to,
      ),
    enabled: !!eventName && !!processDefinitionId,
  });
}
