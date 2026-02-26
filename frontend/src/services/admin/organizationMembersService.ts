import { apiClient } from "@/api/apiClient";
import type {
  Guid,
  OrganizationMemberDto,
  SetOrganizationMemberRoleDto,
  SetOrganizationMemberStatusDto,
} from "@/types/admin.types";

export default class OrganizationMembersService {
  private orgBaseUrl(organizationId: Guid): string {
    return `/api/admin/v1/organizations/${organizationId}/members`;
  }

  public async get(organizationId: Guid, userId: Guid): Promise<OrganizationMemberDto> {
    const { data } = await apiClient.get<OrganizationMemberDto>(`${this.orgBaseUrl(organizationId)}/${userId}`);
    return data;
  }

  public async isMember(organizationId: Guid, userId: Guid): Promise<boolean> {
    const { data } = await apiClient.get<boolean>(`${this.orgBaseUrl(organizationId)}/${userId}/is-member`);
    return data;
  }

  public async list(organizationId: Guid, offset = 0, limit = 50): Promise<OrganizationMemberDto[]> {
    const { data } = await apiClient.get<OrganizationMemberDto[]>(this.orgBaseUrl(organizationId), {
      params: { offset, limit },
    });
    return data;
  }

  public async setRole(organizationId: Guid, userId: Guid, dto: SetOrganizationMemberRoleDto): Promise<void> {
    await apiClient.patch(`${this.orgBaseUrl(organizationId)}/${userId}/role`, dto);
  }

  public async setStatus(organizationId: Guid, userId: Guid, dto: SetOrganizationMemberStatusDto): Promise<void> {
    await apiClient.patch(`${this.orgBaseUrl(organizationId)}/${userId}/status`, dto);
  }

  public async remove(organizationId: Guid, userId: Guid): Promise<void> {
    await apiClient.delete(`${this.orgBaseUrl(organizationId)}/${userId}`);
  }
}
