"use client";

import Link from "next/link";
import { ArrowLeft, Loader2 } from "lucide-react";
import { Badge, Card } from "@/components/shared";
import { useSuperOrganization } from "@/hooks/admin/useSuperOrganization";
import { formatDateTime } from "@/components/app/admin/format";
import type { Guid } from "@/types/admin.types";

export type AdminOrganizationDetailProps = {
  organizationId: Guid;
};

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось загрузить организацию.";
}

export function AdminOrganizationDetail({
  organizationId,
}: AdminOrganizationDetailProps) {
  const { data, isLoading, error } = useSuperOrganization(organizationId);

  return (
    <div className="mx-auto w-full max-w-[90rem]">
      <Link
        href="/admin/organizations"
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
                <h1 className="text-xl font-semibold text-zinc-900">{data.name}</h1>
                {data.deletedAt ? (
                  <Badge className="bg-rose-50 text-rose-700 ring-rose-200">Удалена</Badge>
                ) : null}
              </div>
              <p className="text-sm text-zinc-500">{data.slug}</p>
              {data.description ? (
                <p className="mt-2 text-sm text-zinc-600">{data.description}</p>
              ) : null}
            </div>

            <dl className="mt-5 grid grid-cols-1 gap-4 text-sm sm:grid-cols-2 lg:grid-cols-4">
              <div>
                <dt className="text-xs font-medium uppercase tracking-wide text-zinc-400">Владелец</dt>
                <dd className="mt-1 text-zinc-800">
                  {data.ownerEmail ?? "—"}
                  {data.ownerDisplayName ? (
                    <span className="ml-1 text-zinc-400">({data.ownerDisplayName})</span>
                  ) : null}
                </dd>
              </div>
              <div>
                <dt className="text-xs font-medium uppercase tracking-wide text-zinc-400">Создана</dt>
                <dd className="mt-1 text-zinc-800">{formatDateTime(data.createdAt)}</dd>
              </div>
              <div>
                <dt className="text-xs font-medium uppercase tracking-wide text-zinc-400">Обновлена</dt>
                <dd className="mt-1 text-zinc-800">{formatDateTime(data.updatedAt)}</dd>
              </div>
              {data.deletedAt ? (
                <div>
                  <dt className="text-xs font-medium uppercase tracking-wide text-zinc-400">Удалена</dt>
                  <dd className="mt-1 text-zinc-800">{formatDateTime(data.deletedAt)}</dd>
                </div>
              ) : null}
            </dl>
          </Card>

          <Card className="p-6">
            <h2 className="text-sm font-semibold text-zinc-900">
              Участники{" "}
              <span className="font-normal text-zinc-400">{data.members.length}</span>
            </h2>
            {data.members.length === 0 ? (
              <p className="mt-3 text-sm text-zinc-400">Нет участников.</p>
            ) : (
              <ul className="mt-3 divide-y divide-zinc-100">
                {data.members.map((m) => (
                  <li
                    key={m.userId}
                    className="flex items-center justify-between py-2.5"
                  >
                    <div>
                      <Link
                        href={`/admin/users/${m.userId}`}
                        className="text-sm font-medium text-zinc-800 hover:underline"
                      >
                        {m.displayName || m.email}
                      </Link>
                      <p className="text-xs text-zinc-400">{m.email}</p>
                    </div>
                    <div className="flex items-center gap-3 text-xs text-zinc-500">
                      <span>Роль: {m.role}</span>
                      <span>{formatDateTime(m.joinedAt)}</span>
                    </div>
                  </li>
                ))}
              </ul>
            )}
          </Card>

          <Card className="p-6">
            <h2 className="text-sm font-semibold text-zinc-900">
              Проекты{" "}
              <span className="font-normal text-zinc-400">{data.projects.length}</span>
            </h2>
            {data.projects.length === 0 ? (
              <p className="mt-3 text-sm text-zinc-400">Нет проектов.</p>
            ) : (
              <ul className="mt-3 divide-y divide-zinc-100">
                {data.projects.map((p) => (
                  <li
                    key={p.id}
                    className="flex items-center justify-between py-2.5"
                  >
                    <div>
                      <Link
                        href={`/admin/projects/${p.id}`}
                        className="text-sm font-medium text-zinc-800 hover:underline"
                      >
                        {p.name}
                      </Link>
                      <p className="text-xs text-zinc-400">{p.key}</p>
                    </div>
                    <div className="flex items-center gap-3 text-xs text-zinc-500">
                      <span>Участников: {p.memberCount}</span>
                      <span>{formatDateTime(p.createdAt)}</span>
                      {p.archivedAt ? (
                        <Badge className="bg-zinc-100 text-zinc-600 ring-zinc-200">Архив</Badge>
                      ) : null}
                      {p.deletedAt ? (
                        <Badge className="bg-rose-50 text-rose-700 ring-rose-200">Удалён</Badge>
                      ) : null}
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
