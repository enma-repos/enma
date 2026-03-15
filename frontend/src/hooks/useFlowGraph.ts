import { useQuery } from "@tanstack/react-query";
import type { FlowGraphDto } from "@/types/analytics.types";

// ── Mock data ────────────────────────────────────────────────────────
const MOCK_FLOW_GRAPH: FlowGraphDto = {
  nodes: [
    { eventName: "session_start", visits: 4200, entries: 4200, exits: 0, uniqueChains: 3800 },
    { eventName: "page_view_home", visits: 3900, entries: 320, exits: 180, uniqueChains: 3600 },
    { eventName: "page_view_catalog", visits: 2800, entries: 150, exits: 420, uniqueChains: 2500 },
    { eventName: "search", visits: 1600, entries: 80, exits: 310, uniqueChains: 1400 },
    { eventName: "product_view", visits: 2200, entries: 60, exits: 560, uniqueChains: 1900 },
    { eventName: "add_to_cart", visits: 980, entries: 10, exits: 220, uniqueChains: 850 },
    { eventName: "begin_checkout", visits: 620, entries: 5, exits: 140, uniqueChains: 580 },
    { eventName: "purchase", visits: 410, entries: 0, exits: 410, uniqueChains: 400 },
    { eventName: "page_view_blog", visits: 750, entries: 200, exits: 380, uniqueChains: 650 },
    { eventName: "sign_up", visits: 340, entries: 0, exits: 180, uniqueChains: 320 },
  ],
  edges: [
    { fromEvent: "session_start", toEvent: "page_view_home", transitions: 3600, uniqueChains: 3400 },
    { fromEvent: "session_start", toEvent: "page_view_catalog", transitions: 400, uniqueChains: 350 },
    { fromEvent: "session_start", toEvent: "page_view_blog", transitions: 200, uniqueChains: 180 },
    { fromEvent: "page_view_home", toEvent: "page_view_catalog", transitions: 2100, uniqueChains: 1900 },
    { fromEvent: "page_view_home", toEvent: "search", transitions: 900, uniqueChains: 800 },
    { fromEvent: "page_view_home", toEvent: "page_view_blog", transitions: 450, uniqueChains: 400 },
    { fromEvent: "page_view_home", toEvent: "sign_up", transitions: 250, uniqueChains: 230 },
    { fromEvent: "page_view_catalog", toEvent: "product_view", transitions: 1800, uniqueChains: 1600 },
    { fromEvent: "page_view_catalog", toEvent: "search", transitions: 500, uniqueChains: 450 },
    { fromEvent: "search", toEvent: "product_view", transitions: 1100, uniqueChains: 950 },
    { fromEvent: "search", toEvent: "page_view_catalog", transitions: 300, uniqueChains: 270 },
    { fromEvent: "product_view", toEvent: "add_to_cart", transitions: 950, uniqueChains: 820 },
    { fromEvent: "product_view", toEvent: "page_view_catalog", transitions: 600, uniqueChains: 520 },
    { fromEvent: "add_to_cart", toEvent: "begin_checkout", transitions: 600, uniqueChains: 560 },
    { fromEvent: "add_to_cart", toEvent: "product_view", transitions: 180, uniqueChains: 160 },
    { fromEvent: "begin_checkout", toEvent: "purchase", transitions: 410, uniqueChains: 400 },
    { fromEvent: "begin_checkout", toEvent: "add_to_cart", transitions: 80, uniqueChains: 70 },
    { fromEvent: "page_view_blog", toEvent: "sign_up", transitions: 90, uniqueChains: 85 },
    { fromEvent: "page_view_blog", toEvent: "page_view_home", transitions: 120, uniqueChains: 110 },
  ],
};

/**
 * Fetches the flow graph for a given process definition.
 *
 * TODO: replace mock with AnalyticsService.getFlowGraph()
 */
export function useFlowGraph(
  _organizationId: string,
  _projectId: string,
  _processDefinitionId: string | null,
  _from: string,
  _to: string,
) {
  return useQuery<FlowGraphDto>({
    queryKey: [
      "flow-graph",
      _organizationId,
      _projectId,
      _processDefinitionId,
      _from,
      _to,
    ],
    queryFn: async () => {
      // TODO: return analyticsService.getFlowGraph(...)
      await new Promise((r) => setTimeout(r, 600)); // simulate latency
      return MOCK_FLOW_GRAPH;
    },
    enabled: !!_processDefinitionId,
  });
}
