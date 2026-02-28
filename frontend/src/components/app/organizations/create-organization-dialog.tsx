"use client";

import { useEffect, useMemo, useState } from "react";
import type { CreateOrganizationDto } from "@/types/admin.types";
import { Button, CardDescription, CardHeader, CardTitle, IconX, Input, Modal } from "@/components/shared";

function slugify(value: string) {
  return value
    .trim()
    .toLowerCase()
    .replace(/[^a-z0-9]+/g, "-")
    .replace(/(^-|-$)+/g, "");
}

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось создать организацию.";
}

export type CreateOrganizationDialogProps = {
  open: boolean;
  onClose: () => void;
  onCreate: (dto: Omit<CreateOrganizationDto, "createdByUserId">) => Promise<unknown>;
  isCreating: boolean;
  error: unknown;
};

export function CreateOrganizationDialog({
  open,
  onClose,
  onCreate,
  isCreating,
  error,
}: CreateOrganizationDialogProps) {
  const [name, setName] = useState("");
  const [slug, setSlug] = useState("");
  const [slugTouched, setSlugTouched] = useState(false);

  useEffect(() => {
    if (!open) return;
    setName("");
    setSlug("");
    setSlugTouched(false);
  }, [open]);

  useEffect(() => {
    if (slugTouched) return;
    setSlug(slugify(name));
  }, [name, slugTouched]);

  const canSubmit = useMemo(() => {
    return name.trim().length > 0 && slug.trim().length > 0 && !isCreating;
  }, [isCreating, name, slug]);

  const onSubmit = async () => {
    if (!canSubmit) return;
    try {
      await onCreate({ name: name.trim(), slug: slug.trim() });
      onClose();
    } catch {
      // error handled by parent hook state
    }
  };

  return (
    <Modal open={open} onClose={onClose}>
      <CardHeader className="flex flex-row items-start justify-between gap-4">
        <div>
          <CardTitle>Создать организацию</CardTitle>
          <CardDescription>Название и уникальный slug для URL.</CardDescription>
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
              placeholder="Например: newe"
              className="mt-2"
              autoFocus
            />
          </label>

          <label className="block">
            <div className="text-sm font-medium text-zinc-700">Slug</div>
            <Input
              value={slug}
              onChange={(e) => {
                setSlugTouched(true);
                setSlug(e.currentTarget.value);
              }}
              placeholder="newe"
              className="mt-2"
            />
            <div className="mt-2 text-xs text-zinc-400">
              Будет использоваться в URL: <span className="font-mono">/app/organizations/{slug || "slug"}</span>
            </div>
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
