"use client";

import Link from "next/link";
import { ArrowLeft, Loader2 } from "lucide-react";
import { Badge, Card } from "@/components/shared";
import { useSuperUser } from "@/hooks/admin/useSuperUser";
import { formatDateTime } from "@/components/app/admin/format";
import type { Guid } from "@/types/admin.types";

export type AdminUserDetailProps = {
  userId: Guid;
};

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось загрузить пользователя.";
}

export function AdminUserDetail({ userId }: AdminUserDetailProps) {
  const { data, isLoading, error } = useSuperUser(userId);

  return (
    <div className="mx-auto w-full max-w-[90rem]">
      <Link
        href="/admin/users"
        className="mb-4 inline-flex items-center gap-1 text-sm text-zinc-500 hover:text-zinc-800"
      >
        <ArrowLeft className="h-4 w-4" />
        Назад к списку
      </Link>

      {error ? (
        <div className="mb-4 rounded-2xl border border-red-200 bg-red-50 p-5 text-sm text-red-700">
          {getErrorMessage(error)}
        </div>
      ) : null}

      {isLoading ? (
        <div className="flex justify-center py-12">
          <Loader2 className="h-6 w-6 animate-spin text-zinc-300" />
        </div>
      ) : data ? (
        <div className="space-y-6">
          <Card className="p-6">
            <div className="flex flex-col gap-1">
              <div className="flex items-center gap-2">
                <h1 className="text-xl font-semibold text-zinc-900">
                  {data.displayName || data.email}
                </h1>
                {data.deletedAt ? (
                  <Badge className="bg-rose-50 text-rose-700 ring-rose-200">Удалён</Badge>
                ) : null}
              </div>
              <p className="text-sm text-zinc-500">{data.email}</p>
            </div>

            <dl className="mt-5 grid grid-cols-1 gap-4 text-sm sm:grid-cols-2 lg:grid-cols-4">
              <div>
                <dt className="text-xs font-medium uppercase tracking-wide text-zinc-400">Локаль</dt>
                <dd className="mt-1 text-zinc-800">{data.locale ?? "—"}</dd>
              </div>
              <div>
                <dt className="text-xs font-medium uppercase tracking-wide text-zinc-400">Часовой пояс</dt>
                <dd className="mt-1 text-zinc-800">{data.timezone ?? "—"}</dd>
              </div>
              <div>
                <dt className="text-xs font-medium uppercase tracking-wide text-zinc-400">Создан</dt>
                <dd className="mt-1 text-zinc-800">{formatDateTime(data.createdAt)}</dd>
              </div>
              <div>
                <dt className="text-xs font-medium uppercase tracking-wide text-zinc-400">Обновлён</dt>
                <dd className="mt-1 text-zinc-800">{formatDateTime(data.updatedAt)}</dd>
              </div>
            </dl>
          </Card>

          <Card className="p-6">
            <h2 className="text-sm font-semibold text-zinc-900">
              Организации{" "}
              <span className="font-normal text-zinc-400">
                {data.organizations.length}
              </span>
            </h2>
            {data.organizations.length === 0 ? (
              <p className="mt-3 text-sm text-zinc-400">Нет членств.</p>
            ) : (
              <ul className="mt-3 divide-y divide-zinc-100">
                {data.organizations.map((o) => (
                  <li
                    key={o.organizationId}
                    className="flex items-center justify-between py-2.5"
                  >
                    <div>
                      <Link
                        href={`/admin/organizations/${o.organizationId}`}
                        className="text-sm font-medium text-zinc-800 hover:underline"
                      >
                        {o.organizationName}
                      </Link>
                      <p className="text-xs text-zinc-400">{o.organizationSlug}</p>
                    </div>
                    <div className="flex items-center gap-3 text-xs text-zinc-500">
                      <span>Роль: {o.role}</span>
                      <span>{formatDateTime(o.joinedAt)}</span>
                    </div>
                  </li>
                ))}
              </ul>
            )}
          </Card>

          <Card className="p-6">
            <h2 className="text-sm font-semibold text-zinc-900">
              Проекты{" "}
              <span className="font-normal text-zinc-400">
                {data.projects.length}
              </span>
            </h2>
            {data.projects.length === 0 ? (
              <p className="mt-3 text-sm text-zinc-400">Нет членств.</p>
            ) : (
              <ul className="mt-3 divide-y divide-zinc-100">
                {data.projects.map((p) => (
                  <li
                    key={p.projectId}
                    className="flex items-center justify-between py-2.5"
                  >
                    <div>
                      <Link
                        href={`/admin/projects/${p.projectId}`}
                        className="text-sm font-medium text-zinc-800 hover:underline"
                      >
                        {p.projectName}
                      </Link>
                      <p className="text-xs text-zinc-400">
                        {p.organizationName} · {p.projectKey}
                      </p>
                    </div>
                    <div className="flex items-center gap-3 text-xs text-zinc-500">
                      <span>Роль: {p.role}</span>
                      <span>{formatDateTime(p.joinedAt)}</span>
                    </div>
                  </li>
                ))}
              </ul>
            )}
          </Card>
        </div>
      ) : null}
    </div>
  );
}
