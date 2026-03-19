"use client";

import { useEffect, useRef, useState } from "react";
import { howItWorksSteps } from "@/components/landing/content";
import { SectionHeader } from "@/components/shared";

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
            {howItWorksSteps.map((step, i) => (
              <div
                key={step.number}
                className="relative flex gap-5 sm:gap-8"
                style={{
                  opacity: visible ? 1 : 0,
                  transform: visible ? "translateY(0)" : "translateY(20px)",
                  transition: `all 0.5s ease-out ${i * 0.15}s`,
                }}
              >
                {/* Step number circle */}
                <div className="relative z-10 flex h-16 w-16 shrink-0 items-center justify-center rounded-2xl border border-zinc-200 bg-zinc-50 text-lg font-semibold text-zinc-900">
                  {step.number}
                </div>

                <div className="pt-2">
                  <h3 className="text-xl font-semibold text-zinc-900">{step.title}</h3>
                  <p className="mt-1.5 max-w-lg text-base leading-relaxed text-zinc-500">
                    {step.description}
                  </p>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </section>
  );
}
