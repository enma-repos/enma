import { Card, CardContent, CardHeader, CardTitle, IconExternalLink, cn } from "@/components/shared";
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
      <CardHeader className="flex flex-row items-start justify-between gap-3">
        <div className="flex items-center gap-3">
          <div
            className={cn("grid h-8 w-8 place-items-center rounded-full", toneClasses[metric.tone])}
            aria-hidden="true"
          >
            <span className="text-xs font-semibold">{metric.label.slice(0, 1)}</span>
          </div>
          <CardTitle className="text-sm font-semibold">{metric.label}</CardTitle>
        </div>

        <IconExternalLink className="h-4 w-4 text-zinc-400" aria-hidden="true" />
      </CardHeader>

      <CardContent className="flex items-end justify-between gap-4">
        <div>
          <div className="text-2xl font-semibold tabular-nums">{metric.value}</div>
          <div className="mt-2 flex items-center gap-2 text-xs text-zinc-500">
            <span className="font-medium text-emerald-700">
              {metric.trend.percent}%
            </span>
            <span className="text-zinc-400">•</span>
            <span className="tabular-nums">+{metric.trend.absolute}</span>
          </div>
        </div>

        <div
          className="h-10 w-28 rounded-lg border border-dashed border-zinc-200 bg-zinc-50"
          aria-hidden="true"
        />
      </CardContent>
    </Card>
  );
}

