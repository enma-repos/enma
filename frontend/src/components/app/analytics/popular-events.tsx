import { Badge, Card, CardContent, CardDescription, CardHeader, CardTitle, IconExternalLink, cn } from "@/components/shared";
import type { PopularEvent } from "@/types/analytics.types";

const dotClasses: Record<PopularEvent["color"], string> = {
  purple: "bg-purple-500",
  teal: "bg-teal-500",
  red: "bg-red-500",
  blue: "bg-blue-500",
  pink: "bg-pink-500",
  orange: "bg-orange-500",
  zinc: "bg-zinc-400",
};

export type PopularEventsProps = {
  events: PopularEvent[];
};

export function PopularEvents({ events }: PopularEventsProps) {
  return (
    <Card className="h-full">
      <CardHeader className="flex flex-row items-start justify-between gap-3">
        <div>
          <CardTitle>Популярные события</CardTitle>
          <CardDescription>Самые посещаемые “остановки” по пути</CardDescription>
        </div>
        <IconExternalLink className="h-4 w-4 text-zinc-400" aria-hidden="true" />
      </CardHeader>

      <CardContent className="space-y-3">
        {events.map((event) => (
          <div key={event.id} className="flex items-center gap-3">
            <span
              className={cn("h-8 w-8 rounded-full", dotClasses[event.color])}
              aria-hidden="true"
            />
            <div className="min-w-0 flex-1">
              <div className="truncate text-sm font-semibold text-zinc-900">
                {event.title}
              </div>
              <div className="truncate text-xs text-zinc-500">{event.subtitle}</div>
            </div>
            <div className="flex items-center gap-2">
              <span className="text-sm font-semibold tabular-nums text-zinc-900">
                {event.value}
              </span>
              <Badge tone="positive" className="tabular-nums">
                +{event.deltaPercent}%
              </Badge>
            </div>
            <span className="h-9 w-1.5 rounded-full bg-zinc-100" aria-hidden="true" />
          </div>
        ))}
      </CardContent>
    </Card>
  );
}

