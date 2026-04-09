export type Guid = string;
export type IsoDateString = string;

export enum AccountStatus {
  PendingProfile = 0,
  Created = 1,
  Verified = 2,
  Banned = 3,
  Deleted = 4,
}

export interface AuthTokensDto {
  accessToken: string;
  refreshToken: string;
  refreshTokenExpiresAt: IsoDateString;
}

export interface RefreshTokensDto {
  refreshToken: string;
}

export interface LogoutDto {
  refreshToken: string;
}

export interface GetAccountResponseDto {
  id: Guid;
  email: string;
  status: AccountStatus;
  lastLoginAt: IsoDateString;
  onboardingStartedAt: IsoDateString;
  onboardingCompletedAt: IsoDateString | null;
  createdAt: IsoDateString;
  updatedAt: IsoDateString;
}

export interface AdminUserDto {
  accountId: Guid;
  displayName: string;
  avatarUrl: string | null;
  locale: string | null;
  timezone: string | null;
  createdAt: IsoDateString;
  updatedAt: IsoDateString;
  deletedAt: IsoDateString | null;
}

export type UserRole = "Member" | "SuperAdmin";

export interface MeDto {
  account: GetAccountResponseDto;
  user: AdminUserDto | null;
  role: UserRole;
}

export interface CompleteOnboardingDto {
  displayName: string;
  avatarUrl: string | null;
  locale: string | null;
  timezone: string | null;
}

export interface CompleteOnboardingResultDto {
  account: GetAccountResponseDto;
  user: AdminUserDto;
}

export interface StartExternalAuthRequestDto {
  provider: string;
  successUrl?: string | null;
}

export interface ExternalAuthStartResponseDto {
  url: string;
}