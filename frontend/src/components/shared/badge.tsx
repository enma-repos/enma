"use client";

import type { HTMLAttributes } from "react";
import { cn } from "@/components/shared/cn";

type BadgeTone = "positive" | "neutral";

export type BadgeProps = HTMLAttributes<HTMLSpanElement> & {
  tone?: BadgeTone;
};

const toneClasses: Record<BadgeTone, string> = {
  positive: "bg-emerald-50 text-emerald-700 ring-emerald-200",
  neutral: "bg-zinc-50 text-zinc-700 ring-zinc-200",
};

export function Badge({ className, tone = "neutral", ...props }: BadgeProps) {
  return (
    <span
      className={cn(
        "inline-flex items-center rounded-full px-2 py-0.5 text-[11px] font-medium ring-1 ring-inset",
        toneClasses[tone],
        className,
      )}
      {...props}
    />
  );
}
