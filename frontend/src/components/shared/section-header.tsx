import { cn } from "@/components/shared/cn";

type SectionHeaderProps = {
  tag?: string;
  title: string;
  description?: string;
  centered?: boolean;
  className?: string;
};

export function SectionHeader({
  tag,
  title,
  description,
  centered = false,
  className,
}: SectionHeaderProps) {
  return (
    <div className={cn(centered && "text-center", className)}>
      {tag && (
        <span className="mb-3 inline-block rounded-full border border-blue-200 bg-blue-50 px-3 py-1 text-xs font-medium text-blue-700">
          {tag}
        </span>
      )}
      <h2 className="text-3xl font-semibold tracking-tight text-zinc-950 sm:text-4xl">
        {title}
      </h2>
      {description && (
        <p className={cn("mt-3 text-lg text-zinc-500", centered && "mx-auto max-w-2xl")}>
          {description}
        </p>
      )}
    </div>
  );
}
