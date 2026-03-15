"use client";

import { useState, useRef, useEffect } from "react";
import { Button, IconChevronDown } from "@/components/shared";
import { Calendar } from "lucide-react";

export interface DateRange {
  from: string;
  to: string;
  label: string;
}

const PRESETS: DateRange[] = [
  {
    label: "Сегодня",
    from: todayISO(),
    to: endOfDayISO(),
  },
  {
    label: "Последние 7 дней",
    from: daysAgoISO(7),
    to: endOfDayISO(),
  },
  {
    label: "Последние 14 дней",
    from: daysAgoISO(14),
    to: endOfDayISO(),
  },
  {
    label: "Последние 30 дней",
    from: daysAgoISO(30),
    to: endOfDayISO(),
  },
  {
    label: "Последние 90 дней",
    from: daysAgoISO(90),
    to: endOfDayISO(),
  },
];

function todayISO() {
  const d = new Date();
  d.setHours(0, 0, 0, 0);
  return d.toISOString();
}

function endOfDayISO() {
  const d = new Date();
  d.setHours(23, 59, 59, 999);
  return d.toISOString();
}

function daysAgoISO(n: number) {
  const d = new Date();
  d.setDate(d.getDate() - n);
  d.setHours(0, 0, 0, 0);
  return d.toISOString();
}

function formatDate(iso: string) {
  return new Date(iso).toLocaleDateString("ru-RU", {
    day: "numeric",
    month: "short",
  });
}

interface Props {
  value: DateRange;
  onChange: (range: DateRange) => void;
}

export function DateRangePicker({ value, onChange }: Props) {
  const [open, setOpen] = useState(false);
  const [customMode, setCustomMode] = useState(false);
  const [customFrom, setCustomFrom] = useState("");
  const [customTo, setCustomTo] = useState("");
  const ref = useRef<HTMLDivElement>(null);

  useEffect(() => {
    function handleClick(e: MouseEvent) {
      if (ref.current && !ref.current.contains(e.target as Node)) {
        setOpen(false);
        setCustomMode(false);
      }
    }
    document.addEventListener("mousedown", handleClick);
    return () => document.removeEventListener("mousedown", handleClick);
  }, []);

  function handlePreset(preset: DateRange) {
    onChange(preset);
    setOpen(false);
    setCustomMode(false);
  }

  function handleCustomApply() {
    if (!customFrom || !customTo) return;
    const from = new Date(customFrom);
    from.setHours(0, 0, 0, 0);
    const to = new Date(customTo);
    to.setHours(23, 59, 59, 999);
    onChange({
      label: `${formatDate(from.toISOString())} – ${formatDate(to.toISOString())}`,
      from: from.toISOString(),
      to: to.toISOString(),
    });
    setOpen(false);
    setCustomMode(false);
  }

  return (
    <div ref={ref} className="relative">
      <Button
        size="sm"
        className="rounded-xl"
        onClick={() => setOpen(!open)}
      >
        <Calendar size={14} className="text-zinc-500" />
        <span className="text-zinc-600">{value.label}</span>
        <IconChevronDown className="h-4 w-4 text-zinc-500" />
      </Button>

      {open && (
        <div className="absolute right-0 top-full z-50 mt-2 w-64 rounded-xl border border-zinc-200 bg-white p-2 shadow-lg">
          {!customMode ? (
            <>
              {PRESETS.map((preset) => (
                <button
                  key={preset.label}
                  type="button"
                  className={`w-full rounded-lg px-3 py-2 text-left text-sm transition-colors hover:bg-zinc-50 ${
                    value.label === preset.label
                      ? "bg-zinc-100 font-medium text-zinc-900"
                      : "text-zinc-600"
                  }`}
                  onClick={() => handlePreset(preset)}
                >
                  {preset.label}
                </button>
              ))}
              <div className="my-1 border-t border-zinc-100" />
              <button
                type="button"
                className="w-full rounded-lg px-3 py-2 text-left text-sm text-zinc-600 transition-colors hover:bg-zinc-50"
                onClick={() => setCustomMode(true)}
              >
                Свой диапазон...
              </button>
            </>
          ) : (
            <div className="space-y-3 p-1">
              <div>
                <label className="mb-1 block text-xs text-zinc-500">От</label>
                <input
                  type="date"
                  value={customFrom}
                  onChange={(e) => setCustomFrom(e.target.value)}
                  className="h-9 w-full rounded-lg border border-zinc-200 bg-white px-3 text-sm text-zinc-900 focus:outline-none focus:ring-2 focus:ring-zinc-900/10"
                />
              </div>
              <div>
                <label className="mb-1 block text-xs text-zinc-500">До</label>
                <input
                  type="date"
                  value={customTo}
                  onChange={(e) => setCustomTo(e.target.value)}
                  className="h-9 w-full rounded-lg border border-zinc-200 bg-white px-3 text-sm text-zinc-900 focus:outline-none focus:ring-2 focus:ring-zinc-900/10"
                />
              </div>
              <div className="flex gap-2">
                <button
                  type="button"
                  className="flex-1 rounded-lg border border-zinc-200 px-3 py-1.5 text-sm text-zinc-600 hover:bg-zinc-50"
                  onClick={() => setCustomMode(false)}
                >
                  Назад
                </button>
                <button
                  type="button"
                  className="flex-1 rounded-lg bg-zinc-900 px-3 py-1.5 text-sm text-white hover:bg-zinc-800 disabled:bg-zinc-400"
                  disabled={!customFrom || !customTo}
                  onClick={handleCustomApply}
                >
                  Применить
                </button>
              </div>
            </div>
          )}
        </div>
      )}
    </div>
  );
}

export const DEFAULT_DATE_RANGE: DateRange = PRESETS[1]; // Последние 7 дней
