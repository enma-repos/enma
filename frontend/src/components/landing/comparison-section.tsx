import { comparisonRows } from "@/components/landing/content";
import { SectionHeader } from "@/components/shared";
import { X, Check } from "lucide-react";

export function ComparisonSection() {
  return (
    <section className="mx-auto w-full max-w-7xl px-5 py-16 sm:px-8 sm:py-20">
      <SectionHeader
        title="Before & after enma"
        centered
      />

      <div className="mt-12 overflow-hidden rounded-2xl border border-zinc-200">
        {/* Header row */}
        <div className="grid grid-cols-[1fr_1fr_1fr] border-b border-zinc-200 bg-zinc-50 text-sm font-semibold text-zinc-500">
          <div className="px-6 py-4" />
          <div className="flex items-center gap-2 border-l border-zinc-200 px-6 py-4">
            <X className="h-4 w-4 text-zinc-400" />
            Without enma
          </div>
          <div className="flex items-center gap-2 border-l border-zinc-200 bg-blue-50/50 px-6 py-4 text-blue-700">
            <Check className="h-4 w-4 text-blue-500" />
            With enma
          </div>
        </div>

        {/* Data rows */}
        {comparisonRows.map((row, i) => (
          <div
            key={row.aspect}
            className={`grid grid-cols-[1fr_1fr_1fr] text-sm ${
              i < comparisonRows.length - 1 ? "border-b border-zinc-100" : ""
            }`}
          >
            <div className="px-6 py-4 font-medium text-zinc-900">{row.aspect}</div>
            <div className="border-l border-zinc-100 px-6 py-4 text-zinc-500">
              {row.without}
            </div>
            <div className="border-l border-zinc-100 bg-blue-50/30 px-6 py-4 font-medium text-zinc-800">
              {row.with}
            </div>
          </div>
        ))}
      </div>
    </section>
  );
}
