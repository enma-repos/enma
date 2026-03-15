"use client";

import { useEffect, useState } from "react";
import type { EventDefinitionDto } from "@/types/admin.types";
import { Button, CardHeader, CardTitle, IconX, Input, Modal } from "@/components/shared";

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

export type EventDetailDialogProps = {
  event: EventDefinitionDto | null;
  open: boolean;
  onClose: () => void;
  onSetDescription: (dto: { id: string; description: string | null }) => Promise<unknown>;
  isSavingDescription: boolean;
};

export function EventDetailDialog({
  event,
  open,
  onClose,
  onSetDescription,
  isSavingDescription,
}: EventDetailDialogProps) {
  const [description, setDescription] = useState("");

  useEffect(() => {
    if (!event) return;
    setDescription(event.description ?? "");
  }, [event]);

  if (!event) return null;

  const descriptionChanged = (description.trim() || null) !== (event.description ?? null);

  const handleSaveDescription = async () => {
    if (!descriptionChanged) return;
    try {
      await onSetDescription({
        id: event.id,
        description: description.trim() || null,
      });
    } catch {
      // error visible via parent state
    }
  };

  return (
    <Modal open={open} onClose={onClose}>
      <CardHeader className="flex flex-row items-start justify-between gap-4">
        <CardTitle>Детали события</CardTitle>
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
        <div>
          <div className="text-xs font-medium text-zinc-500">ID</div>
          <div className="mt-1 font-mono text-sm text-zinc-900 select-all">{event.id}</div>
        </div>

        <div>
          <div className="text-xs font-medium text-zinc-500">Название</div>
          <div className="mt-1 text-sm text-zinc-900">{event.name}</div>
        </div>

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
          <div className="text-xs font-medium text-zinc-500">Дата создания</div>
          <div className="mt-1 text-sm text-zinc-900">{formatDate(event.createdAt)}</div>
        </div>

        <div>
          <div className="text-xs font-medium text-zinc-500">Последнее обновление</div>
          <div className="mt-1 text-sm text-zinc-900">{formatDate(event.updatedAt)}</div>
        </div>
      </div>
    </Modal>
  );
}
