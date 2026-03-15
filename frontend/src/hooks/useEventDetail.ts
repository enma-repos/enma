import { useQuery } from "@tanstack/react-query";
import type { EventDetailDto } from "@/types/analytics.types";

// ── Mock lookup ──────────────────────────────────────────────────────
const MOCK_DETAILS: Record<string, EventDetailDto> = {
  session_start: {
    eventName: "session_start",
    totalVisits: 4200,
    totalEntries: 4200,
    totalExits: 0,
    totalUniqueChains: 3800,
    incomingEdges: [],
    outgoingEdges: [
      { fromEvent: "session_start", toEvent: "page_view_home", transitions: 3600, uniqueChains: 3400 },
      { fromEvent: "session_start", toEvent: "page_view_catalog", transitions: 400, uniqueChains: 350 },
      { fromEvent: "session_start", toEvent: "page_view_blog", transitions: 200, uniqueChains: 180 },
    ],
  },
  product_view: {
    eventName: "product_view",
    totalVisits: 2200,
    totalEntries: 60,
    totalExits: 560,
    totalUniqueChains: 1900,
    incomingEdges: [
      { fromEvent: "page_view_catalog", toEvent: "product_view", transitions: 1800, uniqueChains: 1600 },
      { fromEvent: "search", toEvent: "product_view", transitions: 1100, uniqueChains: 950 },
      { fromEvent: "add_to_cart", toEvent: "product_view", transitions: 180, uniqueChains: 160 },
    ],
    outgoingEdges: [
      { fromEvent: "product_view", toEvent: "add_to_cart", transitions: 950, uniqueChains: 820 },
      { fromEvent: "product_view", toEvent: "page_view_catalog", transitions: 600, uniqueChains: 520 },
    ],
  },
  purchase: {
    eventName: "purchase",
    totalVisits: 410,
    totalEntries: 0,
    totalExits: 410,
    totalUniqueChains: 400,
    incomingEdges: [
      { fromEvent: "begin_checkout", toEvent: "purchase", transitions: 410, uniqueChains: 400 },
    ],
    outgoingEdges: [],
  },
};

function makeFallback(eventName: string): EventDetailDto {
  return {
    eventName,
    totalVisits: 0,
    totalEntries: 0,
    totalExits: 0,
    totalUniqueChains: 0,
    incomingEdges: [],
    outgoingEdges: [],
  };
}

/**
 * Fetches event detail when a node is selected.
 *
 * TODO: replace mock with AnalyticsService.getEventDetail()
 */
export function useEventDetail(
  _organizationId: string,
  _projectId: string,
  _processDefinitionId: string | null,
  eventName: string | null,
  _from: string,
  _to: string,
) {
  return useQuery<EventDetailDto>({
    queryKey: [
      "event-detail",
      _organizationId,
      _projectId,
      _processDefinitionId,
      eventName,
      _from,
      _to,
    ],
    queryFn: async () => {
      // TODO: return analyticsService.getEventDetail(...)
      await new Promise((r) => setTimeout(r, 400));
      return MOCK_DETAILS[eventName!] ?? makeFallback(eventName!);
    },
    enabled: !!eventName && !!_processDefinitionId,
  });
}
