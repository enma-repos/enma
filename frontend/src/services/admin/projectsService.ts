import { apiClient } from "@/api/apiClient";
import type {
  CreateProjectDto,
  Guid,
  ProjectDto,
  SetProjectDescriptionDto,
  SetProjectNameDto,
  SetProjectSettingsDto,
} from "@/types/admin.types";

export default class ProjectsService {
  private orgBaseUrl(organizationId: Guid): string {
    return `/api/admin/v1/organizations/${organizationId}/projects`;
  }

  public async create(organizationId: Guid, dto: Omit<CreateProjectDto, "organizationId">): Promise<ProjectDto> {
    const { data } = await apiClient.post<ProjectDto>(this.orgBaseUrl(organizationId), {
      ...dto,
      organizationId,
    } satisfies CreateProjectDto);
    return data;
  }

  public async getById(organizationId: Guid, projectId: Guid): Promise<ProjectDto> {
    const { data } = await apiClient.get<ProjectDto>(`${this.orgBaseUrl(organizationId)}/${projectId}`);
    return data;
  }

  public async getByOrgAndKey(organizationId: Guid, key: string): Promise<ProjectDto> {
    const { data } = await apiClient.get<ProjectDto>(
      `${this.orgBaseUrl(organizationId)}/by-key/${encodeURIComponent(key)}`,
    );
    return data;
  }

  public async listByOrg(organizationId: Guid, offset = 0, limit = 50): Promise<ProjectDto[]> {
    const { data } = await apiClient.get<ProjectDto[]>(this.orgBaseUrl(organizationId), {
      params: { offset, limit },
    });
    return data;
  }

  public async listByUser(offset = 0, limit = 50): Promise<ProjectDto[]> {
    const { data } = await apiClient.get<ProjectDto[]>("/api/admin/v1/me/projects", {
      params: { offset, limit },
    });
    return data;
  }

  public async setName(organizationId: Guid, projectId: Guid, dto: SetProjectNameDto): Promise<void> {
    await apiClient.patch(`${this.orgBaseUrl(organizationId)}/${projectId}/name`, dto);
  }

  public async setDescription(organizationId: Guid, projectId: Guid, dto: SetProjectDescriptionDto): Promise<void> {
    await apiClient.patch(`${this.orgBaseUrl(organizationId)}/${projectId}/description`, dto);
  }

  public async setSettings(organizationId: Guid, projectId: Guid, dto: SetProjectSettingsDto): Promise<void> {
    await apiClient.patch(`${this.orgBaseUrl(organizationId)}/${projectId}/settings`, dto);
  }

  public async archive(organizationId: Guid, projectId: Guid): Promise<void> {
    await apiClient.patch(`${this.orgBaseUrl(organizationId)}/${projectId}/archive`);
  }

  public async unarchive(organizationId: Guid, projectId: Guid): Promise<void> {
    await apiClient.patch(`${this.orgBaseUrl(organizationId)}/${projectId}/unarchive`);
  }

  public async softDelete(organizationId: Guid, projectId: Guid): Promise<void> {
    await apiClient.delete(`${this.orgBaseUrl(organizationId)}/${projectId}`);
  }
}
