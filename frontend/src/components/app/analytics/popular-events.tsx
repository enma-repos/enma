"use client";

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
  cn,
} from "@/components/shared";
import { Zap } from "lucide-react";
import type { TopEventItemDto } from "@/types/analytics.types";

const palette = [
  { bg: "bg-blue-50", fg: "text-blue-600" },
  { bg: "bg-amber-50", fg: "text-amber-600" },
  { bg: "bg-emerald-50", fg: "text-emerald-600" },
  { bg: "bg-violet-50", fg: "text-violet-600" },
  { bg: "bg-rose-50", fg: "text-rose-600" },
  { bg: "bg-cyan-50", fg: "text-cyan-600" },
];

function formatNumber(n: number): string {
  return n.toLocaleString("ru-RU");
}

export type PopularEventsProps = {
  events: TopEventItemDto[];
  isLoading?: boolean;
};

export function PopularEvents({ events, isLoading }: PopularEventsProps) {
  return (
    <Card className="h-full">
      <CardHeader className="flex flex-row items-start justify-between gap-3">
        <div>
          <CardTitle>Популярные события</CardTitle>
          <CardDescription>Самые посещаемые &quot;остановки&quot; по пути</CardDescription>
        </div>
      </CardHeader>

      <hr className="border-zinc-200" />
      <CardContent className="space-y-3 pt-4">
        {isLoading ? (
          [...Array(6)].map((_, i) => (
            <div key={i} className="flex items-center gap-3">
              <div className="h-8 w-8 shrink-0 animate-pulse rounded-full bg-zinc-100" />
              <div className="flex-1 space-y-1.5">
                <div className="h-3.5 w-24 animate-pulse rounded bg-zinc-100" />
                <div className="h-3 w-16 animate-pulse rounded bg-zinc-100" />
              </div>
              <div className="h-4 w-12 animate-pulse rounded bg-zinc-100" />
            </div>
          ))
        ) : events.length === 0 ? (
          <p className="py-4 text-center text-sm text-zinc-400">Нет данных</p>
        ) : (
          events.map((event, index) => (
            <div key={event.eventName} className="flex items-center gap-3">
              <span
                className={cn(
                  "flex h-8 w-8 shrink-0 items-center justify-center rounded-full",
                  palette[index % palette.length].bg,
                )}
                aria-hidden="true"
              >
                <Zap size={14} className={palette[index % palette.length].fg} />
              </span>
              <div className="min-w-0 flex-1">
                <div className="truncate text-sm font-semibold text-zinc-900">
                  {event.eventName}
                </div>
                <div className="truncate text-xs text-zinc-500">
                  {formatNumber(event.visits)} визитов · {formatNumber(event.uniqueChains)} уник. путей
                </div>
              </div>
            </div>
          ))
        )}
      </CardContent>
    </Card>
  );
}
