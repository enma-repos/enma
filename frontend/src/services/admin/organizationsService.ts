import { apiClient } from "@/api/apiClient";
import type {
  CreateOrganizationDto,
  Guid,
  OrganizationDto,
  SetOrganizationNameDto,
  SetOrganizationOwnerDto,
} from "@/types/admin.types";

export default class OrganizationsService {
  private readonly baseUrl = "/api/admin/v1/organizations";

  public async create(dto: CreateOrganizationDto): Promise<OrganizationDto> {
    const { data } = await apiClient.post<OrganizationDto>(this.baseUrl, dto);
    return data;
  }

  public async getById(organizationId: Guid): Promise<OrganizationDto> {
    const { data } = await apiClient.get<OrganizationDto>(`${this.baseUrl}/${organizationId}`);
    return data;
  }

  public async getBySlug(slug: string): Promise<OrganizationDto> {
    const { data } = await apiClient.get<OrganizationDto>(`${this.baseUrl}/by-slug/${encodeURIComponent(slug)}`);
    return data;
  }

  public async listByUser(offset = 0, limit = 50): Promise<OrganizationDto[]> {
    const { data } = await apiClient.get<OrganizationDto[]>(this.baseUrl, {
      params: { offset, limit },
    });
    return data;
  }

  public async setName(organizationId: Guid, dto: SetOrganizationNameDto): Promise<void> {
    await apiClient.patch(`${this.baseUrl}/${organizationId}/name`, dto);
  }

  public async setOwner(organizationId: Guid, dto: SetOrganizationOwnerDto): Promise<void> {
    await apiClient.patch(`${this.baseUrl}/${organizationId}/owner`, dto);
  }

  public async softDelete(organizationId: Guid): Promise<void> {
    await apiClient.delete(`${this.baseUrl}/${organizationId}`);
  }
}
