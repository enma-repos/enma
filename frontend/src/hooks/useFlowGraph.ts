import { useQuery } from "@tanstack/react-query";
import type { FlowGraphDto } from "@/types/analytics.types";
import AnalyticsService from "@/services/analytics/analyticsService";

const analyticsService = new AnalyticsService();

/**
 * Fetches the flow graph for a given process definition.
 */
export function useFlowGraph(
  organizationId: string,
  projectId: string,
  processDefinitionId: string | null,
  from: string,
  to: string,
) {
  return useQuery<FlowGraphDto>({
    queryKey: [
      "flow-graph",
      organizationId,
      projectId,
      processDefinitionId,
      from,
      to,
    ],
    queryFn: () =>
      analyticsService.getFlowGraph(
        organizationId,
        projectId,
        processDefinitionId!,
        from,
        to,
      ),
    enabled: !!processDefinitionId,
  });
}
