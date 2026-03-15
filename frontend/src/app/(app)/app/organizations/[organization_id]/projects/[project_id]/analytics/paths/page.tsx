"use client";

import { useParams } from "next/navigation";
import { FlowGraph } from "@/components/app/analytics/flow/flow-graph";

export default function PathsPage() {
  const params = useParams<{
    organization_id: string;
    project_id: string;
  }>();

  return (
    <div className="mx-auto w-full max-w-[90rem]">

      <section>
        <h1 className="text-xl font-semibold">Пользовательские пути</h1>
        <p className="mt-1 text-sm text-zinc-500">
          Изучайте паттерны поведения пользователей на основе их путей по вашему
          приложению
        </p>

        <div className="mt-5" >
          <FlowGraph
            organizationId={params.organization_id}
            projectId={params.project_id}
          />
        </div>
      </section>
    </div>
  );
}
