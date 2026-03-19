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
      <div className="mx-auto w-full max-w-3xl px-5 pb-28 pt-16 sm:px-8 sm:pb-36 sm:pt-20">
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
