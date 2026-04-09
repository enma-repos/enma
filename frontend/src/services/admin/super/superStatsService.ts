import { apiClient } from "@/api/apiClient";
import type { SuperOverviewStatsDto } from "@/types/super-admin.types";

export default class SuperStatsService {
  private readonly baseUrl = "/api/admin/v1/super/stats";

  public async getOverview(): Promise<SuperOverviewStatsDto> {
    const { data } = await apiClient.get<SuperOverviewStatsDto>(`${this.baseUrl}/overview`);
    return data;
  }
}
