"use client";

import { DateRangePicker, DEFAULT_DATE_RANGE } from "@/components/app/analytics/date-range-picker";
import type { DateRange } from "@/components/app/analytics/date-range-picker";

export type AuditLogsToolbarProps = {
  dateRange: DateRange;
  onDateRangeChange: (range: DateRange) => void;
  resourceType: string;
  onResourceTypeChange: (value: string) => void;
  resourceTypes: string[];
};

export function AuditLogsToolbar({
  dateRange,
  onDateRangeChange,
  resourceType,
  onResourceTypeChange,
  resourceTypes,
}: AuditLogsToolbarProps) {
  return (
    <div className="flex flex-wrap items-center justify-between gap-3">
      <h1 className="text-xl font-semibold text-zinc-900">Журнал действий</h1>
      <div className="flex items-center gap-2">
        <select
          value={resourceType}
          onChange={(e) => onResourceTypeChange(e.target.value)}
          className="h-9 rounded-xl border border-zinc-200 bg-white px-3 text-sm text-zinc-700 focus:outline-none focus:ring-2 focus:ring-zinc-900/10"
        >
          <option value="">Все ресурсы</option>
          {resourceTypes.map((rt) => (
            <option key={rt} value={rt}>
              {rt}
            </option>
          ))}
        </select>
        <DateRangePicker value={dateRange} onChange={onDateRangeChange} />
      </div>
    </div>
  );
}
