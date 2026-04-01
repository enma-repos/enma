"use client";

import { Loader2 } from "lucide-react";
import type { ApiKeyDto } from "@/types/admin.types";
import { Button, Card, PaginationHeader, PaginationFooter } from "@/components/shared";

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

export type ApiKeysTableProps = {
  apiKeys: ApiKeyDto[];
  onRevoke: (id: string) => Promise<unknown>;
  isRevoking: boolean;
  isLoading?: boolean;
  page: number;
  totalPages: number;
  onPageChange: (page: number) => void;
  pageSize: number;
  onPageSizeChange: (pageSize: number) => void;
  totalCount: number;
  search: string;
  onSearchChange: (value: string) => void;
};

export function ApiKeysTable({ apiKeys, onRevoke, isRevoking, isLoading, page, totalPages, onPageChange, pageSize, onPageSizeChange, totalCount, search, onSearchChange }: ApiKeysTableProps) {
  return (
    <Card className="overflow-hidden">
      <PaginationHeader pageSize={pageSize} onPageSizeChange={onPageSizeChange} search={search} onSearchChange={onSearchChange} />
      <table className="w-full text-sm">
        <thead>
          <tr className="border-b border-zinc-100 text-left text-xs font-medium text-zinc-500">
            <th className="px-5 py-3">Ключ</th>
            <th className="px-5 py-3">Отправлено событий</th>
            <th className="px-5 py-3">Дата создания</th>
            <th className="px-5 py-3">Последнее использование</th>
            <th className="px-5 py-3">Статус</th>
            <th className="px-5 py-3 w-24" />
          </tr>
        </thead>
        <tbody>
          {isLoading ? (
            <tr>
              <td colSpan={6} className="py-12">
                <div className="flex justify-center">
                  <Loader2 className="h-6 w-6 animate-spin text-zinc-300" />
                </div>
              </td>
            </tr>
          ) : apiKeys.length === 0 ? (
            <tr>
              <td colSpan={6} className="px-5 py-12 text-center text-sm text-zinc-400">
                API-ключи не найдены
              </td>
            </tr>
          ) : (
            apiKeys.map((key) => (
              <tr key={key.id} className="border-b border-zinc-50 last:border-b-0">
                <td className="px-5 py-4 font-mono text-zinc-900">
                  <span>{key.keyPrefix}</span>
                  <span className="text-zinc-300 select-none">{"••••••••••••••••"}</span>
                </td>
                <td className="px-5 py-4 text-zinc-500">{key.sentEventsCount}</td>
                <td className="px-5 py-4 text-zinc-400">{formatDate(key.createdAt)}</td>
                <td className="px-5 py-4 text-zinc-400">{key.lastUsedAt ? formatDate(key.lastUsedAt) : "—"}</td>
                <td className="px-5 py-4">
                  {key.revokedAt ? (
                    <span className="inline-flex items-center rounded-full bg-red-50 px-2 py-0.5 text-xs font-medium text-red-700">
                      Отозван
                    </span>
                  ) : (
                    <span className="inline-flex items-center rounded-full bg-green-50 px-2 py-0.5 text-xs font-medium text-green-700">
                      Активен
                    </span>
                  )}
                </td>
                <td className="px-5 py-4">
                  {!key.revokedAt ? (
                    <Button
                      variant="ghost"
                      size="sm"
                      className="text-zinc-400 hover:text-red-600 text-xs"
                      onClick={() => onRevoke(key.id)}
                      disabled={isRevoking}
                    >
                      Отозвать
                    </Button>
                  ) : null}
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
      <PaginationFooter page={page} totalPages={totalPages} totalCount={totalCount} pageSize={pageSize} onPageChange={onPageChange} />
    </Card>
  );
}
