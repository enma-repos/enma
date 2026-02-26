import { apiClient } from "@/api/apiClient";
import type {
  Guid,
  SetUserAvatarUrlDto,
  SetUserDisplayNameDto,
  SetUserLocaleDto,
  SetUserTimezoneDto,
  UserDto,
} from "@/types/admin.types";

export default class UsersService {
  private readonly baseUrl = "/api/admin/v1/users";

  public async getById(userId: Guid): Promise<UserDto> {
    const { data } = await apiClient.get<UserDto>(`${this.baseUrl}/${userId}`);
    return data;
  }

  public async exists(userId: Guid): Promise<boolean> {
    const { data } = await apiClient.get<boolean>(`${this.baseUrl}/${userId}/exists`);
    return data;
  }

  public async setDisplayName(userId: Guid, dto: SetUserDisplayNameDto): Promise<void> {
    await apiClient.patch(`${this.baseUrl}/${userId}/display-name`, dto);
  }

  public async setAvatarUrl(userId: Guid, dto: SetUserAvatarUrlDto): Promise<void> {
    await apiClient.patch(`${this.baseUrl}/${userId}/avatar-url`, dto);
  }

  public async setLocale(userId: Guid, dto: SetUserLocaleDto): Promise<void> {
    await apiClient.patch(`${this.baseUrl}/${userId}/locale`, dto);
  }

  public async setTimezone(userId: Guid, dto: SetUserTimezoneDto): Promise<void> {
    await apiClient.patch(`${this.baseUrl}/${userId}/timezone`, dto);
  }

  public async softDelete(userId: Guid): Promise<void> {
    await apiClient.delete(`${this.baseUrl}/${userId}`);
  }
}
