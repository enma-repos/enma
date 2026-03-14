"use client";

import { ProcessType } from "@/types/admin.types";
import type { ProcessDefinitionDto } from "@/types/admin.types";
import { Button, Card, IconTrash } from "@/components/shared";

const processTypeLabels: Record<ProcessType, string> = {
  [ProcessType.Session]: "Сессия",
  [ProcessType.Order]: "Заказ",
  [ProcessType.Checkout]: "Оформление",
  [ProcessType.Ticket]: "Тикет",
  [ProcessType.Custom]: "Пользовательский",
};

function formatDate(value: string) {
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return value;
  return new Intl.DateTimeFormat("ru-RU", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  }).format(date);
}

export type ProcessesTableProps = {
  processes: ProcessDefinitionDto[];
  onSelect: (process: ProcessDefinitionDto) => void;
  onDelete: (process: ProcessDefinitionDto) => void;
};

export function ProcessesTable({ processes, onSelect, onDelete }: ProcessesTableProps) {
  return (
    <Card className="overflow-hidden">
      <table className="w-full text-sm">
        <thead>
          <tr className="border-b border-zinc-100 text-left text-xs font-medium text-zinc-500">
            <th className="px-5 py-3">Название</th>
            <th className="px-5 py-3">Ключ</th>
            <th className="px-5 py-3">Тип</th>
            <th className="px-5 py-3">Дата создания</th>
            <th className="px-5 py-3 w-12" />
          </tr>
        </thead>
        <tbody>
          {processes.map((process) => (
            <tr
              key={process.id}
              className="border-b border-zinc-50 last:border-b-0 cursor-pointer transition-colors hover:bg-zinc-50"
              onClick={() => onSelect(process)}
            >
              <td className="px-5 py-4 font-medium text-zinc-900">{process.name}</td>
              <td className="px-5 py-4 font-mono text-zinc-500">{process.key}</td>
              <td className="px-5 py-4 text-zinc-500">{processTypeLabels[process.type] ?? "—"}</td>
              <td className="px-5 py-4 text-zinc-400">{formatDate(process.createdAt)}</td>
              <td className="px-5 py-4">
                <Button
                  variant="ghost"
                  size="sm"
                  className="h-8 w-8 p-0 text-zinc-400 hover:text-red-600"
                  onClick={(e) => {
                    e.stopPropagation();
                    onDelete(process);
                  }}
                  aria-label={`Удалить ${process.name}`}
                >
                  <IconTrash className="h-4 w-4" />
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </Card>
  );
}
