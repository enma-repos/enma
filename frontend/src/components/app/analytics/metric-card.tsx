import { Card, CardContent, CardHeader, CardTitle, IconExternalLink, cn } from "@/components/shared";
import { Route } from "lucide-react";
import type { AnalyticsMetric } from "@/types/analytics.types";

const toneClasses: Record<AnalyticsMetric["tone"], string> = {
  red: "bg-red-50 text-red-600",
  purple: "bg-purple-50 text-purple-600",
  teal: "bg-teal-50 text-teal-600",
  zinc: "bg-zinc-100 text-zinc-700",
};

export type MetricCardProps = {
  metric: AnalyticsMetric;
};

export function MetricCard({ metric }: MetricCardProps) {
  return (
    <Card className="h-full">
      <CardHeader className="flex flex-row items-center justify-between gap-3">
        <div className="flex items-center gap-3">
          <div
            className={cn("grid h-12 w-12 place-items-center rounded-full", toneClasses[metric.tone])}
            aria-hidden="true"
          >
            <Route size={26} />
          </div>
          <CardTitle className="text-[1.1rem] font-semibold">{metric.label}</CardTitle>
        </div>
        <div className="text-right">
          <div className="text-2xl font-semibold tabular-nums">{metric.value}</div>
          <div className="mt-1 flex items-center justify-end gap-1.5 text-xs text-zinc-500">
            <span className="font-medium text-emerald-700">
              {metric.trend.percent}%
            </span>
            <span className="text-zinc-400">•</span>
            <span className="tabular-nums">+{metric.trend.absolute}</span>
          </div>
        </div>
      </CardHeader>
    </Card>
  );
}

