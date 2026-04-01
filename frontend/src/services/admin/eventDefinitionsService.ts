import { apiClient } from "@/api/apiClient";
import type {
  CreateEventDefinitionDto,
  EventDefinitionDto,
  Guid,
  PaginatedResult,
  SetEventDefinitionDescriptionDto,
} from "@/types/admin.types";

export default class EventDefinitionsService {
  private baseUrl(organizationId: Guid, projectId: Guid): string {
    return `/api/admin/v1/organizations/${organizationId}/projects/${projectId}/event-definitions`;
  }

  public async create(
    organizationId: Guid,
    projectId: Guid,
    dto: Omit<CreateEventDefinitionDto, "projectId">,
  ): Promise<EventDefinitionDto> {
    const { data } = await apiClient.post<EventDefinitionDto>(this.baseUrl(organizationId, projectId), {
      ...dto,
      projectId,
    } satisfies CreateEventDefinitionDto);
    return data;
  }

  public async getById(organizationId: Guid, projectId: Guid, id: Guid): Promise<EventDefinitionDto> {
    const { data } = await apiClient.get<EventDefinitionDto>(`${this.baseUrl(organizationId, projectId)}/${id}`);
    return data;
  }

  public async getByProjectAndName(organizationId: Guid, projectId: Guid, name: string): Promise<EventDefinitionDto> {
    const { data } = await apiClient.get<EventDefinitionDto>(
      `${this.baseUrl(organizationId, projectId)}/by-name/${name}`,
    );
    return data;
  }

  public async listByProject(
    organizationId: Guid,
    projectId: Guid,
    page = 1,
    pageSize = 10,
    search?: string,
  ): Promise<PaginatedResult<EventDefinitionDto>> {
    const { data } = await apiClient.get<PaginatedResult<EventDefinitionDto>>(this.baseUrl(organizationId, projectId), {
      params: { page, pageSize, search: search || undefined },
    });
    return data;
  }

  public async setDescription(
    organizationId: Guid,
    projectId: Guid,
    id: Guid,
    dto: SetEventDefinitionDescriptionDto,
  ): Promise<void> {
    await apiClient.patch(`${this.baseUrl(organizationId, projectId)}/${id}/description`, dto);
  }

  public async delete(organizationId: Guid, projectId: Guid, id: Guid): Promise<void> {
    await apiClient.delete(`${this.baseUrl(organizationId, projectId)}/${id}`);
  }
}
