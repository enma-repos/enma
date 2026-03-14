import { featureItems } from "@/components/landing/content";
import { Info } from "lucide-react";

export function FeaturesSection() {
  return (
    <section className="mx-auto w-full max-w-7xl px-5 py-12 sm:px-8 sm:py-14">
      <div className="max-w-xl">
        <h2 className="text-4xl font-semibold tracking-tight text-zinc-950">
          Features
        </h2>
        <p className="mt-2 text-lg text-zinc-500">Subheading</p>
      </div>

      <div className="mt-10 grid gap-8 sm:grid-cols-2 lg:grid-cols-3">
        {featureItems.map((feature) => (
          <article key={feature.title} className="rounded-lg pr-2">
            <div className="flex items-start gap-3">
              <Info
                className="mt-0.5 h-6 w-6 shrink-0 text-zinc-800"
                aria-hidden="true"
              />
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
        ))}
      </div>
    </section>
  );
}
