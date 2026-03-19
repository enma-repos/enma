"use client";

import { metrics } from "@/components/landing/content";
import { AnimatedCounter } from "@/components/shared";

export function MetricsSection() {
  return (
    <section className="border-t border-zinc-200 bg-zinc-950">
      <div className="mx-auto w-full max-w-7xl px-5 py-16 sm:px-8 sm:py-20">
        <div className="grid gap-8 sm:grid-cols-2 lg:grid-cols-4">
          {metrics.map((m) => (
            <div key={m.label} className="text-center">
              <div className="text-5xl font-bold text-white">
                <AnimatedCounter
                  target={m.value}
                  suffix={m.suffix}
                  duration={2000}
                />
              </div>
              <p className="mt-2 text-sm text-zinc-400">{m.label}</p>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}
