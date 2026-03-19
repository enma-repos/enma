import { testimonials } from "@/components/landing/content";
import { SectionHeader } from "@/components/shared";
import { Quote } from "lucide-react";

export function TestimonialsSection() {
  return (
    <section className="mx-auto w-full max-w-7xl px-5 py-16 sm:px-8 sm:py-20">
      <SectionHeader
        tag="Testimonials"
        title="Trusted by process teams"
        description="Hear from the teams that use enma to optimize their operations every day."
        centered
      />

      <div className="mt-12 grid gap-6 md:grid-cols-3">
        {testimonials.map((t) => (
          <article
            key={t.name}
            className="flex flex-col rounded-2xl border border-zinc-200 bg-white p-8 transition-shadow hover:shadow-sm"
          >
            <Quote className="mb-4 h-6 w-6 text-zinc-300" aria-hidden="true" />
            <blockquote className="flex-1 text-base leading-relaxed text-zinc-600">
              &ldquo;{t.quote}&rdquo;
            </blockquote>
            <div className="mt-6 border-t border-zinc-100 pt-4">
              <p className="text-sm font-semibold text-zinc-900">{t.name}</p>
              <p className="text-xs text-zinc-500">
                {t.role}, {t.company}
              </p>
            </div>
          </article>
        ))}
      </div>
    </section>
  );
}
