"use client";

import { ChevronLeft, ChevronRight, Search } from "lucide-react";

const PAGE_SIZE_OPTIONS = [5, 10, 15, 20];

export type PaginationProps = {
  page: number;
  totalPages: number;
  totalCount: number;
  onPageChange: (page: number) => void;
  pageSize: number;
  onPageSizeChange: (pageSize: number) => void;
};

export function PaginationHeader({ pageSize, onPageSizeChange, search, onSearchChange }: Pick<PaginationProps, "pageSize" | "onPageSizeChange"> & { search?: string; onSearchChange?: (value: string) => void }) {
  return (
    <div className="flex items-center justify-between px-5 py-3">
      {onSearchChange != null ? (
        <div className="relative">
          <Search className="absolute left-2.5 top-1/2 h-4 w-4 -translate-y-1/2 text-zinc-400" />
          <input
            type="text"
            value={search ?? ""}
            onChange={(e) => onSearchChange(e.target.value)}
            placeholder="Поиск..."
            className="h-8 w-56 rounded-lg border border-zinc-200 bg-white pl-8 pr-3 text-sm text-zinc-700 outline-none transition-colors placeholder:text-zinc-400 focus:border-zinc-400"
          />
        </div>
      ) : <div />}
      <div className="flex items-center gap-2">
      <span className="text-sm text-zinc-500">Показать</span>
      <select
        value={pageSize}
        onChange={(e) => onPageSizeChange(Number(e.target.value))}
        className="h-8 rounded-lg border border-zinc-200 bg-white px-2 text-sm text-zinc-700 outline-none transition-colors focus:border-zinc-400"
      >
        {PAGE_SIZE_OPTIONS.map((opt) => (
          <option key={opt} value={opt}>
            {opt}
          </option>
        ))}
      </select>
      </div>
    </div>
  );
}

export function PaginationFooter({ page, totalPages, totalCount, pageSize, onPageChange }: Omit<PaginationProps, "onPageSizeChange">) {
  const from = (page - 1) * pageSize + 1;
  const to = Math.min(page * pageSize, totalCount);

  return (
    <div className="flex items-center justify-between px-5 py-3 border-t border-zinc-100">
      <span className="text-sm text-zinc-400">
        {totalCount > 0 ? `${from}–${to} из ${totalCount}` : "Нет данных"}
      </span>
      <div className="flex items-center gap-1">
        <button
          type="button"
          disabled={page <= 1}
          onClick={() => onPageChange(page - 1)}
          className="inline-flex h-8 w-8 items-center justify-center rounded-lg border border-zinc-200 text-zinc-400 transition-colors hover:bg-zinc-50 hover:text-zinc-700 disabled:opacity-30 disabled:cursor-not-allowed"
        >
          <ChevronLeft className="h-4 w-4" />
        </button>
        <span className="inline-flex h-8 min-w-8 items-center justify-center rounded-lg border border-zinc-300 bg-white px-2 text-sm font-medium text-zinc-700">
          {page}
        </span>
        <button
          type="button"
          disabled={page >= totalPages}
          onClick={() => onPageChange(page + 1)}
          className="inline-flex h-8 w-8 items-center justify-center rounded-lg border border-zinc-200 text-zinc-400 transition-colors hover:bg-zinc-50 hover:text-zinc-700 disabled:opacity-30 disabled:cursor-not-allowed"
        >
          <ChevronRight className="h-4 w-4" />
        </button>
      </div>
    </div>
  );
}
