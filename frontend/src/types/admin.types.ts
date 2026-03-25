export type Guid = string;
export type IsoDateString = string;

export type JsonObject = Record<string, unknown>;

export enum OrganizationRole {
  Owner = 0,
  Admin = 1,
  Analyst = 2,
  Developer = 3,
  Viewer = 4,
}

export enum OrganizationMemberStatus {
  Active = 0,
  Suspended = 1,
}

export enum ProjectRole {
  Owner = 0,
  Admin = 1,
  Analyst = 2,
  Developer = 3,
  Viewer = 4,
}

export enum SdkClientType {
  WebsiteSdk = 0,
  ServerToServer = 1,
  MobileSdk = 2,
}

export interface UserDto {
  id: Guid;
  email: string;
  displayName: string;
  avatarUrl: string | null;
  locale: string | null;
  timezone: string | null;
  createdAt: IsoDateString;
  updatedAt: IsoDateString;
  deletedAt: IsoDateString | null;
}

export interface SetUserDisplayNameDto {
  displayName: string;
}

export interface SetUserAvatarUrlDto {
  avatarUrl: string | null;
}

export interface SetUserLocaleDto {
  locale: string | null;
}

export interface SetUserTimezoneDto {
  timezone: string | null;
}

export interface OrganizationDto {
  id: Guid;
  ownerUserId: Guid;
  createdByUserId: Guid;
  name: string;
  slug: string;
  description: string | null;
  createdAt: IsoDateString;
  updatedAt: IsoDateString;
  deletedAt: IsoDateString | null;
}

export interface CreateOrganizationDto {
  name?: string | null;
  description?: string | null;
  slug: string;
}

export interface SetOrganizationNameDto {
  name: string;
}

export interface SetOrganizationOwnerDto {
  ownerUserId: Guid;
}

export interface ProjectDto {
  id: Guid;
  organizationId: Guid;
  name: string;
  key: string;
  description: string | null;
  isStared: boolean;
  createdByUserId: Guid;
  settings: JsonObject | null;
  createdAt: IsoDateString;
  updatedAt: IsoDateString;
  deletedAt: IsoDateString | null;
  archivedAt: IsoDateString | null;
}

export interface CreateProjectDto {
  organizationId: Guid;
  name?: string | null;
  key?: string | null;
  description?: string | null;
  isStared?: boolean;
  settings?: JsonObject | null;
}

export interface SetProjectNameDto {
  name: string;
}

export interface SetProjectDescriptionDto {
  description: string | null;
}

export interface SetProjectSettingsDto {
  settings: JsonObject | null;
}

export interface OrganizationMemberDto {
  organizationId: Guid;
  userId: Guid;
  role: OrganizationRole;
  status: OrganizationMemberStatus;
  displayName: string;
  email: string;
  avatarUrl: string | null;
  joinedAt: IsoDateString;
  updatedAt: IsoDateString;
}

export interface SetOrganizationMemberRoleDto {
  role: OrganizationRole;
}

export interface SetOrganizationMemberStatusDto {
  status: OrganizationMemberStatus;
}

export interface OrganizationInviteDto {
  id: Guid;
  organizationId: Guid;
  organizationName: string;
  targetEmail: string;
  role: OrganizationRole;
  expiresAt: IsoDateString;
  createdByUserId: Guid;
  acceptedUserId: Guid | null;
  createdAt: IsoDateString;
  acceptedAt: IsoDateString | null;
  declinedUserId: Guid | null;
  declinedAt: IsoDateString | null;
}

export interface CreateOrganizationInviteDto {
  organizationId: Guid;
  targetEmail: string;
  role: OrganizationRole;
}

export type SetInviteAcceptedDto = Record<string, never>;

export type SetInviteDeclinedDto = Record<string, never>;

