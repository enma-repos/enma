import { useQuery } from "@tanstack/react-query";
import type { SummaryDto } from "@/types/analytics.types";
import AnalyticsService from "@/services/analytics/analyticsService";

const analyticsService = new AnalyticsService();

export function useSummary(
  organizationId: string | undefined,
  projectId: string | undefined,
  processDefinitionIds: string[],
  from: string,
  to: string,
) {
  return useQuery<SummaryDto>({
    queryKey: ["summary", organizationId, projectId, processDefinitionIds, from, to],
    queryFn: () =>
      analyticsService.getSummary(
        organizationId!,
        projectId!,
        processDefinitionIds.length > 0 ? processDefinitionIds : null,
        from,
        to,
      ),
    enabled: !!organizationId && !!projectId,
  });
}
