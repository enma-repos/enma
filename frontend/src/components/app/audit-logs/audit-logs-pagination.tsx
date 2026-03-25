"use client";

import { Button } from "@/components/shared";

export type AuditLogsPaginationProps = {
  offset: number;
  limit: number;
  total: number;
  onOffsetChange: (offset: number) => void;
};

export function AuditLogsPagination({ offset, limit, total, onOffsetChange }: AuditLogsPaginationProps) {
  if (total <= limit) return null;

  const currentPage = Math.floor(offset / limit) + 1;
  const totalPages = Math.ceil(total / limit);

  return (
    <div className="flex items-center justify-between mt-4">
      <span className="text-sm text-zinc-500">
        {offset + 1}–{Math.min(offset + limit, total)} из {total}
      </span>
      <div className="flex gap-2">
        <Button
          size="sm"
          className="rounded-xl"
          disabled={offset === 0}
          onClick={() => onOffsetChange(Math.max(0, offset - limit))}
        >
          Назад
        </Button>
        <Button
          size="sm"
          className="rounded-xl"
          disabled={currentPage >= totalPages}
          onClick={() => onOffsetChange(offset + limit)}
        >
          Вперёд
        </Button>
      </div>
    </div>
  );
}
