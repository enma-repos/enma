import {redirect} from "next/navigation";

type PageProps = {
  params: Promise<{
    organization_id: string;
  }>;
};

export default async function Page({params}: PageProps) {
  const {organization_id} = await params;
  redirect(`${organization_id}/projects`);
}
