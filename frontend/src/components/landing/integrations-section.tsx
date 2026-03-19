"use client";

import { useEffect, useRef, useState } from "react";
import { integrations } from "@/components/landing/content";
import { SectionHeader } from "@/components/shared";

export function IntegrationsSection() {
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
    <section className="border-t border-zinc-200 bg-white">
      <div ref={ref} className="mx-auto w-full max-w-7xl px-5 py-16 sm:px-8 sm:py-20">
        <SectionHeader
          tag="Integrations"
          title="Connects to your stack"
          description="Pre-built connectors for popular enterprise systems. Custom integrations via SDK and REST API."
          centered
        />

        <div className="mt-12 grid grid-cols-2 gap-3 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-6">
          {integrations.map((item, i) => (
            <div
              key={item.name}
              className="flex flex-col items-center gap-2 rounded-xl border border-zinc-100 bg-zinc-50/50 px-4 py-6 transition-all duration-200 hover:border-zinc-200 hover:bg-white hover:shadow-sm"
              style={{
                opacity: visible ? 1 : 0,
                transform: visible ? "translateY(0)" : "translateY(12px)",
                transition: `all 0.4s ease-out ${i * 0.05}s`,
              }}
            >
              <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-zinc-100 text-sm font-bold text-zinc-600">
                {item.name.slice(0, 2).toUpperCase()}
              </div>
              <span className="text-sm font-medium text-zinc-900">{item.name}</span>
              <span className="text-[11px] text-zinc-400">{item.category}</span>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}
