"use client";

import type { EventDetailDto } from "@/types/analytics.types";
import { IconX } from "@/components/shared";
import { COLORS } from "@/lib/analytics/flow-constants";

interface Props {
  detail: EventDetailDto | undefined;
  isLoading: boolean;
  onClose: () => void;
}

export function NodeDetailPanel({ detail, isLoading, onClose }: Props) {
  return (
    <div className="flex h-full w-80 shrink-0 flex-col overflow-hidden rounded-2xl border border-zinc-200 bg-white shadow-sm">
      {/* header */}
      <div className="flex items-center justify-between border-b border-zinc-100 px-4 py-3">
        <h3 className="truncate text-sm font-semibold text-zinc-900">
          {detail?.eventName ?? "..."}
        </h3>
        <button
          type="button"
          onClick={onClose}
          className="rounded-lg p-1 text-zinc-400 hover:bg-zinc-100 hover:text-zinc-600"
        >
          <IconX size={16} />
        </button>
      </div>

      {/* body */}
      <div className="flex-1 overflow-y-auto px-4 py-3">
        {isLoading && (
          <div className="space-y-3">
            {[1, 2, 3, 4].map((i) => (
              <div key={i} className="h-4 animate-pulse rounded bg-zinc-100" />
            ))}
          </div>
        )}

        {!isLoading && detail && (
          <div className="space-y-5">
            {/* metrics */}
            <div className="grid grid-cols-2 gap-3">
              <Stat label="Visits" value={detail.totalVisits} />
              <Stat label="Unique chains" value={detail.totalUniqueChains} />
              <Stat
                label="Entries"
                value={detail.totalEntries}
                color={COLORS.entry}
              />
              <Stat
                label="Exits"
                value={detail.totalExits}
                color={COLORS.exit}
              />
            </div>

            {/* incoming */}
            {detail.incomingEdges.length > 0 && (
              <section>
                <h4 className="mb-2 text-xs font-medium uppercase tracking-wide text-zinc-400">
                  Входящие переходы
                </h4>
                <ul className="space-y-1.5">
                  {detail.incomingEdges.map((e) => (
                    <li
                      key={e.fromEvent}
                      className="flex items-center justify-between text-sm"
                    >
                      <span className="truncate text-zinc-700">
                        {e.fromEvent}
                      </span>
                      <span className="shrink-0 font-medium text-zinc-900">
                        {e.transitions.toLocaleString()}
                      </span>
                    </li>
                  ))}
                </ul>
              </section>
            )}

            {/* outgoing */}
            {detail.outgoingEdges.length > 0 && (
              <section>
                <h4 className="mb-2 text-xs font-medium uppercase tracking-wide text-zinc-400">
                  Исходящие переходы
                </h4>
                <ul className="space-y-1.5">
                  {detail.outgoingEdges.map((e) => (
                    <li
                      key={e.toEvent}
                      className="flex items-center justify-between text-sm"
                    >
                      <span className="truncate text-zinc-700">
                        {e.toEvent}
                      </span>
                      <span className="shrink-0 font-medium text-zinc-900">
                        {e.transitions.toLocaleString()}
                      </span>
                    </li>
                  ))}
                </ul>
              </section>
            )}
          </div>
        )}
      </div>
    </div>
  );
}

function Stat({
  label,
  value,
  color,
}: {
  label: string;
  value: number;
  color?: string;
}) {
  return (
    <div className="rounded-lg bg-zinc-50 px-3 py-2">
      <p className="text-[11px] text-zinc-500">{label}</p>
      <p
        className="text-base font-semibold"
        style={{ color: color ?? "#18181b" }}
      >
        {value.toLocaleString()}
      </p>
    </div>
  );
}
