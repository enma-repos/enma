"use client";

import { Button, CardDescription, CardHeader, CardTitle, IconX, Modal } from "@/components/shared";

export type ConfirmDialogProps = {
  open: boolean;
  onClose: () => void;
  onConfirm: () => void;
  title: string;
  description?: string;
  confirmLabel?: string;
  isConfirming?: boolean;
};

export function ConfirmDialog({
  open,
  onClose,
  onConfirm,
  title,
  description,
  confirmLabel = "Удалить",
  isConfirming = false,
}: ConfirmDialogProps) {
  return (
    <Modal open={open} onClose={onClose}>
      <CardHeader className="flex flex-row items-start justify-between gap-4">
        <div>
          <CardTitle>{title}</CardTitle>
          {description ? <CardDescription>{description}</CardDescription> : null}
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
        <div className="flex flex-col-reverse gap-2 sm:flex-row sm:justify-end">
          <Button onClick={onClose} disabled={isConfirming}>
            Отмена
          </Button>
          <Button variant="primary" onClick={onConfirm} disabled={isConfirming}>
            {isConfirming ? "Удаление..." : confirmLabel}
          </Button>
        </div>
      </div>
    </Modal>
  );
}
