"use client";

import { useEffect, useMemo, useState } from "react";
import type { CreateProjectDto } from "@/types/admin.types";
import { Button, CardDescription, CardHeader, CardTitle, IconX, Input, Modal } from "@/components/shared";

function keyify(value: string) {
  return value
    .trim()
    .toLowerCase()
    .replace(/[^a-z0-9]+/g, "-")
    .replace(/(^-|-$)+/g, "");
}

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось создать проект.";
}

export type CreateProjectDialogProps = {
  open: boolean;
  onClose: () => void;
  organizationSlug: string;
  onCreate: (dto: Omit<CreateProjectDto, "organizationId" | "createdByUserId">) => Promise<unknown>;
  isCreating: boolean;
  error: unknown;
};

export function CreateProjectDialog({
  open,
  onClose,
  organizationSlug,
  onCreate,
  isCreating,
  error,
}: CreateProjectDialogProps) {
  const [name, setName] = useState("");
  const [key, setKey] = useState("");
  const [keyTouched, setKeyTouched] = useState(false);
  const [description, setDescription] = useState("");

  useEffect(() => {
    if (!open) return;
    setName("");
    setKey("");
    setKeyTouched(false);
    setDescription("");
  }, [open]);

  useEffect(() => {
    if (keyTouched) return;
    setKey(keyify(name));
  }, [name, keyTouched]);

  const canSubmit = useMemo(() => {
    return name.trim().length > 0 && key.trim().length > 0 && !isCreating;
  }, [isCreating, key, name]);

  const onSubmit = async () => {
    if (!canSubmit) return;
    try {
      await onCreate({
        name: name.trim(),
        key: key.trim(),
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
          <CardTitle>Создать проект</CardTitle>
          <CardDescription>Название, ключ и опциональное описание.</CardDescription>
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
              placeholder="Например: dashboard"
              className="mt-2"
              autoFocus
            />
          </label>

          <label className="block">
            <div className="text-sm font-medium text-zinc-700">Ключ</div>
            <Input
              value={key}
              onChange={(e) => {
                setKeyTouched(true);
                setKey(e.currentTarget.value);
              }}
              placeholder="dashboard"
              className="mt-2"
            />
            <div className="mt-2 text-xs text-zinc-400">
              Будет использоваться в URL:{" "}
              <span className="font-mono">
                /app/organizations/{organizationSlug}/projects/{key || "key"}
              </span>
            </div>
          </label>

          <label className="block">
            <div className="text-sm font-medium text-zinc-700">Описание</div>
            <Input
              value={description}
              onChange={(e) => setDescription(e.currentTarget.value)}
              placeholder="Коротко о проекте"
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

