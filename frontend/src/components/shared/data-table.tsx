"use client";

import { Card } from "@/components/shared/card";

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
};

export function DataTable<T>({ columns, rows, getRowKey, onRowClick, renderActions }: DataTableProps<T>) {
  return (
    <Card className="overflow-hidden">
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
          {rows.map((row) => (
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
          ))}
        </tbody>
      </table>
    </Card>
  );
}
