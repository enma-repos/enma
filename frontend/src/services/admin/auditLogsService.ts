import { apiClient } from "@/api/apiClient";
import type { AuditLogDto, CreateAuditLogDto, Guid, IsoDateString, PaginatedResult } from "@/types/admin.types";

type DateQuery = IsoDateString | Date;

function toIsoOrUndefined(value: DateQuery | null | undefined): IsoDateString | undefined {
  if (value == null) return undefined;
  return value instanceof Date ? value.toISOString() : value;
}

export interface AuditLogFilters {
  from?: DateQuery | null;
  to?: DateQuery | null;
  action?: string | null;
  resourceType?: string | null;
  actorUserId?: Guid | null;
  search?: string | null;
  page?: number;
  pageSize?: number;
}

export default class AuditLogsService {
  private orgBaseUrl(organizationId: Guid): string {
    return `/api/admin/v1/organizations/${organizationId}`;
  }

  public async append(organizationId: Guid, dto: Omit<CreateAuditLogDto, "organizationId">): Promise<void> {
    await apiClient.post(`${this.orgBaseUrl(organizationId)}/audit-logs`, {
      ...dto,
      organizationId,
    } satisfies CreateAuditLogDto);
  }

  public async listByOrg(
    organizationId: Guid,
    filters: AuditLogFilters = {},
  ): Promise<PaginatedResult<AuditLogDto>> {
    const { data } = await apiClient.get<PaginatedResult<AuditLogDto>>(
      `${this.orgBaseUrl(organizationId)}/audit-logs`,
      {
        params: {
          from: toIsoOrUndefined(filters.from),
          to: toIsoOrUndefined(filters.to),
          action: filters.action || undefined,
          resourceType: filters.resourceType || undefined,
          actorUserId: filters.actorUserId || undefined,
          search: filters.search || undefined,
          page: filters.page ?? 1,
          pageSize: filters.pageSize ?? 10,
        },
      },
    );
    return data;
  }

  public async listByProject(
    organizationId: Guid,
    projectId: Guid,
    filters: AuditLogFilters = {},
  ): Promise<PaginatedResult<AuditLogDto>> {
    const { data } = await apiClient.get<PaginatedResult<AuditLogDto>>(
      `${this.orgBaseUrl(organizationId)}/projects/${projectId}/audit-logs`,
      {
        params: {
          from: toIsoOrUndefined(filters.from),
          to: toIsoOrUndefined(filters.to),
          action: filters.action || undefined,
          resourceType: filters.resourceType || undefined,
          actorUserId: filters.actorUserId || undefined,
          search: filters.search || undefined,
          page: filters.page ?? 1,
          pageSize: filters.pageSize ?? 10,
        },
      },
    );
    return data;
  }
}
