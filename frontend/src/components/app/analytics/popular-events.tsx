import {
  Badge,
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
  IconExternalLink,
  IconEye,
  IconClick,
  IconLogin,
  IconCart,
  IconCard,
  IconSearch,
  cn,
} from "@/components/shared";
import type { PopularEvent } from "@/types/analytics.types";
import type { IconProps } from "@/components/shared/icons";
import type { FC } from "react";

const dotClasses: Record<PopularEvent["color"], string> = {
  blue: "bg-blue-500",
  amber: "bg-amber-500",
  emerald: "bg-emerald-500",
  violet: "bg-violet-500",
  rose: "bg-rose-500",
  cyan: "bg-cyan-500",
};

const iconMap: Record<PopularEvent["icon"], FC<IconProps>> = {
  eye: IconEye,
  click: IconClick,
  login: IconLogin,
  cart: IconCart,
  card: IconCard,
  search: IconSearch,
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
          <CardDescription>Самые посещаемые "остановки" по пути</CardDescription>
        </div>
        <IconExternalLink className="h-4 w-4 text-zinc-400" aria-hidden="true" />
      </CardHeader>

      <CardContent className="space-y-3">
        {events.map((event) => {
          const Icon = iconMap[event.icon];
          return (
            <div key={event.id} className="flex items-center gap-3">
              <span
                className={cn(
                  "flex h-8 w-8 shrink-0 items-center justify-center rounded-full",
                  dotClasses[event.color],
                )}
                aria-hidden="true"
              >
                <Icon size={16} className="text-white" />
              </span>
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
          );
        })}
      </CardContent>
    </Card>
  );
}
