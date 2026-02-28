import { ProjectsScreen } from "@/components/app/projects/projects-screen";

type PageProps = {
  params: Promise<{
    organization_id: string;
  }>;
};

export default async function Page({ params }: PageProps) {
  const { organization_id } = await params;
  return <ProjectsScreen organizationSlug={organization_id} />;
}
