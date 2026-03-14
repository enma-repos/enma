"use client";

import { useEffect, useState } from "react";
import { Button, Input } from "@/components/shared";

function slugify(value: string) {
  return value
    .trim()
    .toLowerCase()
    .replace(/[^a-z0-9]+/g, "-")
    .replace(/(^-|-$)+/g, "");
}

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось завершить настройку.";
}

export type OnboardingOrganizationStepProps = {
  orgName: string;
  onOrgNameChange: (value: string) => void;
  orgSlug: string;
  onOrgSlugChange: (value: string) => void;
  onBack: () => void;
  onComplete: () => void;
  isPending: boolean;
  error: unknown;
};

export function OnboardingOrganizationStep({
  orgName,
  onOrgNameChange,
  orgSlug,
  onOrgSlugChange,
  onBack,
  onComplete,
  isPending,
  error,
}: OnboardingOrganizationStepProps) {
  const [slugTouched, setSlugTouched] = useState(false);

  useEffect(() => {
    if (slugTouched) return;
    onOrgSlugChange(slugify(orgName));
  }, [orgName, slugTouched, onOrgSlugChange]);

  const canSubmit =
    orgName.trim().length > 0 && orgSlug.trim().length >= 3 && !isPending;

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === "Enter" && canSubmit) {
      onComplete();
    }
  };

  return (
    <div>
      <div className="space-y-4">
        <label className="block">
          <div className="text-sm font-medium text-zinc-700">
            Название организации
          </div>
          <Input
            value={orgName}
            onChange={(e) => onOrgNameChange(e.currentTarget.value)}
            onKeyDown={handleKeyDown}
            placeholder="Например: My Company"
            className="mt-2"
            autoFocus
          />
        </label>

        <label className="block">
          <div className="text-sm font-medium text-zinc-700">Slug</div>
          <Input
            value={orgSlug}
            onChange={(e) => {
              setSlugTouched(true);
              onOrgSlugChange(e.currentTarget.value);
            }}
            onKeyDown={handleKeyDown}
            placeholder="my-company"
            className="mt-2"
          />
          <div className="mt-1.5 text-xs text-zinc-400">
            Будет использоваться в URL:{" "}
            <span className="font-mono">
              /app/organizations/{orgSlug || "slug"}
            </span>
          </div>
        </label>

        {error ? (
          <div className="rounded-xl border border-red-200 bg-red-50 p-3 text-sm text-red-700">
            {getErrorMessage(error)}
          </div>
        ) : null}
      </div>

      <div className="mt-6 flex flex-col-reverse gap-2 sm:flex-row sm:justify-end">
        <Button onClick={onBack} disabled={isPending}>
          Назад
        </Button>
        <Button
          variant="primary"
          onClick={onComplete}
          disabled={!canSubmit}
        >
          {isPending ? "Создаём..." : "Завершить"}
        </Button>
      </div>
    </div>
  );
}
