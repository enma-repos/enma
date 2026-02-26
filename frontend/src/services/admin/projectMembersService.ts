import { apiClient } from "@/api/apiClient";
import type { AddProjectMemberDto, Guid, ProjectMemberDto, SetProjectMemberRoleDto } from "@/types/admin.types";

export default class ProjectMembersService {
  private projectBaseUrl(organizationId: Guid, projectId: Guid): string {
    return `/api/admin/v1/organizations/${organizationId}/projects/${projectId}/members`;
  }

  public async add(
    organizationId: Guid,
    projectId: Guid,
    dto: Omit<AddProjectMemberDto, "projectId">,
  ): Promise<ProjectMemberDto> {
    const { data } = await apiClient.post<ProjectMemberDto>(this.projectBaseUrl(organizationId, projectId), {
      ...dto,
      projectId,
    } satisfies AddProjectMemberDto);
    return data;
  }

  public async get(organizationId: Guid, projectId: Guid, userId: Guid): Promise<ProjectMemberDto> {
    const { data } = await apiClient.get<ProjectMemberDto>(`${this.projectBaseUrl(organizationId, projectId)}/${userId}`);
    return data;
  }

  public async list(organizationId: Guid, projectId: Guid, offset = 0, limit = 50): Promise<ProjectMemberDto[]> {
    const { data } = await apiClient.get<ProjectMemberDto[]>(this.projectBaseUrl(organizationId, projectId), {
      params: { offset, limit },
    });
    return data;
  }

  public async setRole(
    organizationId: Guid,
    projectId: Guid,
    userId: Guid,
    dto: SetProjectMemberRoleDto,
  ): Promise<void> {
    await apiClient.patch(`${this.projectBaseUrl(organizationId, projectId)}/${userId}/role`, dto);
  }

  public async remove(organizationId: Guid, projectId: Guid, userId: Guid): Promise<void> {
    await apiClient.delete(`${this.projectBaseUrl(organizationId, projectId)}/${userId}`);
  }
}
