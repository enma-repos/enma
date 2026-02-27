export type MetricTrend = {
  percent: number;
  absolute: number;
};

export type AnalyticsMetric = {
  id: string;
  label: string;
  value: string;
  trend: MetricTrend;
  tone: "red" | "purple" | "teal" | "zinc";
};

export type PopularEvent = {
  id: string;
  title: string;
  subtitle: string;
  value: string;
  deltaPercent: number;
  color:
    | "purple"
    | "teal"
    | "red"
    | "blue"
    | "pink"
    | "orange"
    | "zinc";
};

