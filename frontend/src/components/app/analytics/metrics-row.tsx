import type { AnalyticsMetric } from "@/types/analytics.types";
import { MetricCard } from "@/components/app/analytics/metric-card";

export type MetricsRowProps = {
  metrics: AnalyticsMetric[];
};

export function MetricsRow({ metrics }: MetricsRowProps) {
  return (
    <div className="grid grid-cols-1 gap-4 md:grid-cols-2 xl:grid-cols-4">
      {metrics.map((metric) => (
        <MetricCard key={metric.id} metric={metric} />
      ))}
    </div>
  );
}

