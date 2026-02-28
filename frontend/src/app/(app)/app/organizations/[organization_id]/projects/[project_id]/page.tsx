import { redirect } from "next/navigation";

type PageProps = {
  params: Promise<{
    organization_id: string;
    project_id: string;
  }>;
};

export default async function Page({ params }: PageProps) {
  const { organization_id, project_id } = await params;
  redirect(`/app/organizations/${organization_id}/projects/${project_id}/analytics/summary`);
}
