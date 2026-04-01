"use client";

import { Loader2 } from "lucide-react";
import { Card } from "@/components/shared/card";
import { PaginationHeader, PaginationFooter } from "@/components/shared/pagination";

export type DataTableColumn<T> = {
  key: string;
  header: string;
  render: (row: T) => React.ReactNode;
  className?: string;
};

export type DataTableProps<T> = {
  columns: DataTableColumn<T>[];
  rows: T[];
  getRowKey: (row: T) => string;
  onRowClick?: (row: T) => void;
  renderActions?: (row: T) => React.ReactNode;
  isLoading?: boolean;
  emptyMessage?: string;
  page?: number;
  totalPages?: number;
  totalCount?: number;
  onPageChange?: (page: number) => void;
  pageSize?: number;
  onPageSizeChange?: (pageSize: number) => void;
  search?: string;
  onSearchChange?: (value: string) => void;
};

export function DataTable<T>({ columns, rows, getRowKey, onRowClick, renderActions, isLoading, emptyMessage = "Нет данных", page, totalPages, totalCount, onPageChange, pageSize, onPageSizeChange, search, onSearchChange }: DataTableProps<T>) {
  const hasPagination = page != null && totalPages != null && totalCount != null && onPageChange != null && pageSize != null && onPageSizeChange != null;
  const colSpan = columns.length + (renderActions ? 1 : 0);

  return (
    <Card className="overflow-hidden">
      {hasPagination && (
        <PaginationHeader pageSize={pageSize} onPageSizeChange={onPageSizeChange} search={search} onSearchChange={onSearchChange} />
      )}
      <table className="w-full text-sm">
        <thead>
          <tr className="border-b border-zinc-100 text-left text-xs font-medium text-zinc-500">
            {columns.map((col) => (
              <th key={col.key} className={col.className ?? "px-5 py-3"}>
                {col.header}
              </th>
            ))}
            {renderActions ? <th className="px-5 py-3 w-12" /> : null}
          </tr>
        </thead>
        <tbody>
          {isLoading ? (
            <tr>
              <td colSpan={colSpan} className="py-12">
                <div className="flex justify-center">
                  <Loader2 className="h-6 w-6 animate-spin text-zinc-300" />
                </div>
              </td>
            </tr>
          ) : rows.length === 0 ? (
            <tr>
              <td colSpan={colSpan} className="px-5 py-12 text-center text-sm text-zinc-400">
                {emptyMessage}
              </td>
            </tr>
          ) : (
            rows.map((row) => (
              <tr
                key={getRowKey(row)}
                className={`border-b border-zinc-50 last:border-b-0 transition-colors hover:bg-zinc-50 ${onRowClick ? "cursor-pointer" : ""}`}
                onClick={onRowClick ? () => onRowClick(row) : undefined}
              >
                {columns.map((col) => (
                  <td key={col.key} className={col.className ?? "px-5 py-4"}>
                    {col.render(row)}
                  </td>
                ))}
                {renderActions ? (
                  <td className="px-5 py-4" onClick={(e) => e.stopPropagation()}>
                    {renderActions(row)}
                  </td>
                ) : null}
              </tr>
            ))
          )}
        </tbody>
      </table>
      {hasPagination && (
        <PaginationFooter page={page} totalPages={totalPages} totalCount={totalCount} pageSize={pageSize} onPageChange={onPageChange} />
      )}
    </Card>
  );
}
