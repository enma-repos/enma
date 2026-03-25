"use client";

import { useEffect, useMemo, useState } from "react";
import type { CreateEventDefinitionDto } from "@/types/admin.types";
import { Button, CardDescription, CardHeader, CardTitle, IconX, Input, Modal } from "@/components/shared";

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось создать событие.";
}

const LATIN_NAME_REGEX = /^[a-zA-Z0-9_.\-]+$/;

function isLatinName(value: string): boolean {
  return LATIN_NAME_REGEX.test(value);
}

export type CreateEventDialogProps = {
  open: boolean;
  onClose: () => void;
  onCreate: (dto: Omit<CreateEventDefinitionDto, "projectId">) => Promise<unknown>;
  isCreating: boolean;
  error: unknown;
};

export function CreateEventDialog({
  open,
  onClose,
  onCreate,
  isCreating,
  error,
}: CreateEventDialogProps) {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");

  useEffect(() => {
    if (!open) return;
    setName("");
    setDescription("");
  }, [open]);

  const nameValid = name.trim().length > 0 && isLatinName(name.trim());

  const canSubmit = useMemo(() => {
    return nameValid && !isCreating;
  }, [isCreating, nameValid]);

  const onSubmit = async () => {
    if (!canSubmit) return;
    try {
      await onCreate({
        name: name.trim(),
        description: description.trim().length ? description.trim() : null,
      });
      onClose();
    } catch {
      // error handled by parent hook state
    }
  };

  return (
    <Modal open={open} onClose={onClose}>
      <CardHeader className="flex flex-row items-start justify-between gap-4">
        <div>
          <CardTitle>Создать событие</CardTitle>
          <CardDescription>Название и опциональное описание.</CardDescription>
        </div>
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

      <div className="px-5 pb-5">
        <div className="space-y-4">
          <label className="block">
            <div className="text-sm font-medium text-zinc-700">Название</div>
            <Input
              value={name}
              onChange={(e) => setName(e.currentTarget.value)}
              placeholder="Например: page_view"
              className="mt-2"
              autoFocus
            />
            {name.trim().length > 0 && !isLatinName(name.trim()) ? (
              <p className="mt-1 text-xs text-red-500">
                Название может содержать только латиницу, цифры, _, . и -
              </p>
            ) : null}
          </label>

          <label className="block">
            <div className="text-sm font-medium text-zinc-700">Описание</div>
            <Input
              value={description}
              onChange={(e) => setDescription(e.currentTarget.value)}
              placeholder="Коротко о событии"
              className="mt-2"
            />
          </label>

          {error ? (
            <div className="rounded-xl border border-red-200 bg-red-50 p-3 text-sm text-red-700">
              {getErrorMessage(error)}
            </div>
          ) : null}
        </div>

        <div className="mt-6 flex flex-col-reverse gap-2 sm:flex-row sm:justify-end">
          <Button onClick={onClose} disabled={isCreating}>
            Отмена
          </Button>
          <Button variant="primary" onClick={onSubmit} disabled={!canSubmit}>
            {isCreating ? "Создаём..." : "Создать"}
          </Button>
        </div>
      </div>
    </Modal>
  );
}
