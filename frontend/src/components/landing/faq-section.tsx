"use client";

import { faqItems } from "@/components/landing/content";
import { Accordion, SectionHeader } from "@/components/shared";

export function FaqSection() {
  const items = faqItems.map((f) => ({
    title: f.question,
    content: f.answer,
  }));

  return (
    <section className="bg-white">
      <div className="mx-auto w-full max-w-3xl px-5 py-16 sm:px-8 sm:py-20">
        <SectionHeader
          title="Frequently asked questions"
          centered
        />

        <div className="mt-12">
          <Accordion items={items} />
        </div>
      </div>
    </section>
  );
}
