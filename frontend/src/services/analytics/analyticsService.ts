import { apiClient } from "@/api/apiClient";
import type { Guid } from "@/types/admin.types";
import type {
  ActorBreakdownDto,
  EntryExitPointsDto,
  EventDetailDto,
  FlowGraphDto,
  FunnelAnalysisDto,
  FunnelAnalysisRequest,
  SummaryDto,
  TimeTrendsDto,
  TopEventsDto,
} from "@/types/analytics.types";

export default class AnalyticsService {
  private baseUrl(organizationId: Guid, projectId: Guid, processDefinitionId: Guid): string {
    return `/api/analytics/v1/organizations/${organizationId}/projects/${projectId}/process-definitions/${processDefinitionId}`;
  }

  public async getFlowGraph(
    organizationId: Guid,
    projectId: Guid,
    processDefinitionId: Guid,
    from: string,
    to: string,
    entryEvent?: string,
  ): Promise<FlowGraphDto> {
    const { data } = await apiClient.get<FlowGraphDto>(
      `${this.baseUrl(organizationId, projectId, processDefinitionId)}/flow`,
      { params: { from, to, ...(entryEvent ? { entryEvent } : {}) } },
    );
    return data;
  }

  private projectBaseUrl(organizationId: Guid, projectId: Guid): string {
    return `/api/analytics/v1/organizations/${organizationId}/projects/${projectId}`;
  }

  public async getMultiProcessFlowGraph(
    organizationId: Guid,
    projectId: Guid,
    processDefinitionIds: Guid[],
    from: string,
    to: string,
  ): Promise<FlowGraphDto> {
    const { data } = await apiClient.post<FlowGraphDto>(
      `${this.projectBaseUrl(organizationId, projectId)}/flow`,
      { processDefinitionIds },
      { params: { from, to } },
    );
    return data;
  }

  public async analyzeFunnel(
    organizationId: Guid,
    projectId: Guid,
    processDefinitionId: Guid,
    from: string,
    to: string,
    request: FunnelAnalysisRequest,
  ): Promise<FunnelAnalysisDto> {
    const { data } = await apiClient.post<FunnelAnalysisDto>(
      `${this.baseUrl(organizationId, projectId, processDefinitionId)}/funnel`,
      request,
      { params: { from, to } },
    );
    return data;
  }

  public async getTopEvents(
    organizationId: Guid,
    projectId: Guid,
    processDefinitionId: Guid,
    from: string,
    to: string,
    sortBy = "visits",
    limit = 20,
  ): Promise<TopEventsDto> {
    const { data } = await apiClient.get<TopEventsDto>(
      `${this.baseUrl(organizationId, projectId, processDefinitionId)}/top-events`,
      { params: { from, to, sortBy, limit } },
    );
    return data;
  }

  public async getEntryExitPoints(
    organizationId: Guid,
    projectId: Guid,
    processDefinitionId: Guid,
    from: string,
    to: string,
    limit = 10,
  ): Promise<EntryExitPointsDto> {
    const { data } = await apiClient.get<EntryExitPointsDto>(
      `${this.baseUrl(organizationId, projectId, processDefinitionId)}/entry-exit`,
      { params: { from, to, limit } },
    );
    return data;
  }

  public async getTimeTrends(
    organizationId: Guid,
    projectId: Guid,
    processDefinitionId: Guid,
    from: string,
    to: string,
    granularity = "Hour",
  ): Promise<TimeTrendsDto> {
    const { data } = await apiClient.get<TimeTrendsDto>(
      `${this.baseUrl(organizationId, projectId, processDefinitionId)}/trends`,
      { params: { from, to, granularity } },
    );
    return data;
  }

  public async getEventDetail(
    organizationId: Guid,
    projectId: Guid,
    processDefinitionId: Guid,
    eventName: string,
    from: string,
    to: string,
  ): Promise<EventDetailDto> {
    const { data } = await apiClient.get<EventDetailDto>(
      `${this.baseUrl(organizationId, projectId, processDefinitionId)}/events/${eventName}`,
      { params: { from, to } },
    );
    return data;
  }

  public async getSummary(
    organizationId: Guid,
    projectId: Guid,
    processDefinitionIds: Guid[] | null,
    from: string,
    to: string,
  ): Promise<SummaryDto> {
    const { data } = await apiClient.post<SummaryDto>(
      `${this.projectBaseUrl(organizationId, projectId)}/summary`,
      { processDefinitionIds },
      { params: { from, to } },
    );
    return data;
  }

  public async getActorBreakdown(
    organizationId: Guid,
    projectId: Guid,
    processDefinitionId: Guid,
    from: string,
    to: string,
  ): Promise<ActorBreakdownDto> {
    const { data } = await apiClient.get<ActorBreakdownDto>(
      `${this.baseUrl(organizationId, projectId, processDefinitionId)}/actor-breakdown`,
      { params: { from, to } },
    );
    return data;
  }
}
