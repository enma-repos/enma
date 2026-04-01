"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import type { SdkClientDto } from "@/types/admin.types";
import { SdkClientType } from "@/types/admin.types";
import { useSdkClients } from "@/hooks/useSdkClients";
import { useApiKeys } from "@/hooks/useApiKeys";
import { Button, Card, IconArrowLeft, IconCopy, IconKey, IconPlus, Input } from "@/components/shared";
import { ApiKeysTable } from "@/components/app/apps/api-keys-table";

const clientTypeLabels: Record<SdkClientType, string> = {
  [SdkClientType.WebsiteSdk]: "Website SDK",
  [SdkClientType.ServerToServer]: "Server-to-Server",
  [SdkClientType.MobileSdk]: "Mobile SDK",
};

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

export type AppDetailScreenProps = {
  organizationSlug: string;
  projectKey: string;
  appId: string;
};

export function AppDetailScreen({ organizationSlug, projectKey, appId }: AppDetailScreenProps) {
  const {
    sdkClients,
    organizationId,
    projectId,
    isLoading,
    error,
    setClientName,
    isSavingName,
    setClientDescription,
    isSavingDescription,
  } = useSdkClients(organizationSlug, projectKey);

  const app = sdkClients.find((c: SdkClientDto) => c.id === appId) ?? null;

  const {
    apiKeys,
    page,
    setPage,
    totalPages,
    totalCount,
    isLoading: isLoadingKeys,
    createKey,
    isCreatingKey,
    createdKey,
    resetCreatedKey,
    revokeKey,
    isRevoking,
    pageSize,
    setPageSize,
    search,
    setSearch,
  } = useApiKeys(organizationId, projectId, appId);

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [copied, setCopied] = useState(false);

  useEffect(() => {
    if (!app) return;
    setName(app.name);
    setDescription(app.description ?? "");
  }, [app]);

  const backHref = `/app/organizations/${organizationSlug}/projects/${projectKey}/apps`;

  if (isLoading) {
    return (
      <div className="mx-auto w-full max-w-[90rem] mt-6">
        <Card>
          <div className="animate-pulse p-6 space-y-4">
            <div className="h-5 w-1/3 rounded bg-zinc-100" />
            <div className="h-4 w-1/2 rounded bg-zinc-100" />
            <div className="h-4 w-1/4 rounded bg-zinc-100" />
          </div>
        </Card>
      </div>
    );
  }

  if (error || !app) {
    return (
      <div className="mx-auto w-full max-w-[90rem] mt-6">
        <Link href={backHref} className="inline-flex items-center gap-2 text-sm text-zinc-500 hover:text-zinc-900 transition-colors mb-4">
          <IconArrowLeft className="h-4 w-4" />
          Назад к приложениям
        </Link>
        <div className="rounded-2xl border border-red-200 bg-red-50 p-5 text-sm text-red-700">
          {error instanceof Error ? error.message : "Приложение не найдено."}
        </div>
      </div>
    );
  }

  const nameChanged = name.trim() !== app.name;
  const descriptionChanged = description.trim() !== (app.description ?? "");

  const handleSaveName = async () => {
    if (!nameChanged || !name.trim()) return;
    try {
      await setClientName({ id: app.id, name: name.trim() });
    } catch {
      // error visible via parent state
    }
  };

  const handleSaveDescription = async () => {
    if (!descriptionChanged) return;
    try {
      await setClientDescription({ id: app.id, description: description.trim() || null });
    } catch {
      // error visible via parent state
    }
  };

  const handleCreateKey = async () => {
    try {
      await createKey();
      setCopied(false);
    } catch {
      // error visible via hook state
    }
  };

  const handleCopyKey = async () => {
    if (!createdKey) return;
    try {
      await navigator.clipboard.writeText(createdKey.key);
      setCopied(true);
    } catch {
      // fallback: do nothing
    }
  };

  return (
    <div className="mx-auto w-full max-w-[90rem] mt-6 space-y-6">
      <Link href={backHref} className="inline-flex items-center gap-2 text-sm text-zinc-500 hover:text-zinc-900 transition-colors">
        <IconArrowLeft className="h-4 w-4" />
        Назад к приложениям
      </Link>

      <Card className="p-6">
        <h1 className="text-xl font-semibold text-zinc-900 mb-6">Детали приложения</h1>

        <div className="space-y-4">
          <div>
            <div className="text-xs font-medium text-zinc-500">ID</div>
            <div className="mt-1 text-sm text-zinc-900 font-mono">{app.id}</div>
          </div>

          <label className="block">
            <div className="text-xs font-medium text-zinc-500">Название</div>
            <div className="mt-1 flex items-center gap-2">
              <Input
                value={name}
                onChange={(e) => setName(e.currentTarget.value)}
                className="flex-1"
              />
              {nameChanged ? (
                <Button
                  variant="primary"
                  size="sm"
                  onClick={handleSaveName}
                  disabled={!name.trim() || isSavingName}
                >
                  {isSavingName ? "..." : "Сохранить"}
                </Button>
              ) : null}
            </div>
          </label>

          <div>
            <div className="text-xs font-medium text-zinc-500">Тип</div>
            <div className="mt-1 text-sm text-zinc-900">{clientTypeLabels[app.type] ?? app.type}</div>
          </div>

          <label className="block">
            <div className="text-xs font-medium text-zinc-500">Описание</div>
            <div className="mt-1 flex items-center gap-2">
              <Input
                value={description}
                onChange={(e) => setDescription(e.currentTarget.value)}
                className="flex-1"
              />
              {descriptionChanged ? (
                <Button
                  variant="primary"
                  size="sm"
                  onClick={handleSaveDescription}
                  disabled={isSavingDescription}
                >
                  {isSavingDescription ? "..." : "Сохранить"}
                </Button>
              ) : null}
            </div>
          </label>

          <div>
            <div className="text-xs font-medium text-zinc-500">Дата создания</div>
            <div className="mt-1 text-sm text-zinc-900">{formatDate(app.createdAt)}</div>
          </div>

          <div>
            <div className="text-xs font-medium text-zinc-500">Последнее обновление</div>
            <div className="mt-1 text-sm text-zinc-900">{formatDate(app.updatedAt)}</div>
          </div>

          {app.disabledAt ? (
            <div>
              <div className="text-xs font-medium text-zinc-500">Отключено</div>
              <div className="mt-1 text-sm text-red-600">{formatDate(app.disabledAt)}</div>
            </div>
          ) : null}
        </div>
      </Card>

      <div>
        <div className="flex flex-wrap items-center justify-between gap-3 mb-4">
          <h2 className="text-lg font-semibold text-zinc-900 flex items-center gap-2">
            <IconKey className="h-5 w-5 text-zinc-400" />
            API-ключи
          </h2>
          <Button variant="primary" className="rounded-xl" onClick={handleCreateKey} disabled={isCreatingKey}>
            <IconPlus className="h-4 w-4" aria-hidden="true" />
            {isCreatingKey ? "Создаём..." : "Создать ключ"}
          </Button>
        </div>

        {createdKey ? (
          <div className="mb-4 rounded-xl border border-amber-200 bg-amber-50 p-4">
            <div className="flex items-center gap-2 text-sm font-medium text-amber-800 mb-2">
              Новый API-ключ создан
            </div>
            <div className="flex items-center gap-2">
              <code className="flex-1 rounded-lg bg-white border border-amber-200 px-3 py-2 text-sm font-mono text-zinc-900 select-all">
                {createdKey.key}
              </code>
              <Button
                variant={copied ? "primary" : "secondary"}
                size="sm"
                onClick={handleCopyKey}
                className="shrink-0"
              >
                <IconCopy className="h-4 w-4" />
                {copied ? "Скопировано" : "Копировать"}
              </Button>
            </div>
            <p className="mt-2 text-xs text-amber-700">
              Сохраните ключ сейчас — после закрытия этого уведомления вы не сможете увидеть его снова.
            </p>
            <div className="mt-3 flex justify-end">
              <Button size="sm" onClick={resetCreatedKey}>
                Закрыть
              </Button>
            </div>
          </div>
        ) : null}

        <ApiKeysTable apiKeys={apiKeys} onRevoke={revokeKey} isRevoking={isRevoking} isLoading={isLoadingKeys} page={page} totalPages={totalPages} onPageChange={setPage} pageSize={pageSize} onPageSizeChange={setPageSize} totalCount={totalCount} search={search} onSearchChange={setSearch} />
      </div>
    </div>
  );
}
