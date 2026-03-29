"use client";

import { useState, useRef, useEffect } from "react";
import { Button, IconChevronDown } from "@/components/shared";
import { Layers } from "lucide-react";
import type { ProcessDefinitionDto } from "@/types/admin.types";

interface Props {
  processDefinitions: ProcessDefinitionDto[];
  selectedIds: string[];
  onChange: (ids: string[]) => void;
}

export function ProcessSelector({ processDefinitions, selectedIds, onChange }: Props) {
  const [open, setOpen] = useState(false);
  const ref = useRef<HTMLDivElement>(null);

  useEffect(() => {
    function handleClick(e: MouseEvent) {
      if (ref.current && !ref.current.contains(e.target as Node)) {
        setOpen(false);
      }
    }
    document.addEventListener("mousedown", handleClick);
    return () => document.removeEventListener("mousedown", handleClick);
  }, []);

  const toggle = (id: string) => {
    onChange(
      selectedIds.includes(id)
        ? selectedIds.filter((x) => x !== id)
        : [...selectedIds, id],
    );
  };

  const label =
    selectedIds.length === 0
      ? "Все процессы"
      : selectedIds.length === 1
        ? processDefinitions.find((p) => p.id === selectedIds[0])?.name ||
          processDefinitions.find((p) => p.id === selectedIds[0])?.key ||
          "1 процесс"
        : `${selectedIds.length} процесс(ов)`;

  if (processDefinitions.length === 0) return null;

  return (
    <div ref={ref} className="relative">
      <Button
        size="sm"
        className="rounded-xl"
        onClick={() => setOpen(!open)}
      >
        <Layers size={14} className="text-zinc-500" />
        <span className="text-zinc-600">{label}</span>
        <IconChevronDown className="h-4 w-4 text-zinc-500" />
      </Button>

      {open && (
        <div className="absolute right-0 top-full z-50 mt-2 w-64 rounded-xl border border-zinc-200 bg-white p-2 shadow-lg">
          {processDefinitions.map((p) => (
            <button
              key={p.id}
              type="button"
              className={`flex w-full items-center gap-2 rounded-lg px-3 py-2 text-left text-sm transition-colors hover:bg-zinc-50 ${
                selectedIds.includes(p.id)
                  ? "bg-zinc-100 font-medium text-zinc-900"
                  : "text-zinc-600"
              }`}
              onClick={() => toggle(p.id)}
            >
              <input
                type="checkbox"
                checked={selectedIds.includes(p.id)}
                readOnly
                className="h-3.5 w-3.5 rounded border-zinc-300 text-indigo-500 accent-indigo-500"
              />
              <div>
                <span>{p.name || p.key}</span>
                {p.description && (
                  <span className="mt-0.5 block text-xs text-zinc-400 truncate">
                    {p.description}
                  </span>
                )}
              </div>
            </button>
          ))}
        </div>
      )}
    </div>
  );
}
