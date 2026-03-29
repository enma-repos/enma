export type MetricTrend = {
  percent: number;
  absolute: number;
};

export type AnalyticsMetric = {
  id: string;
  label: string;
  value: string;
  trend: MetricTrend;
  tone: "red" | "purple" | "teal" | "zinc";
};

export type PopularEvent = {
  id: string;
  title: string;
  subtitle: string;
  value: string;
  deltaPercent: number;
  icon: "eye" | "click" | "login" | "cart" | "card" | "search";
  color: "blue" | "amber" | "emerald" | "violet" | "rose" | "cyan";
};

// --- Flow Graph ---

export interface FlowNodeDto {
  eventName: string;
  visits: number;
  entries: number;
  exits: number;
  uniqueChains: number;
}

export interface FlowEdgeDto {
  fromEvent: string;
  toEvent: string;
  transitions: number;
  uniqueChains: number;
}

export interface FlowGraphDto {
  nodes: FlowNodeDto[];
  edges: FlowEdgeDto[];
}

// --- Funnel ---

export interface FunnelAnalysisRequest {
  steps: string[];
}

export interface FunnelStepDto {
  eventName: string;
  stepIndex: number;
  visits: number;
  conversionRateFromPrevious: number;
  conversionRateFromFirst: number;
}

export interface FunnelAnalysisDto {
  steps: FunnelStepDto[];
}

// --- Top Events ---

export interface TopEventItemDto {
  eventName: string;
  visits: number;
  entries: number;
  exits: number;
  uniqueChains: number;
}

export interface TopEventsDto {
  events: TopEventItemDto[];
}

// --- Entry/Exit ---

export interface EntryExitPointDto {
  eventName: string;
  count: number;
  rate: number;
}

export interface EntryExitPointsDto {
  entries: EntryExitPointDto[];
  exits: EntryExitPointDto[];
}

// --- Time Trends ---

export interface TimeTrendPointDto {
  bucketStart: string;
  visits: number;
  entries: number;
  exits: number;
  uniqueChains: number;
}

export interface TimeTrendsDto {
  points: TimeTrendPointDto[];
  granularity: string;
}

// --- Event Detail ---

export interface EventDetailDto {
  eventName: string;
  totalVisits: number;
  totalEntries: number;
  totalExits: number;
  totalUniqueChains: number;
  incomingEdges: FlowEdgeDto[];
  outgoingEdges: FlowEdgeDto[];
}

// --- Actor Breakdown ---

export interface ActorBreakdownItemDto {
  fromEvent: string;
  toEvent: string;
  uniqueUsers: number;
  uniqueAnonymous: number;
  totalTransitions: number;
  userRate: number;
  anonymousRate: number;
}

export interface ActorBreakdownDto {
  edges: ActorBreakdownItemDto[];
}

// --- Summary ---

export interface SummaryMetricDto {
  value: number;
  trendPercent: number;
  trendAbsolute: number;
}

export interface SummaryDto {
  totalVisits: SummaryMetricDto;
  uniqueChains: SummaryMetricDto;
  uniqueUsers: SummaryMetricDto;
  avgStepsPerChain: SummaryMetricDto;
}

