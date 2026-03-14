"use client";

import { Button, Input } from "@/components/shared";

export type OnboardingProfileStepProps = {
  displayName: string;
  onDisplayNameChange: (value: string) => void;
  avatarUrl: string;
  onAvatarUrlChange: (value: string) => void;
  onContinue: () => void;
};

export function OnboardingProfileStep({
  displayName,
  onDisplayNameChange,
  avatarUrl,
  onAvatarUrlChange,
  onContinue,
}: OnboardingProfileStepProps) {
  const canContinue = displayName.trim().length >= 2;

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === "Enter" && canContinue) {
      onContinue();
    }
  };

  return (
    <div>
      <div className="space-y-4">
        <label className="block">
          <div className="text-sm font-medium text-zinc-700">
            Отображаемое имя
          </div>
          <Input
            value={displayName}
            onChange={(e) => onDisplayNameChange(e.currentTarget.value)}
            onKeyDown={handleKeyDown}
            placeholder="Как вас зовут?"
            className="mt-2"
            autoFocus
          />
          <div className="mt-1.5 text-xs text-zinc-400">
            Минимум 2 символа. Будет видно другим участникам.
          </div>
        </label>
      </div>

      <div className="mt-6 flex justify-end">
        <Button
          variant="primary"
          onClick={onContinue}
          disabled={!canContinue}
        >
          Продолжить
        </Button>
      </div>
    </div>
  );
}
