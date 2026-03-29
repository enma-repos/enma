"use client";

import { useState } from "react";
import { useParams } from "next/navigation";
import { FlowGraph } from "@/components/app/analytics/flow/flow-graph";
import { useProcessDefinitions } from "@/hooks/useProcessDefinitions";
import { ProcessSelector } from "@/components/app/analytics/process-selector";
import {
  DateRangePicker,
  DEFAULT_DATE_RANGE,
  type DateRange,
} from "@/components/app/analytics/date-range-picker";

export default function PathsPage() {
  const params = useParams<{
    organization_id: string;
    project_id: string;
  }>();

  const {
    organization,
    project,
    processDefinitions,
    isLoading,
  } = useProcessDefinitions(params.organization_id, params.project_id);

  const [selectedProcessIds, setSelectedProcessIds] = useState<string[]>([]);
  const [dateRange, setDateRange] = useState<DateRange>(DEFAULT_DATE_RANGE);

  const resolvedProcessIds =
    selectedProcessIds.length > 0
      ? selectedProcessIds
      : processDefinitions[0]?.id
        ? [processDefinitions[0].id]
        : [];

  return (
    <div className="mx-auto w-full max-w-[90rem] mt-6">
      <section>
        <div className="flex flex-wrap items-start justify-between gap-4">
          <div>
            <h1 className="text-xl font-semibold">Пользовательские пути</h1>
            <p className="mt-1 text-sm text-zinc-500">
              Изучайте паттерны поведения пользователей на основе их путей по
              вашему приложению
            </p>
          </div>

          <div className="flex items-center gap-3">
            <ProcessSelector
              processDefinitions={processDefinitions}
              selectedIds={selectedProcessIds}
              onChange={setSelectedProcessIds}
            />
            <DateRangePicker value={dateRange} onChange={setDateRange} />
          </div>
        </div>

        <div className="mt-5">
          {isLoading ? (
            <div className="grid h-[900px] place-items-center rounded-2xl border border-zinc-200 bg-zinc-50">
              <div className="flex flex-col items-center gap-2">
                <div className="h-6 w-6 animate-spin rounded-full border-2 border-zinc-300 border-t-zinc-600" />
                <span className="text-sm text-zinc-400">Загрузка...</span>
              </div>
            </div>
          ) : !organization || !project ? (
            <div className="rounded-2xl border border-red-200 bg-red-50 p-5 text-sm text-red-700">
              Не удалось загрузить организацию или проект
            </div>
          ) : processDefinitions.length === 0 ? (
            <div className="grid h-[400px] place-items-center rounded-2xl border border-zinc-200 bg-zinc-50">
              <span className="text-sm text-zinc-400">
                Нет определённых процессов. Создайте процесс, чтобы увидеть граф.
              </span>
            </div>
          ) : (
            <FlowGraph
              organizationId={organization.id}
              projectId={project.id}
              processDefinitionIds={resolvedProcessIds}
              from={dateRange.from}
              to={dateRange.to}
            />
          )}
        </div>
      </section>
    </div>
  );
}
