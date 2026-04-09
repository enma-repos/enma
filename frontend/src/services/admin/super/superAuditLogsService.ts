import { apiClient } from "@/api/apiClient";
import type { AuditLogDto, PaginatedResult } from "@/types/admin.types";
import type { SuperAuditLogsQuery } from "@/types/super-admin.types";

export default class SuperAuditLogsService {
  private readonly baseUrl = "/api/admin/v1/super/audit-logs";

  public async list(query: SuperAuditLogsQuery = {}): Promise<PaginatedResult<AuditLogDto>> {
    const { data } = await apiClient.get<PaginatedResult<AuditLogDto>>(this.baseUrl, {
      params: {
        page: query.page ?? 1,
        pageSize: query.pageSize ?? 20,
        search: query.search || undefined,
        from: query.from || undefined,
        to: query.to || undefined,
        action: query.action || undefined,
        resourceType: query.resourceType || undefined,
        actorUserId: query.actorUserId || undefined,
        organizationId: query.organizationId || undefined,
        projectId: query.projectId || undefined,
      },
    });
    return data;
  }
}
