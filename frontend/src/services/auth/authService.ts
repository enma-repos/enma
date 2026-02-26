import { apiClient } from "@/api/apiClient";
import type {
  AuthTokensDto,
  CompleteOnboardingDto,
  CompleteOnboardingResultDto,
  LogoutDto,
  MeDto,
  RefreshTokensDto,
} from "@/types/auth.types";

export default class AuthService {
  private readonly baseUrl = "/api/auth/v1";

  public async refresh(dto: RefreshTokensDto): Promise<AuthTokensDto> {
    const { data } = await apiClient.post<AuthTokensDto>(`${this.baseUrl}/refresh`, dto);
    return data;
  }

  public async logout(dto: LogoutDto): Promise<void> {
    await apiClient.post(`${this.baseUrl}/logout`, dto);
  }

  public async getMe(): Promise<MeDto> {
    const { data } = await apiClient.get<MeDto>(`${this.baseUrl}/me`);
    return data;
  }

  public async completeOnboarding(dto: CompleteOnboardingDto): Promise<CompleteOnboardingResultDto> {
    const { data } = await apiClient.post<CompleteOnboardingResultDto>(`${this.baseUrl}/onboarding/complete`, dto);
    return data;
  }
}
