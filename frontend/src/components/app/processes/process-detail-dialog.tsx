"use client";

import { useEffect, useState } from "react";
import { ProcessType } from "@/types/admin.types";
import type { ProcessDefinitionDto } from "@/types/admin.types";
import { Button, CardHeader, CardTitle, IconX, Input, Modal } from "@/components/shared";

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
    hour: "2-digit",
    minute: "2-digit",
  }).format(date);
}

export type ProcessDetailDialogProps = {
  process: ProcessDefinitionDto | null;
  open: boolean;
  onClose: () => void;
  onSetName: (dto: { id: string; name: string }) => Promise<unknown>;
  onSetDescription: (dto: { id: string; description: string | null }) => Promise<unknown>;
  isSavingName: boolean;
  isSavingDescription: boolean;
};

export function ProcessDetailDialog({
  process,
  open,
  onClose,
  onSetName,
  onSetDescription,
  isSavingName,
  isSavingDescription,
}: ProcessDetailDialogProps) {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");

  useEffect(() => {
    if (!process) return;
    setName(process.name);
    setDescription(process.description ?? "");
  }, [process]);

  if (!process) return null;

  const nameChanged = name.trim() !== process.name;
  const descriptionChanged = (description.trim() || null) !== (process.description ?? null);

  const handleSaveName = async () => {
    if (!nameChanged || !name.trim()) return;
    try {
      await onSetName({ id: process.id, name: name.trim() });
    } catch {
      // error visible via parent state
    }
  };

  const handleSaveDescription = async () => {
    if (!descriptionChanged) return;
    try {
      await onSetDescription({
        id: process.id,
        description: description.trim() || null,
      });
    } catch {
      // error visible via parent state
    }
  };

  return (
    <Modal open={open} onClose={onClose}>
      <CardHeader className="flex flex-row items-start justify-between gap-4">
        <CardTitle>Детали процесса</CardTitle>
        <Button
          variant="ghost"
          size="sm"
          className="h-10 w-10 rounded-xl p-0"
          onClick={onClose}
          aria-label="Close"
        >
          <IconX className="h-5 w-5" />
        </Button>
      </CardHeader>

      <div className="px-5 pb-5 space-y-4">
        <label className="block">
          <div className="text-xs font-medium text-zinc-500">Название</div>
          <div className="mt-1 flex items-center gap-2">
            <Input
              value={name}
              onChange={(e) => setName(e.currentTarget.value)}
              className="flex-1"
            />
            {nameChanged ? (
              <Button
                variant="primary"
                size="sm"
                onClick={handleSaveName}
                disabled={!name.trim() || isSavingName}
              >
                {isSavingName ? "..." : "Сохранить"}
              </Button>
            ) : null}
          </div>
        </label>

        <label className="block">
          <div className="text-xs font-medium text-zinc-500">Описание</div>
          <div className="mt-1 flex items-center gap-2">
            <Input
              value={description}
              onChange={(e) => setDescription(e.currentTarget.value)}
              placeholder="Без описания"
              className="flex-1"
            />
            {descriptionChanged ? (
              <Button
                variant="primary"
                size="sm"
                onClick={handleSaveDescription}
                disabled={isSavingDescription}
              >
                {isSavingDescription ? "..." : "Сохранить"}
              </Button>
            ) : null}
          </div>
        </label>

        <div>
          <div className="text-xs font-medium text-zinc-500">Ключ</div>
          <div className="mt-1 font-mono text-sm text-zinc-900">{process.key}</div>
        </div>

        <div>
          <div className="text-xs font-medium text-zinc-500">Тип</div>
          <div className="mt-1 text-sm text-zinc-900">{processTypeLabels[process.type] ?? "—"}</div>
        </div>

        <div>
          <div className="text-xs font-medium text-zinc-500">Дата создания</div>
          <div className="mt-1 text-sm text-zinc-900">{formatDate(process.createdAt)}</div>
        </div>

        <div>
          <div className="text-xs font-medium text-zinc-500">Последнее обновление</div>
          <div className="mt-1 text-sm text-zinc-900">{formatDate(process.updatedAt)}</div>
        </div>
      </div>
    </Modal>
  );
}
