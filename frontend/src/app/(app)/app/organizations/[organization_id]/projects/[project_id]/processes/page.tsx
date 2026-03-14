import { ProcessesScreen } from "@/components/app/processes/processes-screen";

type PageProps = {
  params: Promise<{
    organization_id: string;
    project_id: string;
  }>;
};

export default async function Page({ params }: PageProps) {
  const { organization_id, project_id } = await params;
  return <ProcessesScreen organizationSlug={organization_id} projectKey={project_id} />;
}
