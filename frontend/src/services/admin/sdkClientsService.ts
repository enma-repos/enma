import { apiClient } from "@/api/apiClient";
import type {
  CreateSdkClientDto,
  Guid,
  SdkClientDto,
  SetSdkClientNameDto,
  SetSdkClientSettingsDto,
  SetSdkClientTypeDto,
} from "@/types/admin.types";

export default class SdkClientsService {
  private projectBaseUrl(organizationId: Guid, projectId: Guid): string {
    return `/api/admin/v1/organizations/${organizationId}/projects/${projectId}/sdk-clients`;
  }

  public async create(
    organizationId: Guid,
    projectId: Guid,
    dto: Omit<CreateSdkClientDto, "projectId">,
  ): Promise<SdkClientDto> {
    const { data } = await apiClient.post<SdkClientDto>(this.projectBaseUrl(organizationId, projectId), {
      ...dto,
      projectId,
    } satisfies CreateSdkClientDto);
    return data;
  }

  public async getById(organizationId: Guid, projectId: Guid, clientId: Guid): Promise<SdkClientDto> {
    const { data } = await apiClient.get<SdkClientDto>(`${this.projectBaseUrl(organizationId, projectId)}/${clientId}`);
    return data;
  }

  public async listByProject(organizationId: Guid, projectId: Guid, offset = 0, limit = 50): Promise<SdkClientDto[]> {
    const { data } = await apiClient.get<SdkClientDto[]>(this.projectBaseUrl(organizationId, projectId), {
      params: { offset, limit },
    });
    return data;
  }

  public async setName(organizationId: Guid, projectId: Guid, clientId: Guid, dto: SetSdkClientNameDto): Promise<void> {
    await apiClient.patch(`${this.projectBaseUrl(organizationId, projectId)}/${clientId}/name`, dto);
  }

  public async setSettings(
    organizationId: Guid,
    projectId: Guid,
    clientId: Guid,
    dto: SetSdkClientSettingsDto,
  ): Promise<void> {
    await apiClient.patch(`${this.projectBaseUrl(organizationId, projectId)}/${clientId}/settings`, dto);
  }

  public async setType(organizationId: Guid, projectId: Guid, clientId: Guid, dto: SetSdkClientTypeDto): Promise<void> {
    await apiClient.patch(`${this.projectBaseUrl(organizationId, projectId)}/${clientId}/type`, dto);
  }

  public async disable(organizationId: Guid, projectId: Guid, clientId: Guid): Promise<void> {
    await apiClient.patch(`${this.projectBaseUrl(organizationId, projectId)}/${clientId}/disable`);
  }

  public async enable(organizationId: Guid, projectId: Guid, clientId: Guid): Promise<void> {
    await apiClient.patch(`${this.projectBaseUrl(organizationId, projectId)}/${clientId}/enable`);
  }
}
