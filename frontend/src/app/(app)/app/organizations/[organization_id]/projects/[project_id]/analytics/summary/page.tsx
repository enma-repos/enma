"use client";

import { useParams } from "next/navigation";
import { AnalyticsSummaryScreen } from "@/components/app/analytics/summary-screen";

export default function Page() {
  const params = useParams<{
    organization_id: string;
    project_id: string;
  }>();

  return (
    <AnalyticsSummaryScreen
      organizationId={params.organization_id}
      projectId={params.project_id}
    />
  );
}
