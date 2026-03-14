import { AppDetailScreen } from "@/components/app/apps/app-detail-screen";

type PageProps = {
  params: Promise<{
    organization_id: string;
    project_id: string;
    app_id: string;
  }>;
};

export default async function Page({ params }: PageProps) {
  const { organization_id, project_id, app_id } = await params;
  return (
    <AppDetailScreen
      organizationSlug={organization_id}
      projectKey={project_id}
      appId={app_id}
    />
  );
}
