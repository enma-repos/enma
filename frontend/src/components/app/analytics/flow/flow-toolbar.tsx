"use client";

interface Props {
  minTransitions: number;
  onMinTransitionsChange: (value: number) => void;
  entryEventFilter: string | null;
  onClearEntryEventFilter: () => void;
}

export function FlowToolbar({
  minTransitions,
  onMinTransitionsChange,
  entryEventFilter,
  onClearEntryEventFilter,
}: Props) {
  return (
    <div className="flex flex-wrap items-center gap-4 rounded-xl border border-zinc-200 bg-white px-4 py-2.5">
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

      {entryEventFilter && (
        <div className="flex items-center gap-1.5 rounded-full bg-indigo-50 px-3 py-1">
          <span className="text-xs text-indigo-700">
            Пути от: <span className="font-semibold">{entryEventFilter}</span>
          </span>
          <button
            type="button"
            onClick={onClearEntryEventFilter}
            className="ml-0.5 rounded-full p-0.5 text-indigo-400 hover:bg-indigo-100 hover:text-indigo-600"
          >
            <svg width="12" height="12" viewBox="0 0 12 12" fill="none">
              <path
                d="M3 3l6 6M9 3l-6 6"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
              />
            </svg>
          </button>
        </div>
      )}
    </div>
  );
}
