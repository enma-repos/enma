import { Button, IconChevronDown } from "@/components/shared";
import { FlowPlaceholder } from "@/components/app/analytics/flow-placeholder";
import { MetricsRow } from "@/components/app/analytics/metrics-row";
import { PopularEvents } from "@/components/app/analytics/popular-events";
import type { AnalyticsMetric, PopularEvent } from "@/types/analytics.types";

const metrics: AnalyticsMetric[] = [
  { id: "path-a", label: "Путь A", value: "31,300", trend: { percent: 5, absolute: 156 }, tone: "red" },
  { id: "path-b", label: "Путь B", value: "31,300", trend: { percent: 5, absolute: 156 }, tone: "purple" },
  { id: "path-c", label: "Путь C", value: "31,300", trend: { percent: 5, absolute: 156 }, tone: "teal" },
  { id: "other", label: "Прочие пути", value: "14,210", trend: { percent: 10, absolute: 142 }, tone: "zinc" },
];

const popularEvents: PopularEvent[] = Array.from({ length: 9 }).map((_, index) => ({
  id: `event-${index}`,
  title: "Title Here",
  subtitle: "Subtext",
  value: "24.500",
  deltaPercent: 12,
  color: (["purple", "teal", "red", "blue", "pink", "orange"] as const)[index % 6],
}));

export function AnalyticsSummaryScreen() {
  return (
    <div className="mx-auto w-full max-w-6xl">
      <div className="text-sm font-medium text-zinc-500">Web App</div>

      <div className="mt-2 flex flex-wrap items-center gap-3 text-sm">
        <div className="font-semibold text-zinc-900">Аналитика</div>
        <div className="text-zinc-300">|</div>
        <div className="font-semibold text-zinc-500">Сводка</div>
      </div>

      <section className="mt-7">
        <div className="flex flex-wrap items-start justify-between gap-4">
          <div>
            <h1 className="text-xl font-semibold">Ключевые метрики</h1>
            <p className="mt-1 text-sm text-zinc-500">
              Отслеживайте динамику изменения популярности разных пользовательских путей в вашем приложении
            </p>
          </div>

          <Button size="sm" className="rounded-xl">
            <span className="text-zinc-600">Select Data</span>
            <IconChevronDown className="h-4 w-4 text-zinc-500" />
          </Button>
        </div>

        <div className="mt-5">
          <MetricsRow metrics={metrics} />
        </div>
      </section>

      <section className="mt-10">
        <h2 className="text-lg font-semibold text-zinc-900">Пользовательские пути</h2>
        <p className="mt-1 text-sm text-zinc-500">
          Изучайте паттерны поведения пользователей на основе их путей по вашему приложению
        </p>

        <div className="mt-5 grid grid-cols-1 gap-6 lg:grid-cols-3">
          <div className="lg:col-span-2">
            <FlowPlaceholder />
          </div>
          <PopularEvents events={popularEvents} />
        </div>
      </section>
    </div>
  );
}

