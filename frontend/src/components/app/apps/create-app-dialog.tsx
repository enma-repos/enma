"use client";

import { useEffect, useMemo, useState } from "react";
import { SdkClientType } from "@/types/admin.types";
import type { CreateSdkClientDto } from "@/types/admin.types";
import { Button, CardDescription, CardHeader, CardTitle, IconX, Input, Modal } from "@/components/shared";

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось создать приложение.";
}

const clientTypeOptions: { value: SdkClientType; label: string }[] = [
  { value: SdkClientType.WebsiteSdk, label: "Website SDK" },
  { value: SdkClientType.ServerToServer, label: "Server-to-Server" },
  { value: SdkClientType.MobileSdk, label: "Mobile SDK" },
];

export type CreateAppDialogProps = {
  open: boolean;
  onClose: () => void;
  onCreate: (dto: Omit<CreateSdkClientDto, "projectId">) => Promise<unknown>;
  isCreating: boolean;
  error: unknown;
};

export function CreateAppDialog({
  open,
  onClose,
  onCreate,
  isCreating,
  error,
}: CreateAppDialogProps) {
  const [name, setName] = useState("");
  const [type, setType] = useState<SdkClientType>(SdkClientType.WebsiteSdk);
  const [description, setDescription] = useState("");

  useEffect(() => {
    if (!open) return;
    setName("");
    setType(SdkClientType.WebsiteSdk);
    setDescription("");
  }, [open]);

  const canSubmit = useMemo(() => {
    return name.trim().length > 0 && !isCreating;
  }, [isCreating, name]);

  const onSubmit = async () => {
    if (!canSubmit) return;
    try {
      await onCreate({
        name: name.trim(),
        type,
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
          <CardTitle>Создать приложение</CardTitle>
          <CardDescription>Название, тип и опциональное описание.</CardDescription>
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
              placeholder="Например: Мой сайт"
              className="mt-2"
              autoFocus
            />
          </label>

          <label className="block">
            <div className="text-sm font-medium text-zinc-700">Тип</div>
            <select
              value={type}
              onChange={(e) => setType(Number(e.currentTarget.value) as SdkClientType)}
              className="mt-2 h-10 w-full rounded-lg border border-zinc-200 bg-white px-3 text-sm text-zinc-900 outline-none transition-colors focus:border-zinc-400 focus:ring-2 focus:ring-zinc-900/10"
            >
              {clientTypeOptions.map((opt) => (
                <option key={opt.value} value={opt.value}>
                  {opt.label}
                </option>
              ))}
            </select>
          </label>

          <label className="block">
            <div className="text-sm font-medium text-zinc-700">Описание</div>
            <Input
              value={description}
              onChange={(e) => setDescription(e.currentTarget.value)}
              placeholder="Коротко о приложении"
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
