import { AdminOrganizationDetail } from "@/components/app/admin/organizations/admin-organization-detail";
import type { Guid } from "@/types/admin.types";

export default async function Page({
  params,
}: {
  params: Promise<{ organization_id: string }>;
}) {
  const { organization_id } = await params;
  return <AdminOrganizationDetail organizationId={organization_id as Guid} />;
}
