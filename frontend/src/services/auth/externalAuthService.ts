import { apiClient } from "@/api/apiClient";
import type { ExternalAuthStartResponseDto, StartExternalAuthRequestDto } from "@/types/auth.types";

export default class ExternalAuthService {
  private readonly baseUrl = "/api/auth/v1/external";

  public async startGoogleAuth(dto: Omit<StartExternalAuthRequestDto, "provider">): Promise<ExternalAuthStartResponseDto> {
    const { data } = await apiClient.post<ExternalAuthStartResponseDto>(`${this.baseUrl}/google/start`, {
      ...dto,
      provider: "google",
    } satisfies StartExternalAuthRequestDto);
    return data;
  }
}
