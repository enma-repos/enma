import { apiClient } from "@/api/apiClient";
import type {
  CreateProcessDefinitionDto,
  Guid,
  ProcessDefinitionDto,
  SetProcessDefinitionDescriptionDto,
  SetProcessDefinitionNameDto,
} from "@/types/admin.types";

export default class ProcessDefinitionsService {
  private baseUrl(organizationId: Guid, projectId: Guid): string {
    return `/api/admin/v1/organizations/${organizationId}/projects/${projectId}/process-definitions`;
  }

  public async create(
    organizationId: Guid,
    projectId: Guid,
    dto: Omit<CreateProcessDefinitionDto, "projectId">,
  ): Promise<ProcessDefinitionDto> {
    const { data } = await apiClient.post<ProcessDefinitionDto>(this.baseUrl(organizationId, projectId), {
      ...dto,
      projectId,
    } satisfies CreateProcessDefinitionDto);
    return data;
  }

  public async getById(organizationId: Guid, projectId: Guid, id: Guid): Promise<ProcessDefinitionDto> {
    const { data } = await apiClient.get<ProcessDefinitionDto>(`${this.baseUrl(organizationId, projectId)}/${id}`);
    return data;
  }

  public async getByProjectAndKey(organizationId: Guid, projectId: Guid, key: string): Promise<ProcessDefinitionDto> {
    const { data } = await apiClient.get<ProcessDefinitionDto>(
      `${this.baseUrl(organizationId, projectId)}/by-key/${key}`,
    );
    return data;
  }

  public async listByProject(
    organizationId: Guid,
    projectId: Guid,
    offset = 0,
    limit = 50,
  ): Promise<ProcessDefinitionDto[]> {
    const { data } = await apiClient.get<ProcessDefinitionDto[]>(this.baseUrl(organizationId, projectId), {
      params: { offset, limit },
    });
    return data;
  }

  public async setName(
    organizationId: Guid,
    projectId: Guid,
    id: Guid,
    dto: SetProcessDefinitionNameDto,
  ): Promise<void> {
    await apiClient.patch(`${this.baseUrl(organizationId, projectId)}/${id}/name`, dto);
  }

  public async setDescription(
    organizationId: Guid,
    projectId: Guid,
    id: Guid,
    dto: SetProcessDefinitionDescriptionDto,
  ): Promise<void> {
    await apiClient.patch(`${this.baseUrl(organizationId, projectId)}/${id}/description`, dto);
  }

  public async delete(organizationId: Guid, projectId: Guid, id: Guid): Promise<void> {
    await apiClient.delete(`${this.baseUrl(organizationId, projectId)}/${id}`);
  }
}
