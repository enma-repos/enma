import { apiClient } from "@/api/apiClient";
import type { Guid, PaginatedResult } from "@/types/admin.types";
import type {
  SuperListQuery,
  SuperOrganizationDetailsDto,
  SuperOrganizationListItemDto,
} from "@/types/super-admin.types";

// Future: softDelete, restore, transferOwnership.
export default class SuperOrganizationsService {
  private readonly baseUrl = "/api/admin/v1/super/organizations";

  public async list(
    query: SuperListQuery = {},
  ): Promise<PaginatedResult<SuperOrganizationListItemDto>> {
    const { data } = await apiClient.get<PaginatedResult<SuperOrganizationListItemDto>>(
      this.baseUrl,
      {
        params: {
          page: query.page ?? 1,
          pageSize: query.pageSize ?? 20,
          search: query.search || undefined,
          includeDeleted: query.includeDeleted ?? false,
        },
      },
    );
    return data;
  }

  public async getById(organizationId: Guid): Promise<SuperOrganizationDetailsDto> {
    const { data } = await apiClient.get<SuperOrganizationDetailsDto>(
      `${this.baseUrl}/${organizationId}`,
    );
    return data;
  }
}
