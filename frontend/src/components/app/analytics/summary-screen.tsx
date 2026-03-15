"use client";

import { useState } from "react";
import { FlowGraph } from "@/components/app/analytics/flow/flow-graph";
import { MetricsRow } from "@/components/app/analytics/metrics-row";
import { PopularEvents } from "@/components/app/analytics/popular-events";
import { DateRangePicker, DEFAULT_DATE_RANGE, type DateRange } from "@/components/app/analytics/date-range-picker";
import type { AnalyticsMetric, PopularEvent } from "@/types/analytics.types";

const metrics: AnalyticsMetric[] = [
    {id: "path-a", label: "Путь A", value: "31,300", trend: {percent: 5, absolute: 156}, tone: "red"},
    {id: "path-b", label: "Путь B", value: "31,300", trend: {percent: 5, absolute: 156}, tone: "purple"},
    {id: "path-c", label: "Путь C", value: "31,300", trend: {percent: 5, absolute: 156}, tone: "teal"},
    {id: "other", label: "Прочие пути", value: "14,210", trend: {percent: 10, absolute: 142}, tone: "zinc"},
];

const popularEvents: PopularEvent[] = Array.from({length: 6}).map((_, index) => ({
    id: `event-${index}`,
    title: "Title Here",
    subtitle: "Subtext",
    value: "24.500",
    deltaPercent: 12,
    color: (["purple", "teal", "red", "blue", "pink", "orange"] as const)[index % 6],
}));

interface Props {
    organizationId: string;
    projectId: string;
}

export function AnalyticsSummaryScreen({organizationId, projectId}: Props) {
    const [dateRange, setDateRange] = useState<DateRange>(DEFAULT_DATE_RANGE);

    return (
        <div className="mx-auto w-full max-w-[90rem]">

            <section className="mt-2">
                <div className="flex flex-wrap items-start justify-between gap-4">
                    <div>
                        <h1 className="text-xl font-semibold">Ключевые метрики</h1>
                        <p className="mt-1 text-sm text-zinc-500">
                            Отслеживайте динамику изменения популярности разных пользовательских путей в вашем
                            приложении
                        </p>
                    </div>

                    <DateRangePicker value={dateRange} onChange={setDateRange} />
                </div>
                <div className="mt-5">
                    <MetricsRow metrics={metrics}/>
                </div>
            </section>

            <section className="mt-7">
                <h2 className="text-xl font-semibold text-zinc-900">Пользовательские пути</h2>
                <p className="mt-1 text-sm text-zinc-500">
                    Изучайте паттерны поведения пользователей на основе их путей по вашему приложению
                </p>

                <div className="mt-5 grid grid-cols-1 gap-6 lg:grid-cols-3">
                    <div className="lg:col-span-2">
                        <FlowGraph
                            organizationId={organizationId}
                            projectId={projectId}
                            readonly
                        />
                    </div>
                    <PopularEvents events={popularEvents}/>
                </div>
            </section>
        </div>
    );
}
