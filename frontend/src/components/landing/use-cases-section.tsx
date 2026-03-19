"use client";

import { useState } from "react";
import { useCases } from "@/components/landing/content";
import { SectionHeader } from "@/components/shared";
import { Factory, Landmark, Monitor, Truck } from "lucide-react";
import type { LucideProps } from "lucide-react";
import { cn } from "@/components/shared/cn";

const icons: Record<string, React.FC<LucideProps>> = {
  manufacturing: Factory,
  finance: Landmark,
  it: Monitor,
  logistics: Truck,
};

export function UseCasesSection() {
  const [active, setActive] = useState(0);
  const current = useCases[active];

  return (
    <section className="mx-auto w-full max-w-7xl px-5 py-16 sm:px-8 sm:py-20">
      <SectionHeader
        title="Built for every industry"
        centered
      />

      <div className="mt-12 grid gap-8 lg:grid-cols-[280px_1fr]">
        {/* Tabs */}
        <div className="flex gap-2 lg:flex-col lg:gap-1">
          {useCases.map((uc, i) => {
            const Icon = icons[uc.id];
            const isActive = active === i;
            return (
              <button
                key={uc.id}
                onClick={() => setActive(i)}
                className={cn(
                  "flex cursor-pointer items-center gap-3 rounded-xl px-4 py-3 text-left transition-all duration-200",
                  isActive
                    ? "border border-zinc-200 bg-white shadow-sm"
                    : "hover:bg-zinc-100",
                )}
              >
                <div
                  className={cn(
                    "flex h-9 w-9 shrink-0 items-center justify-center rounded-lg transition-colors",
                    isActive ? "bg-blue-50 text-blue-600" : "bg-zinc-100 text-zinc-400",
                  )}
                >
                  {Icon && <Icon className="h-4.5 w-4.5" />}
                </div>
                <div className="min-w-0">
                  <p
                    className={cn(
                      "text-sm font-semibold transition-colors",
                      isActive ? "text-zinc-900" : "text-zinc-500",
                    )}
                  >
                    {uc.industry}
                  </p>
                </div>
              </button>
            );
          })}
        </div>

        {/* Content */}
        <div
          key={current.id}
          className="animate-fade-in rounded-2xl border border-zinc-200 bg-white p-8 sm:p-10"
        >
          <h3 className="text-2xl font-semibold text-zinc-900">{current.title}</h3>
          <p className="mt-3 max-w-xl text-base leading-relaxed text-zinc-500">
            {current.description}
          </p>
          <div className="mt-6 flex flex-wrap gap-2">
            {current.highlights.map((h) => (
              <span
                key={h}
                className="rounded-full border border-emerald-200 bg-emerald-50 px-3 py-1 text-xs font-medium text-emerald-700"
              >
                {h}
              </span>
            ))}
          </div>
        </div>
      </div>
    </section>
  );
}
