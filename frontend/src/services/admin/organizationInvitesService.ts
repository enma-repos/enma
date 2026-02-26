import { apiClient } from "@/api/apiClient";
import type {
  CreateOrganizationInviteDto,
  Guid,
  OrganizationInviteDto,
  SetInviteAcceptedDto,
} from "@/types/admin.types";

export default class OrganizationInvitesService {
  private orgBaseUrl(organizationId: Guid): string {
    return `/api/admin/v1/organizations/${organizationId}/invites`;
  }

  public async create(
    organizationId: Guid,
    dto: Omit<CreateOrganizationInviteDto, "organizationId">,
  ): Promise<OrganizationInviteDto> {
    const { data } = await apiClient.post<OrganizationInviteDto>(this.orgBaseUrl(organizationId), {
      ...dto,
      organizationId,
    } satisfies CreateOrganizationInviteDto);
    return data;
  }

  public async getById(organizationId: Guid, inviteId: Guid): Promise<OrganizationInviteDto> {
    const { data } = await apiClient.get<OrganizationInviteDto>(`${this.orgBaseUrl(organizationId)}/${inviteId}`);
    return data;
  }

  public async listActive(organizationId: Guid, offset = 0, limit = 50): Promise<OrganizationInviteDto[]> {
    const { data } = await apiClient.get<OrganizationInviteDto[]>(`${this.orgBaseUrl(organizationId)}/active`, {
      params: { offset, limit },
    });
    return data;
  }

  public async getActiveByEmail(organizationId: Guid, email: string): Promise<OrganizationInviteDto> {
    const { data } = await apiClient.get<OrganizationInviteDto>(`${this.orgBaseUrl(organizationId)}/active/by-email`, {
      params: { email },
    });
    return data;
  }

  public async accept(organizationId: Guid, inviteId: Guid, dto: SetInviteAcceptedDto): Promise<void> {
    await apiClient.patch(`${this.orgBaseUrl(organizationId)}/${inviteId}/accept`, dto);
  }
}