export interface ProjectMemberDto {
  projectId: Guid;
  userId: Guid;
  role: ProjectRole;
  displayName: string;
  email: string;
  avatarUrl: string | null;
  joinedAt: IsoDateString;
  updatedAt: IsoDateString;
}

export interface AddProjectMemberDto {
  projectId: Guid;
  userId: Guid;
  role: ProjectRole;
}

export interface SetProjectMemberRoleDto {
  role: ProjectRole;
}

export interface SdkClientDto {
  id: Guid;
  projectId: Guid;
  name: string;
  type: SdkClientType;
  description: string | null;
  settings: JsonObject | null;
  createdAt: IsoDateString;
  updatedAt: IsoDateString;
  disabledAt: IsoDateString | null;
}

export interface CreateSdkClientDto {
  projectId: Guid;
  name?: string | null;
  description?: string | null;
  type: SdkClientType;
  settings?: JsonObject | null;
}

export interface SetSdkClientNameDto {
  name: string;
}

export interface SetSdkClientSettingsDto {
  settings: JsonObject | null;
}

export interface SetSdkClientTypeDto {
  type: SdkClientType;
}

export interface ApiKeyDto {
  id: Guid;
  sdkClientId: Guid;
  keyPrefix: string;
  sentEventsCount: number;
  createdAt: IsoDateString;
  lastUsedAt: IsoDateString | null;
  revokedAt: IsoDateString | null;
}

export interface ApiKeyFirstCreationDto extends ApiKeyDto {
  key: string;
}

export interface AuditLogDto {
  id: Guid;
  organizationId: Guid;
  projectId: Guid | null;
  actorUserId: Guid | null;
  action: string;
  resourceType: string;
  resourceId: string;
  ip: string | null;
  payload: JsonObject | null;
  createdAt: IsoDateString;
}

export interface CreateAuditLogDto {
  organizationId: Guid;
  projectId?: Guid | null;
  action?: string | null;
  resourceType?: string | null;
  resourceId?: string | null;
  ip?: string | null;
  payload?: JsonObject | null;
}

export interface PagedResult<T> {
  items: T[];
  total: number;
}

export enum ProcessType {
  Session = 0,
  Order = 1,
  Checkout = 2,
  Ticket = 3,
  Custom = 4,
}

export interface ProcessDefinitionDto {
  id: Guid;
  projectId: Guid;
  name: string;
  key: string;
  type: ProcessType;
  description: string | null;
  createdByUserId: Guid;
  createdAt: IsoDateString;
  updatedAt: IsoDateString;
  deletedAt: IsoDateString | null;
}

export interface CreateProcessDefinitionDto {
  projectId: Guid;
  name?: string | null;
  key?: string | null;
  type: ProcessType;
  description?: string | null;
}

export interface SetProcessDefinitionNameDto {
  name: string;
}

export interface SetProcessDefinitionDescriptionDto {
  description: string | null;
}

export interface EventDefinitionDto {
  id: Guid;
  projectId: Guid;
  name: string;
  description: string | null;
  createdByUserId: Guid;
  createdAt: IsoDateString;
  updatedAt: IsoDateString;
  deletedAt: IsoDateString | null;
}

export interface CreateEventDefinitionDto {
  projectId: Guid;
  name?: string | null;
  description?: string | null;
}

export interface SetEventDefinitionDescriptionDto {
  description: string | null;
}

export enum NotificationType {
  OrganizationInviteReceived = 0,
  OrganizationInviteAccepted = 1,
  OrganizationInviteDeclined = 2,
  OrganizationRoleChanged = 3,
  RemovedFromOrganization = 4,
  AddedToProject = 5,
  ProjectRoleChanged = 6,
  RemovedFromProject = 7,
}

export interface NotificationDto {
  id: Guid;
  recipientUserId: Guid;
  type: NotificationType;
  title: string;
  message: string;
  resourceId: Guid | null;
  isRead: boolean;
  createdAt: IsoDateString;
  readAt: IsoDateString | null;
}
