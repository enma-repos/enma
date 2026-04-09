import type {
  Guid,
  IsoDateString,
  OrganizationRole,
  ProjectRole,
} from "./admin.types";

export interface SuperOverviewStatsDto {
  totalUsers: number;
  totalOrganizations: number;
  totalProjects: number;
  totalApiKeys: number;
  recentAuditLogsLast24Hours: number;
}

export interface SuperListQuery {
  page?: number;
  pageSize?: number;
  search?: string;
  includeDeleted?: boolean;
}

export interface SuperProjectsQuery extends SuperListQuery {
  organizationId?: Guid;
}

export interface SuperAuditLogsQuery {
  page?: number;
  pageSize?: number;
  search?: string;
  from?: IsoDateString;
  to?: IsoDateString;
  action?: string;
  resourceType?: string;
  actorUserId?: Guid;
  organizationId?: Guid;
  projectId?: Guid;
}

// ---------- Users ----------

export interface SuperUserListItemDto {
  id: Guid;
  email: string;
  displayName: string;
  avatarUrl: string | null;
  organizationCount: number;
  projectCount: number;
  createdAt: IsoDateString;
  deletedAt: IsoDateString | null;
}

export interface SuperUserOrganizationMembershipDto {
  organizationId: Guid;
  organizationName: string;
  organizationSlug: string;
  role: OrganizationRole;
  joinedAt: IsoDateString;
}

export interface SuperUserProjectMembershipDto {
  projectId: Guid;
  projectName: string;
  projectKey: string;
  organizationId: Guid;
  organizationName: string;
  role: ProjectRole;
  joinedAt: IsoDateString;
}

export interface SuperUserDetailsDto {
  id: Guid;
  email: string;
  displayName: string;
  avatarUrl: string | null;
  locale: string | null;
  timezone: string | null;
  createdAt: IsoDateString;
  updatedAt: IsoDateString;
  deletedAt: IsoDateString | null;
  organizations: SuperUserOrganizationMembershipDto[];
  projects: SuperUserProjectMembershipDto[];
}

// ---------- Organizations ----------

export interface SuperOrganizationListItemDto {
  id: Guid;
  name: string;
  slug: string;
  description: string | null;
  ownerUserId: Guid;
  ownerEmail: string | null;
  ownerDisplayName: string | null;
  memberCount: number;
  projectCount: number;
  createdAt: IsoDateString;
  deletedAt: IsoDateString | null;
}

export interface SuperOrganizationMemberDto {
  userId: Guid;
  email: string;
  displayName: string;
  avatarUrl: string | null;
  role: OrganizationRole;
  joinedAt: IsoDateString;
}

export interface SuperOrganizationProjectDto {
  id: Guid;
  name: string;
  key: string;
  memberCount: number;
  createdAt: IsoDateString;
  deletedAt: IsoDateString | null;
  archivedAt: IsoDateString | null;
}

export interface SuperOrganizationDetailsDto {
  id: Guid;
  name: string;
  slug: string;
  description: string | null;
  ownerUserId: Guid;
  ownerEmail: string | null;
  ownerDisplayName: string | null;
  createdAt: IsoDateString;
  updatedAt: IsoDateString;
  deletedAt: IsoDateString | null;
  members: SuperOrganizationMemberDto[];
  projects: SuperOrganizationProjectDto[];
}

// ---------- Projects ----------

export interface SuperProjectListItemDto {
  id: Guid;
  name: string;
  key: string;
  description: string | null;
  organizationId: Guid;
  organizationName: string;
  organizationSlug: string;
  memberCount: number;
  createdAt: IsoDateString;
  deletedAt: IsoDateString | null;
  archivedAt: IsoDateString | null;
}

export interface SuperProjectMemberDto {
  userId: Guid;
  email: string;
  displayName: string;
  avatarUrl: string | null;
  role: ProjectRole;
  joinedAt: IsoDateString;
}

export interface SuperProjectDetailsDto {
  id: Guid;
  name: string;
  key: string;
  description: string | null;
  organizationId: Guid;
  organizationName: string;
  organizationSlug: string;
  createdByUserId: Guid;
  createdAt: IsoDateString;
  updatedAt: IsoDateString;
  deletedAt: IsoDateString | null;
  archivedAt: IsoDateString | null;
  sdkClientCount: number;
  processDefinitionCount: number;
  eventDefinitionCount: number;
  members: SuperProjectMemberDto[];
}
