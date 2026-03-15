"use client";

import { Button } from "@/components/shared";

interface Props {
  minTransitions: number;
  onMinTransitionsChange: (value: number) => void;
}

export function FlowToolbar({ minTransitions, onMinTransitionsChange }: Props) {
  return (
    <div className="flex flex-wrap items-center gap-4 rounded-xl border border-zinc-200 bg-white px-4 py-2.5 shadow-sm">
      {/* process selector placeholder */}
      <div className="flex items-center gap-2">
        <span className="text-xs font-medium text-zinc-500">Процесс:</span>
        <Button size="sm" className="rounded-lg text-xs">
          Session (mock)
        </Button>
      </div>

      <div className="h-5 w-px bg-zinc-200" />

      {/* date range placeholder */}
      <div className="flex items-center gap-2">
        <span className="text-xs font-medium text-zinc-500">Период:</span>
        <Button size="sm" className="rounded-lg text-xs">
          Последние 7 дней
        </Button>
      </div>

      <div className="h-5 w-px bg-zinc-200" />

      {/* min transitions filter */}
      <div className="flex items-center gap-2">
        <label
          htmlFor="min-transitions"
          className="whitespace-nowrap text-xs font-medium text-zinc-500"
        >
          Мин. переходов:
        </label>
        <input
          id="min-transitions"
          type="range"
          min={0}
          max={500}
          step={10}
          value={minTransitions}
          onChange={(e) => onMinTransitionsChange(Number(e.target.value))}
          className="h-1.5 w-28 cursor-pointer accent-indigo-500"
        />
        <span className="min-w-[2rem] text-xs font-semibold text-zinc-700">
          {minTransitions}
        </span>
      </div>
    </div>
  );
}
