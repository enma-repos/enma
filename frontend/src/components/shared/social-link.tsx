import type { ElementType } from "react";
import { cn } from "@/components/shared/cn";

type SocialLinkProps = {
  href: string;
  icon: ElementType;
  label: string;
  className?: string;
};

export function SocialLink({ href, icon: Icon, label, className }: SocialLinkProps) {
  return (
    <a
      href={href}
      target="_blank"
      rel="noopener noreferrer"
      aria-label={label}
      className={cn(
        "inline-flex h-9 w-9 items-center justify-center rounded-lg border border-zinc-200 text-zinc-500 transition-colors hover:border-zinc-300 hover:text-zinc-900",
        className,
      )}
    >
      <Icon className="h-4 w-4" />
    </a>
  );
}
