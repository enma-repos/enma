import { Card } from "@/components/shared";

export function ProjectsSkeleton() {
  return (
    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
      {Array.from({ length: 6 }).map((_, index) => (
        <Card key={index} className="p-5">
          <div className="animate-pulse">
            <div className="flex items-start justify-between gap-4">
              <div className="h-12 w-12 rounded-xl bg-zinc-100" />
              <div className="h-4 w-16 rounded bg-zinc-100" />
            </div>
            <div className="mt-4 space-y-2">
              <div className="h-5 w-3/4 rounded bg-zinc-100" />
              <div className="h-4 w-full rounded bg-zinc-100" />
              <div className="h-4 w-1/3 rounded bg-zinc-100" />
            </div>
          </div>
        </Card>
      ))}
    </div>
  );
}

