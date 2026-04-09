"use client";

import type { ReactNode } from "react";
import {
  Boxes,
  Building2,
  ClipboardList,
  KeyRound,
  Loader2,
  Users,
} from "lucide-react";
import { Card } from "@/components/shared";
import { useSuperStats } from "@/hooks/admin/useSuperStats";

type TileProps = {
  label: string;
  value: number | null;
  icon: ReactNode;
  hint?: string;
};

function Tile({ label, value, icon, hint }: TileProps) {
  return (
    <Card className="p-5">
      <div className="flex items-start justify-between">
        <div>
          <p className="text-xs font-medium uppercase tracking-wide text-zinc-500">
            {label}
          </p>
          <p className="mt-2 text-2xl font-semibold text-zinc-900 tabular-nums">
            {value == null ? "—" : value.toLocaleString("ru-RU")}
          </p>
          {hint ? <p className="mt-1 text-xs text-zinc-400">{hint}</p> : null}
        </div>
        <div className="grid h-10 w-10 place-items-center rounded-xl bg-zinc-100 text-zinc-600">
          {icon}
        </div>
      </div>
    </Card>
  );
}

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось загрузить статистику.";
}

export function AdminOverviewScreen() {
  const { data, isLoading, error } = useSuperStats();

  return (
    <div className="mx-auto w-full max-w-[90rem]">
      <div className="mb-6">
        <h1 className="text-xl font-semibold text-zinc-900">Обзор платформы</h1>
        <p className="mt-1 text-sm text-zinc-500">
          Ключевые метрики по всем пользователям, организациям и проектам.
        </p>
      </div>

      {error ? (
        <div className="mb-4 rounded-2xl border border-red-200 bg-red-50 p-5 text-sm text-red-700">
          {getErrorMessage(error)}
        </div>
      ) : null}

      {isLoading ? (
        <div className="flex justify-center py-12">
          <Loader2 className="h-6 w-6 animate-spin text-zinc-300" />
        </div>
      ) : (
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-5">
          <Tile
            label="Пользователи"
            value={data?.totalUsers ?? null}
            icon={<Users className="h-5 w-5" />}
          />
          <Tile
            label="Организации"
            value={data?.totalOrganizations ?? null}
            icon={<Building2 className="h-5 w-5" />}
          />
          <Tile
            label="Проекты"
            value={data?.totalProjects ?? null}
            icon={<Boxes className="h-5 w-5" />}
          />
          <Tile
            label="API-ключи"
            value={data?.totalApiKeys ?? null}
            icon={<KeyRound className="h-5 w-5" />}
          />
          <Tile
            label="Аудит-логи (24ч)"
            value={data?.recentAuditLogsLast24Hours ?? null}
            icon={<ClipboardList className="h-5 w-5" />}
            hint="За последние 24 часа"
          />
        </div>
      )}
    </div>
  );
}
