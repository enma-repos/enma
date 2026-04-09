import { AdminProjectDetail } from "@/components/app/admin/projects/admin-project-detail";
import type { Guid } from "@/types/admin.types";

export default async function Page({
  params,
}: {
  params: Promise<{ project_id: string }>;
}) {
  const { project_id } = await params;
  return <AdminProjectDetail projectId={project_id as Guid} />;
}
