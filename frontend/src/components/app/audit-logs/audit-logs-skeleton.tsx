import { Card } from "@/components/shared";

export function AuditLogsSkeleton() {
  return (
    <Card>
      <div className="animate-pulse">
        <div className="border-b border-zinc-100 px-5 py-3 flex gap-8">
          <div className="h-4 w-20 rounded bg-zinc-100" />
          <div className="h-4 w-24 rounded bg-zinc-100" />
          <div className="h-4 w-20 rounded bg-zinc-100" />
          <div className="h-4 w-16 rounded bg-zinc-100" />
          <div className="h-4 w-28 rounded bg-zinc-100" />
        </div>
        {Array.from({ length: 6 }).map((_, index) => (
          <div key={index} className="flex items-center gap-8 border-b border-zinc-50 px-5 py-4 last:border-b-0">
            <div className="h-5 w-20 rounded bg-zinc-100" />
            <div className="h-4 w-32 rounded bg-zinc-100" />
            <div className="h-4 w-20 rounded bg-zinc-100" />
            <div className="h-4 w-24 rounded bg-zinc-100" />
            <div className="ml-auto h-4 w-28 rounded bg-zinc-100" />
          </div>
        ))}
      </div>
    </Card>
  );
}
