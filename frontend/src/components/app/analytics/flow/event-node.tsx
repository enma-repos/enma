"use client";

import { Handle, Position, type NodeProps } from "@xyflow/react";
import type { EventNodeData } from "@/lib/analytics/map-flow-to-react-flow";
import { COLORS } from "@/lib/analytics/flow-constants";

export function EventNode({ data, selected }: NodeProps) {
  const { eventName, visits, entries, exits } = data as EventNodeData;
  const isEntry = (entries as number) > 0;
  const isExit = (exits as number) > 0;

  return (
    <div
      className="rounded-xl border bg-white px-4 py-3 shadow-sm transition-shadow hover:shadow-md"
      style={{
        borderColor: selected ? COLORS.selected : COLORS.nodeBorder,
        minWidth: 180,
      }}
    >
      <Handle
        type="target"
        position={Position.Top}
        className="!h-0 !w-0 !border-0 !bg-transparent !min-h-0 !min-w-0"
      />

      <div className="flex items-center gap-2">
        {isEntry && (
          <span
            className="h-2 w-2 shrink-0 rounded-full"
            style={{ backgroundColor: COLORS.entry }}
            title="Entry point"
          />
        )}
        {isExit && (
          <span
            className="h-2 w-2 shrink-0 rounded-full"
            style={{ backgroundColor: COLORS.exit }}
            title="Exit point"
          />
        )}
        <span className="truncate text-sm font-semibold text-zinc-900">
          {eventName as string}
        </span>
      </div>

      <div className="mt-1.5 flex items-center gap-3 text-[11px] text-zinc-500">
        <span>{(visits as number).toLocaleString()} visits</span>
        {isEntry && (
          <span style={{ color: COLORS.entry }}>
            {(entries as number).toLocaleString()} in
          </span>
        )}
        {isExit && (
          <span style={{ color: COLORS.exit }}>
            {(exits as number).toLocaleString()} out
          </span>
        )}
      </div>

      <Handle
        type="source"
        position={Position.Bottom}
        className="!h-0 !w-0 !border-0 !bg-transparent !min-h-0 !min-w-0"
      />
    </div>
  );
}
