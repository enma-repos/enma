import { useQuery } from "@tanstack/react-query";
import type { FlowGraphDto } from "@/types/analytics.types";
import AnalyticsService from "@/services/analytics/analyticsService";
import type { Guid } from "@/types/admin.types";

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
  entryEvent?: string | null,
) {
  return useQuery<FlowGraphDto>({
    queryKey: [
      "flow-graph",
      organizationId,
      projectId,
      processDefinitionId,
      from,
      to,
      entryEvent ?? null,
    ],
    queryFn: () =>
      analyticsService.getFlowGraph(
        organizationId,
        projectId,
        processDefinitionId!,
        from,
        to,
        entryEvent ?? undefined,
      ),
    enabled: !!processDefinitionId,
  });
}

/**
 * Fetches the flow graph for multiple process definitions at once.
 */
export function useMultiProcessFlowGraph(
  organizationId: string,
  projectId: string,
  processDefinitionIds: Guid[],
  from: string,
  to: string,
) {
  return useQuery<FlowGraphDto>({
    queryKey: [
      "flow-graph-multi",
      organizationId,
      projectId,
      processDefinitionIds,
      from,
      to,
    ],
    queryFn: () =>
      analyticsService.getMultiProcessFlowGraph(
        organizationId,
        projectId,
        processDefinitionIds,
        from,
        to,
      ),
    enabled: processDefinitionIds.length > 0,
  });
}
