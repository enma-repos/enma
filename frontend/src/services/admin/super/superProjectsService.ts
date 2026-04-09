import { apiClient } from "@/api/apiClient";
import type { Guid, PaginatedResult } from "@/types/admin.types";
import type {
  SuperProjectDetailsDto,
  SuperProjectListItemDto,
  SuperProjectsQuery,
} from "@/types/super-admin.types";

// Future: softDelete, restore, archive, unarchive.
export default class SuperProjectsService {
  private readonly baseUrl = "/api/admin/v1/super/projects";

  public async list(
    query: SuperProjectsQuery = {},
  ): Promise<PaginatedResult<SuperProjectListItemDto>> {
    const { data } = await apiClient.get<PaginatedResult<SuperProjectListItemDto>>(this.baseUrl, {
      params: {
        page: query.page ?? 1,
        pageSize: query.pageSize ?? 20,
        search: query.search || undefined,
        includeDeleted: query.includeDeleted ?? false,
        organizationId: query.organizationId || undefined,
      },
    });
    return data;
  }

  public async getById(projectId: Guid): Promise<SuperProjectDetailsDto> {
    const { data } = await apiClient.get<SuperProjectDetailsDto>(`${this.baseUrl}/${projectId}`);
    return data;
  }
}
