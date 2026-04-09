"use client";

import Link from "next/link";
import { ArrowLeft, Loader2 } from "lucide-react";
import { Badge, Card } from "@/components/shared";
import { useSuperProject } from "@/hooks/admin/useSuperProject";
import { formatDateTime } from "@/components/app/admin/format";
import type { Guid } from "@/types/admin.types";

export type AdminProjectDetailProps = {
  projectId: Guid;
};

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось загрузить проект.";
}

export function AdminProjectDetail({ projectId }: AdminProjectDetailProps) {
  const { data, isLoading, error } = useSuperProject(projectId);

  return (
    <div className="mx-auto w-full max-w-[90rem]">
      <Link
        href="/admin/projects"
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
                  <Badge className="bg-rose-50 text-rose-700 ring-rose-200">Удалён</Badge>
                ) : data.archivedAt ? (
                  <Badge className="bg-zinc-100 text-zinc-600 ring-zinc-200">Архив</Badge>
                ) : null}
              </div>
              <p className="text-sm text-zinc-500">{data.key}</p>
              {data.description ? (
                <p className="mt-2 text-sm text-zinc-600">{data.description}</p>
              ) : null}
            </div>

            <dl className="mt-5 grid grid-cols-1 gap-4 text-sm sm:grid-cols-2 lg:grid-cols-4">
              <div>
                <dt className="text-xs font-medium uppercase tracking-wide text-zinc-400">Организация</dt>
                <dd className="mt-1">
                  <Link
                    href={`/admin/organizations/${data.organizationId}`}
                    className="text-zinc-800 hover:underline"
                  >
                    {data.organizationName}
                  </Link>
                  <span className="ml-1 text-zinc-400">({data.organizationSlug})</span>
                </dd>
              </div>
              <div>
                <dt className="text-xs font-medium uppercase tracking-wide text-zinc-400">Создан</dt>
                <dd className="mt-1 text-zinc-800">{formatDateTime(data.createdAt)}</dd>
              </div>
              <div>
                <dt className="text-xs font-medium uppercase tracking-wide text-zinc-400">Обновлён</dt>
                <dd className="mt-1 text-zinc-800">{formatDateTime(data.updatedAt)}</dd>
              </div>
              {data.archivedAt ? (
                <div>
                  <dt className="text-xs font-medium uppercase tracking-wide text-zinc-400">Архивирован</dt>
                  <dd className="mt-1 text-zinc-800">{formatDateTime(data.archivedAt)}</dd>
                </div>
              ) : null}
              {data.deletedAt ? (
                <div>
                  <dt className="text-xs font-medium uppercase tracking-wide text-zinc-400">Удалён</dt>
                  <dd className="mt-1 text-zinc-800">{formatDateTime(data.deletedAt)}</dd>
                </div>
              ) : null}
            </dl>

            <dl className="mt-5 grid grid-cols-1 gap-4 text-sm sm:grid-cols-3">
              <div>
                <dt className="text-xs font-medium uppercase tracking-wide text-zinc-400">SDK-клиенты</dt>
                <dd className="mt-1 text-zinc-800 tabular-nums">{data.sdkClientCount}</dd>
              </div>
              <div>
                <dt className="text-xs font-medium uppercase tracking-wide text-zinc-400">События</dt>
                <dd className="mt-1 text-zinc-800 tabular-nums">{data.eventDefinitionCount}</dd>
              </div>
              <div>
                <dt className="text-xs font-medium uppercase tracking-wide text-zinc-400">Процессы</dt>
                <dd className="mt-1 text-zinc-800 tabular-nums">{data.processDefinitionCount}</dd>
              </div>
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
        </div>
      ) : null}
    </div>
  );
}
