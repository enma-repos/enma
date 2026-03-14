import { Card } from "@/components/shared";

export function ProcessesSkeleton() {
  return (
    <Card>
      <div className="animate-pulse">
        <div className="border-b border-zinc-100 px-5 py-3">
          <div className="h-4 w-1/4 rounded bg-zinc-100" />
        </div>
        {Array.from({ length: 4 }).map((_, index) => (
          <div key={index} className="flex items-center gap-4 border-b border-zinc-50 px-5 py-4 last:border-b-0">
            <div className="h-4 w-1/4 rounded bg-zinc-100" />
            <div className="h-4 w-1/6 rounded bg-zinc-100" />
            <div className="h-4 w-1/6 rounded bg-zinc-100" />
            <div className="ml-auto h-4 w-16 rounded bg-zinc-100" />
          </div>
        ))}
      </div>
    </Card>
  );
}
