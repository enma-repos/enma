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

const featureStyles: Record<
  string,
  { icon: React.FC<LucideProps>; iconColor: string; bgColor: string }
> = {
  "Data connectors": { icon: Cable, iconColor: "text-blue-600", bgColor: "bg-blue-50" },
  "Live process maps": { icon: Map, iconColor: "text-blue-600", bgColor: "bg-blue-50" },
  "Anomaly detection": { icon: AlertTriangle, iconColor: "text-blue-600", bgColor: "bg-blue-50" },
  "Team workspaces": { icon: Users, iconColor: "text-blue-600", bgColor: "bg-blue-50" },
  "Custom metrics": { icon: BarChart3, iconColor: "text-blue-600", bgColor: "bg-blue-50" },
  "Enterprise security": { icon: ShieldCheck, iconColor: "text-blue-600", bgColor: "bg-blue-50" },
};

export function FeaturesSection() {
  return (
    <section className="mx-auto w-full max-w-7xl px-5 py-16 sm:px-8 sm:py-20">
      <div className="text-center">
        <h2 className="text-4xl font-semibold tracking-tight text-zinc-950">
          Features
        </h2>
      </div>

      <div className="mt-12 grid gap-5 sm:grid-cols-2 lg:grid-cols-3">
        {featureItems.map((feature) => {
          const style = featureStyles[feature.title];
          const Icon = style?.icon;
          return (
            <article
              key={feature.title}
              className="rounded-2xl border border-zinc-200 bg-white p-6 transition-all duration-200 hover:shadow-md"
            >
              <div
                className={`flex h-11 w-11 items-center justify-center rounded-xl ${
                  style?.bgColor ?? "bg-zinc-100"
                }`}
              >
                {Icon && (
                  <Icon
                    className={`h-5 w-5 ${style?.iconColor ?? "text-zinc-600"}`}
                    aria-hidden="true"
                  />
                )}
              </div>
              <h3 className="mt-4 text-lg font-semibold text-zinc-900">
                {feature.title}
              </h3>
              <p className="mt-2 text-sm leading-relaxed text-zinc-500">
                {feature.description}
              </p>
            </article>
          );
        })}
      </div>
    </section>
  );
}
