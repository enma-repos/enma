import type { HTMLAttributes } from "react";
import { cn } from "@/components/shared/cn";

export type AvatarProps = HTMLAttributes<HTMLDivElement> & {
  initials?: string;
};

export function Avatar({ className, initials = "U", ...props }: AvatarProps) {
  return (
    <div
      className={cn(
        "grid h-9 w-9 place-items-center rounded-full bg-zinc-200 text-xs font-semibold text-zinc-700",
        className,
      )}
      {...props}
    >
      {initials}
    </div>
  );
}

