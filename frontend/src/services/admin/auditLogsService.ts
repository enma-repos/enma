import { apiClient } from "@/api/apiClient";
import type { AuditLogDto, CreateAuditLogDto, Guid, IsoDateString } from "@/types/admin.types";

type DateQuery = IsoDateString | Date;

function toIsoOrUndefined(value: DateQuery | null | undefined): IsoDateString | undefined {
  if (value == null) return undefined;
  return value instanceof Date ? value.toISOString() : value;
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
    from: DateQuery | null = null,
    to: DateQuery | null = null,
    offset = 0,
    limit = 50,
  ): Promise<AuditLogDto[]> {
    const { data } = await apiClient.get<AuditLogDto[]>(`${this.orgBaseUrl(organizationId)}/audit-logs`, {
      params: { from: toIsoOrUndefined(from), to: toIsoOrUndefined(to), offset, limit },
    });
    return data;
  }

  public async listByProject(
    organizationId: Guid,
    projectId: Guid,
    from: DateQuery | null = null,
    to: DateQuery | null = null,
    offset = 0,
    limit = 50,
  ): Promise<AuditLogDto[]> {
    const { data } = await apiClient.get<AuditLogDto[]>(`${this.orgBaseUrl(organizationId)}/projects/${projectId}/audit-logs`, {
      params: { from: toIsoOrUndefined(from), to: toIsoOrUndefined(to), offset, limit },
    });
    return data;
  }
}
