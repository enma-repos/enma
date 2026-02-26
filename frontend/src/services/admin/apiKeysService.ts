import { apiClient } from "@/api/apiClient";
import type { ApiKeyDto, ApiKeyFirstCreationDto, Guid } from "@/types/admin.types";

export default class ApiKeysService {
  private clientBaseUrl(organizationId: Guid, projectId: Guid, clientId: Guid): string {
    return `/api/admin/v1/organizations/${organizationId}/projects/${projectId}/sdk-clients/${clientId}/api-keys`;
  }

  public async create(organizationId: Guid, projectId: Guid, clientId: Guid): Promise<ApiKeyFirstCreationDto> {
    const { data } = await apiClient.post<ApiKeyFirstCreationDto>(this.clientBaseUrl(organizationId, projectId, clientId));
    return data;
  }

  public async getById(
    organizationId: Guid,
    projectId: Guid,
    clientId: Guid,
    apiKeyId: Guid,
  ): Promise<ApiKeyDto> {
    const { data } = await apiClient.get<ApiKeyDto>(`${this.clientBaseUrl(organizationId, projectId, clientId)}/${apiKeyId}`);
    return data;
  }

  public async listBySdkClient(
    organizationId: Guid,
    projectId: Guid,
    clientId: Guid,
    offset = 0,
    limit = 50,
  ): Promise<ApiKeyDto[]> {
    const { data } = await apiClient.get<ApiKeyDto[]>(this.clientBaseUrl(organizationId, projectId, clientId), {
      params: { offset, limit },
    });
    return data;
  }

  public async listActiveByPrefix(
    organizationId: Guid,
    projectId: Guid,
    clientId: Guid,
    keyPrefix: string,
    limit = 50,
  ): Promise<ApiKeyDto[]> {
    void organizationId;
    void projectId;
    void clientId;
    const { data } = await apiClient.get<ApiKeyDto[]>("/api/v1/api-keys/active", {
      params: { keyPrefix, limit },
    });
    return data;
  }

  public async updateLastUsed(organizationId: Guid, projectId: Guid, clientId: Guid, apiKeyId: Guid): Promise<void> {
    await apiClient.patch(`${this.clientBaseUrl(organizationId, projectId, clientId)}/${apiKeyId}/last-used`);
  }

  public async revoke(organizationId: Guid, projectId: Guid, clientId: Guid, apiKeyId: Guid): Promise<void> {
    await apiClient.patch(`${this.clientBaseUrl(organizationId, projectId, clientId)}/${apiKeyId}/revoke`);
  }
}
