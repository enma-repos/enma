import { redirect } from "next/navigation";

export default function Page() {
  redirect("/app/organizations/some_org/projects/some_proj/analytics/summary");
}

