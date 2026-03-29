"use client";

import { useState } from "react";
import { FlowGraph } from "@/components/app/analytics/flow/flow-graph";
import { MetricsRow } from "@/components/app/analytics/metrics-row";
import { PopularEvents } from "@/components/app/analytics/popular-events";
import { ProcessSelector } from "@/components/app/analytics/process-selector";
import { DateRangePicker, DEFAULT_DATE_RANGE, type DateRange } from "@/components/app/analytics/date-range-picker";
import { useProcessDefinitions } from "@/hooks/useProcessDefinitions";
import { useSummary } from "@/hooks/useSummary";
import type { AnalyticsMetric, PopularEvent } from "@/types/analytics.types";

const popularEvents: PopularEvent[] = [
    {id: "ev-1", title: "Просмотр каталога", subtitle: "catalog_view", value: "24,500", deltaPercent: 12, icon: "eye", color: "blue"},
    {id: "ev-2", title: "Клик по товару", subtitle: "product_click", value: "18,320", deltaPercent: 8, icon: "click", color: "amber"},
    {id: "ev-3", title: "Регистрация", subtitle: "sign_up", value: "12,150", deltaPercent: 15, icon: "login", color: "emerald"},
    {id: "ev-4", title: "Добавление в корзину", subtitle: "add_to_cart", value: "9,870", deltaPercent: 6, icon: "cart", color: "violet"},
    {id: "ev-5", title: "Оплата", subtitle: "payment_complete", value: "5,430", deltaPercent: 3, icon: "card", color: "rose"},
    {id: "ev-6", title: "Поиск", subtitle: "search_query", value: "21,600", deltaPercent: 10, icon: "search", color: "cyan"},
];

function formatNumber(n: number): string {
    if (Number.isInteger(n)) {
        return n.toLocaleString("ru-RU");
    }
    return n.toLocaleString("ru-RU", { minimumFractionDigits: 1, maximumFractionDigits: 1 });
}

interface Props {
    organizationId: string;
    projectId: string;
}

export function AnalyticsSummaryScreen({organizationId, projectId}: Props) {
    const [dateRange, setDateRange] = useState<DateRange>(DEFAULT_DATE_RANGE);
    const [selectedProcessIds, setSelectedProcessIds] = useState<string[]>([]);

    const {
        organization,
        project,
        processDefinitions,
        isLoading,
    } = useProcessDefinitions(organizationId, projectId);

    const { data: summary, isLoading: summaryLoading } = useSummary(
        organization?.id,
        project?.id,
        selectedProcessIds,
        dateRange.from,
        dateRange.to,
    );

    const zeroMetric = { value: 0, trendPercent: 0, trendAbsolute: 0 };
    const s = summary ?? { totalVisits: zeroMetric, uniqueChains: zeroMetric, uniqueUsers: zeroMetric, avgStepsPerChain: zeroMetric };

    const metrics: AnalyticsMetric[] = [
        {
            id: "total-visits",
            label: "Всего событий",
            value: formatNumber(s.totalVisits.value),
            trend: { percent: s.totalVisits.trendPercent, absolute: s.totalVisits.trendAbsolute },
            tone: "red",
        },
        {
            id: "unique-chains",
            label: "Уникальные цепочки",
            value: formatNumber(s.uniqueChains.value),
            trend: { percent: s.uniqueChains.trendPercent, absolute: s.uniqueChains.trendAbsolute },
            tone: "purple",
        },
        {
            id: "unique-users",
            label: "Уникальные пользователи",
            value: formatNumber(s.uniqueUsers.value),
            trend: { percent: s.uniqueUsers.trendPercent, absolute: s.uniqueUsers.trendAbsolute },
            tone: "teal",
        },
        {
            id: "avg-steps",
            label: "Ср. шагов в пути",
            value: formatNumber(s.avgStepsPerChain.value),
            trend: { percent: s.avgStepsPerChain.trendPercent, absolute: s.avgStepsPerChain.trendAbsolute },
            tone: "zinc",
        },
    ];

    const resolvedProcessIds =
        selectedProcessIds.length > 0
            ? selectedProcessIds
            : processDefinitions[0]?.id
                ? [processDefinitions[0].id]
                : [];

    return (
        <div className="mx-auto w-full max-w-[90rem] mt-6">

            <section className="mt-2">
                <div className="flex flex-wrap items-start justify-between gap-4">
                    <div>
                        <h1 className="text-xl font-semibold">Ключевые метрики</h1>
                        <p className="mt-1 text-sm text-zinc-500">
                            Отслеживайте общую динамику активности пользователей в вашем приложении
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
                    {summaryLoading || isLoading ? (
                        <div className="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-4">
                            {[...Array(4)].map((_, i) => (
                                <div key={i} className="h-[120px] animate-pulse rounded-2xl border border-zinc-200 bg-zinc-50" />
                            ))}
                        </div>
                    ) : (
                        <MetricsRow metrics={metrics} />
                    )}
                </div>
            </section>

            <section className="mt-7">
                <h2 className="text-xl font-semibold text-zinc-900">Пользовательские пути</h2>
                <p className="mt-1 text-sm text-zinc-500">
                    Изучайте паттерны поведения пользователей на основе их путей по вашему приложению
                </p>

                <div className="mt-5 grid grid-cols-1 gap-6 lg:grid-cols-3">
                    <div className="lg:col-span-2">
                        {!isLoading && organization && project && resolvedProcessIds.length > 0 ? (
                            <FlowGraph
                                organizationId={organization.id}
                                projectId={project.id}
                                processDefinitionIds={resolvedProcessIds}
                                from={dateRange.from}
                                to={dateRange.to}
                                readonly
                            />
                        ) : (
                            <div className="grid h-[680px] place-items-center rounded-2xl border border-zinc-200 bg-zinc-50">
                                <span className="text-sm text-zinc-400">
                                    {isLoading ? "Загрузка..." : "Нет процессов для отображения"}
                                </span>
                            </div>
                        )}
                    </div>
                    <PopularEvents events={popularEvents}/>
                </div>
            </section>
        </div>
    );
}
