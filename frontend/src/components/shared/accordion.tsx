"use client";

import { useState, type ReactNode } from "react";
import { ChevronDown } from "lucide-react";
import { cn } from "@/components/shared/cn";

type AccordionItem = {
  title: string;
  content: ReactNode;
};

type AccordionProps = {
  items: AccordionItem[];
  className?: string;
};

export function Accordion({ items, className }: AccordionProps) {
  const [openIndex, setOpenIndex] = useState<number | null>(null);

  return (
    <div className={cn("divide-y divide-zinc-200 border-y border-zinc-200", className)}>
      {items.map((item, i) => {
        const isOpen = openIndex === i;
        return (
          <div key={i}>
            <button
              onClick={() => setOpenIndex(isOpen ? null : i)}
              className="flex w-full cursor-pointer items-center justify-between py-5 text-left transition-colors hover:text-zinc-600"
            >
              <span className="text-base font-medium text-zinc-900">{item.title}</span>
              <ChevronDown
                className={cn(
                  "h-5 w-5 shrink-0 text-zinc-400 transition-transform duration-300",
                  isOpen && "rotate-180",
                )}
              />
            </button>
            <div
              className={cn(
                "grid transition-all duration-300",
                isOpen ? "grid-rows-[1fr] pb-5" : "grid-rows-[0fr]",
              )}
            >
              <div className="overflow-hidden">
                <div className="text-sm leading-relaxed text-zinc-500">{item.content}</div>
              </div>
            </div>
          </div>
        );
      })}
    </div>
  );
}
