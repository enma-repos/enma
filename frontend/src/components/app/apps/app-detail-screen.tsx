"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { SdkClientType } from "@/types/admin.types";
import type { SdkClientDto } from "@/types/admin.types";
import { useSdkClients } from "@/hooks/useSdkClients";
import { useApiKeys } from "@/hooks/useApiKeys";
import { Button, Card, IconArrowLeft, IconCopy, IconKey, IconPlus, Input } from "@/components/shared";
import { ApiKeysTable } from "@/components/app/apps/api-keys-table";

const clientTypeLabels: Record<SdkClientType, string> = {
  [SdkClientType.WebsiteSdk]: "Website SDK",
  [SdkClientType.ServerToServer]: "Server-to-Server",
  [SdkClientType.MobileSdk]: "Mobile SDK",
};

const clientTypeOptions: { value: SdkClientType; label: string }[] = [
  { value: SdkClientType.WebsiteSdk, label: "Website SDK" },
  { value: SdkClientType.ServerToServer, label: "Server-to-Server" },
  { value: SdkClientType.MobileSdk, label: "Mobile SDK" },
];

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
    setClientType,
    isSavingType,
  } = useSdkClients(organizationSlug, projectKey);

  const app = sdkClients.find((c: SdkClientDto) => c.id === appId) ?? null;

  const {
    apiKeys,
    isLoading: isLoadingKeys,
    createKey,
    isCreatingKey,
    createdKey,
    resetCreatedKey,
    revokeKey,
    isRevoking,
  } = useApiKeys(organizationId, projectId, appId);

  const [name, setName] = useState("");
  const [type, setType] = useState<SdkClientType>(SdkClientType.WebsiteSdk);
  const [copied, setCopied] = useState(false);

  useEffect(() => {
    if (!app) return;
    setName(app.name);
    setType(app.type);
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
  const typeChanged = type !== app.type;

  const handleSaveName = async () => {
    if (!nameChanged || !name.trim()) return;
    try {
      await setClientName({ id: app.id, name: name.trim() });
    } catch {
      // error visible via parent state
    }
  };

  const handleSaveType = async () => {
    if (!typeChanged) return;
    try {
      await setClientType({ id: app.id, type });
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

          <label className="block">
            <div className="text-xs font-medium text-zinc-500">Тип</div>
            <div className="mt-1 flex items-center gap-2">
              <select
                value={type}
                onChange={(e) => setType(Number(e.currentTarget.value) as SdkClientType)}
                className="h-10 w-full flex-1 rounded-lg border border-zinc-200 bg-white px-3 text-sm text-zinc-900 outline-none transition-colors focus:border-zinc-400 focus:ring-2 focus:ring-zinc-900/10"
              >
                {clientTypeOptions.map((opt) => (
                  <option key={opt.value} value={opt.value}>
                    {opt.label}
                  </option>
                ))}
              </select>
              {typeChanged ? (
                <Button
                  variant="primary"
                  size="sm"
                  onClick={handleSaveType}
                  disabled={isSavingType}
                >
                  {isSavingType ? "..." : "Сохранить"}
                </Button>
              ) : null}
            </div>
          </label>

          <div>
            <div className="text-xs font-medium text-zinc-500">Описание</div>
            <div className="mt-1 text-sm text-zinc-900">{app.description ?? "—"}</div>
          </div>

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

        {isLoadingKeys ? (
          <Card>
            <div className="animate-pulse p-5 space-y-3">
              {Array.from({ length: 3 }).map((_, i) => (
                <div key={i} className="h-4 w-full rounded bg-zinc-100" />
              ))}
            </div>
          </Card>
        ) : apiKeys.length === 0 ? (
          <Card className="p-7">
            <h3 className="text-sm font-semibold text-zinc-900">API-ключи не найдены</h3>
            <p className="mt-2 text-sm text-zinc-500">
              Создайте первый API-ключ для этого приложения.
            </p>
          </Card>
        ) : (
          <ApiKeysTable apiKeys={apiKeys} onRevoke={revokeKey} isRevoking={isRevoking} />
        )}
      </div>
    </div>
  );
}
