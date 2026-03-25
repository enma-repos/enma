import { apiClient } from "@/api/apiClient";
import type { Guid, NotificationDto } from "@/types/admin.types";

const BASE_URL = "/api/admin/v1/notifications";

export default class NotificationsService {
  public async list(unreadOnly = false, offset = 0, limit = 20): Promise<NotificationDto[]> {
    const { data } = await apiClient.get<NotificationDto[]>(BASE_URL, {
      params: { unreadOnly, offset, limit },
    });
    return data;
  }

  public async getUnreadCount(): Promise<number> {
    const { data } = await apiClient.get<number>(`${BASE_URL}/unread-count`);
    return data;
  }

  public async markAsRead(notificationId: Guid): Promise<void> {
    await apiClient.patch(`${BASE_URL}/${notificationId}/read`);
  }

  public async markAllAsRead(): Promise<void> {
    await apiClient.patch(`${BASE_URL}/read-all`);
  }

  public async delete(notificationId: Guid): Promise<void> {
    await apiClient.delete(`${BASE_URL}/${notificationId}`);
  }
}
