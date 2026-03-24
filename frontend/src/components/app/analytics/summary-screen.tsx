"use client";

import { useState } from "react";
import { FlowGraph } from "@/components/app/analytics/flow/flow-graph";
import { MetricsRow } from "@/components/app/analytics/metrics-row";
import { PopularEvents } from "@/components/app/analytics/popular-events";
import { DateRangePicker, DEFAULT_DATE_RANGE, type DateRange } from "@/components/app/analytics/date-range-picker";
import { useProcessDefinitions } from "@/hooks/useProcessDefinitions";
import type { AnalyticsMetric, PopularEvent } from "@/types/analytics.types";

const metrics: AnalyticsMetric[] = [
    {id: "path-a", label: "Путь A", value: "31,300", trend: {percent: 5, absolute: 156}, tone: "red"},
    {id: "path-b", label: "Путь B", value: "31,300", trend: {percent: 5, absolute: 156}, tone: "purple"},
    {id: "path-c", label: "Путь C", value: "31,300", trend: {percent: 5, absolute: 156}, tone: "teal"},
    {id: "other", label: "Прочие пути", value: "14,210", trend: {percent: 10, absolute: 142}, tone: "zinc"},
];

const popularEvents: PopularEvent[] = [
    {id: "ev-1", title: "Просмотр каталога", subtitle: "catalog_view", value: "24,500", deltaPercent: 12, icon: "eye", color: "blue"},
    {id: "ev-2", title: "Клик по товару", subtitle: "product_click", value: "18,320", deltaPercent: 8, icon: "click", color: "amber"},
    {id: "ev-3", title: "Регистрация", subtitle: "sign_up", value: "12,150", deltaPercent: 15, icon: "login", color: "emerald"},
    {id: "ev-4", title: "Добавление в корзину", subtitle: "add_to_cart", value: "9,870", deltaPercent: 6, icon: "cart", color: "violet"},
    {id: "ev-5", title: "Оплата", subtitle: "payment_complete", value: "5,430", deltaPercent: 3, icon: "card", color: "rose"},
    {id: "ev-6", title: "Поиск", subtitle: "search_query", value: "21,600", deltaPercent: 10, icon: "search", color: "cyan"},
];

interface Props {
    organizationId: string;
    projectId: string;
}

export function AnalyticsSummaryScreen({organizationId, projectId}: Props) {
    const [dateRange, setDateRange] = useState<DateRange>(DEFAULT_DATE_RANGE);

    const {
        organization,
        project,
        processDefinitions,
        isLoading,
    } = useProcessDefinitions(organizationId, projectId);

    const firstProcessId = processDefinitions[0]?.id ?? null;

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
                        {!isLoading && organization && project && firstProcessId ? (
                            <FlowGraph
                                organizationId={organization.id}
                                projectId={project.id}
                                processDefinitionIds={[firstProcessId]}
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
