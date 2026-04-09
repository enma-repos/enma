import { AdminUserDetail } from "@/components/app/admin/users/admin-user-detail";
import type { Guid } from "@/types/admin.types";

export default async function Page({
  params,
}: {
  params: Promise<{ user_id: string }>;
}) {
  const { user_id } = await params;
  return <AdminUserDetail userId={user_id as Guid} />;
}
