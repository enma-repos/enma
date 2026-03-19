import { featureItems } from "@/components/landing/content";
import {
  Cable,
  Map,
  AlertTriangle,
  Users,
  BarChart3,
  ShieldCheck,
} from "lucide-react";
import type { LucideProps } from "lucide-react";

const featureIcons: Record<string, React.FC<LucideProps>> = {
  "Data connectors": Cable,
  "Live process maps": Map,
  "Anomaly detection": AlertTriangle,
  "Team workspaces": Users,
  "Custom metrics": BarChart3,
  "Enterprise security": ShieldCheck,
};

export function FeaturesSection() {
  return (
    <section className="mx-auto w-full max-w-7xl px-5 py-12 sm:px-8 sm:py-14">
      <div className="text-center">
        <h2 className="text-4xl font-semibold tracking-tight text-zinc-950">
          Features
        </h2>
      </div>

      <div className="mt-10 grid gap-8 sm:grid-cols-2 lg:grid-cols-3">
        {featureItems.map((feature) => {
          const Icon = featureIcons[feature.title];
          return (
            <article key={feature.title} className="rounded-lg pr-2">
              <div className="flex items-start gap-3">
                {Icon ? (
                  <Icon
                    className="mt-0.5 h-6 w-6 shrink-0 text-zinc-800"
                    aria-hidden="true"
                  />
                ) : (
                  <div className="mt-0.5 h-6 w-6 shrink-0" />
                )}
                <div>
                  <h3 className="text-2xl font-semibold leading-tight text-zinc-900">
                    {feature.title}
                  </h3>
                  <p className="mt-2 text-base leading-relaxed text-zinc-500">
                    {feature.description}
                  </p>
                </div>
              </div>
            </article>
          );
        })}
      </div>
    </section>
  );
}
