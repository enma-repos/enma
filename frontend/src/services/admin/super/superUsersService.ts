import { apiClient } from "@/api/apiClient";
import type { Guid, PaginatedResult } from "@/types/admin.types";
import type {
  SuperListQuery,
  SuperUserDetailsDto,
  SuperUserListItemDto,
} from "@/types/super-admin.types";

// Future: softDelete, restore, ban.
export default class SuperUsersService {
  private readonly baseUrl = "/api/admin/v1/super/users";

  public async list(query: SuperListQuery = {}): Promise<PaginatedResult<SuperUserListItemDto>> {
    const { data } = await apiClient.get<PaginatedResult<SuperUserListItemDto>>(this.baseUrl, {
      params: {
        page: query.page ?? 1,
        pageSize: query.pageSize ?? 20,
        search: query.search || undefined,
        includeDeleted: query.includeDeleted ?? false,
      },
    });
    return data;
  }

  public async getById(userId: Guid): Promise<SuperUserDetailsDto> {
    const { data } = await apiClient.get<SuperUserDetailsDto>(`${this.baseUrl}/${userId}`);
    return data;
  }
}
