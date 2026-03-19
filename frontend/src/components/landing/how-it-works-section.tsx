"use client";

import { useEffect, useRef, useState } from "react";
import { howItWorksSteps } from "@/components/landing/content";
import { SectionHeader } from "@/components/shared";
import { Cable, Search, AlertTriangle, TrendingUp } from "lucide-react";
import type { LucideProps } from "lucide-react";

const stepStyles: {
  icon: React.FC<LucideProps>;
  iconColor: string;
  bgColor: string;
  borderColor: string;
}[] = [
  { icon: Cable, iconColor: "text-blue-600", bgColor: "bg-blue-50", borderColor: "border-blue-200" },
  { icon: Search, iconColor: "text-blue-600", bgColor: "bg-blue-50", borderColor: "border-blue-200" },
  { icon: AlertTriangle, iconColor: "text-blue-600", bgColor: "bg-blue-50", borderColor: "border-blue-200" },
  { icon: TrendingUp, iconColor: "text-blue-600", bgColor: "bg-blue-50", borderColor: "border-blue-200" },
];

export function HowItWorksSection() {
  const [visible, setVisible] = useState(false);
  const ref = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const el = ref.current;
    if (!el) return;
    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) {
          setVisible(true);
          observer.disconnect();
        }
      },
      { threshold: 0.2 },
    );
    observer.observe(el);
    return () => observer.disconnect();
  }, []);

  return (
    <section className="bg-white">
      <div ref={ref} className="mx-auto w-full max-w-7xl px-5 py-16 sm:px-8 sm:py-20">
        <SectionHeader
          title="From raw data to actionable insights"
          centered
        />

        <div className="relative mt-14">
          {/* Connector line */}
          <div className="absolute left-8 top-0 hidden h-full w-px bg-zinc-200 sm:block" />

          <div className="grid gap-8 sm:gap-10">
            {howItWorksSteps.map((step, i) => {
              const style = stepStyles[i];
              const Icon = style?.icon;
              return (
                <div
                  key={step.number}
                  className="relative flex gap-5 sm:gap-8"
                  style={{
                    opacity: visible ? 1 : 0,
                    transform: visible ? "translateY(0)" : "translateY(20px)",
                    transition: `all 0.5s ease-out ${i * 0.15}s`,
                  }}
                >
                  {/* Step icon circle */}
                  <div
                    className={`relative z-10 flex h-16 w-16 shrink-0 items-center justify-center rounded-2xl border ${
                      style ? `${style.bgColor} ${style.borderColor}` : "border-zinc-200 bg-zinc-50"
                    }`}
                  >
                    {Icon ? (
                      <Icon
                        className={`h-6 w-6 ${style.iconColor}`}
                        aria-hidden="true"
                      />
                    ) : (
                      <span className="text-lg font-semibold text-zinc-900">
                        {step.number}
                      </span>
                    )}
                  </div>

                  <div className="pt-2">
                    <div className="flex items-center gap-3">
                      <span className="text-xs font-semibold uppercase tracking-wider text-zinc-400">
                        Step {step.number}
                      </span>
                    </div>
                    <h3 className="mt-1 text-xl font-semibold text-zinc-900">{step.title}</h3>
                    <p className="mt-1.5 max-w-lg text-base leading-relaxed text-zinc-500">
                      {step.description}
                    </p>
                  </div>
                </div>
              );
            })}
          </div>
        </div>
      </div>
    </section>
  );
}
