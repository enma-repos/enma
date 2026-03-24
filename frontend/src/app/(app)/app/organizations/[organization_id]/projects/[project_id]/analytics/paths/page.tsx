"use client";

import { useState, useRef, useEffect } from "react";
import { useParams } from "next/navigation";
import { FlowGraph } from "@/components/app/analytics/flow/flow-graph";
import { useProcessDefinitions } from "@/hooks/useProcessDefinitions";
import { Button, IconChevronDown } from "@/components/shared";
import { Layers } from "lucide-react";
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
  const [processPickerOpen, setProcessPickerOpen] = useState(false);
  const pickerRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    function handleClick(e: MouseEvent) {
      if (pickerRef.current && !pickerRef.current.contains(e.target as Node)) {
        setProcessPickerOpen(false);
      }
    }
    document.addEventListener("mousedown", handleClick);
    return () => document.removeEventListener("mousedown", handleClick);
  }, []);

  // Default to the first process if none selected
  const resolvedProcessIds =
    selectedProcessIds.length > 0
      ? selectedProcessIds
      : processDefinitions[0]?.id
        ? [processDefinitions[0].id]
        : [];

  const toggleProcess = (id: string) => {
    setSelectedProcessIds((prev) =>
      prev.includes(id) ? prev.filter((x) => x !== id) : [...prev, id],
    );
  };

  const pickerLabel =
    resolvedProcessIds.length === 0
      ? "Выберите процесс"
      : resolvedProcessIds.length === 1
        ? processDefinitions.find((p) => p.id === resolvedProcessIds[0])?.name ||
          processDefinitions.find((p) => p.id === resolvedProcessIds[0])?.key ||
          "1 процесс"
        : `${resolvedProcessIds.length} процесс(ов)`;

  return (
    <div className="mx-auto w-full max-w-[90rem]">
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
            {processDefinitions.length > 0 && (
              <div ref={pickerRef} className="relative">
                <Button
                  size="sm"
                  className="rounded-xl"
                  onClick={() => setProcessPickerOpen(!processPickerOpen)}
                >
                  <Layers size={14} className="text-zinc-500" />
                  <span className="text-zinc-600">{pickerLabel}</span>
                  <IconChevronDown className="h-4 w-4 text-zinc-500" />
                </Button>

                {processPickerOpen && (
                  <div className="absolute right-0 top-full z-50 mt-2 w-64 rounded-xl border border-zinc-200 bg-white p-2 shadow-lg">
                    {processDefinitions.map((p) => (
                      <button
                        key={p.id}
                        type="button"
                        className={`flex w-full items-center gap-2 rounded-lg px-3 py-2 text-left text-sm transition-colors hover:bg-zinc-50 ${
                          resolvedProcessIds.includes(p.id)
                            ? "bg-zinc-100 font-medium text-zinc-900"
                            : "text-zinc-600"
                        }`}
                        onClick={() => toggleProcess(p.id)}
                      >
                        <input
                          type="checkbox"
                          checked={resolvedProcessIds.includes(p.id)}
                          readOnly
                          className="h-3.5 w-3.5 rounded border-zinc-300 text-indigo-500 accent-indigo-500"
                        />
                        <div>
                          <span>{p.name || p.key}</span>
                          {p.description && (
                            <span className="mt-0.5 block text-xs text-zinc-400 truncate">
                              {p.description}
                            </span>
                          )}
                        </div>
                      </button>
                    ))}
                  </div>
                )}
              </div>
            )}
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
