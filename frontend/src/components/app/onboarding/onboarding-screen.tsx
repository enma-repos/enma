"use client";

import { useCallback, useState } from "react";
import { Card, EnmaLogo } from "@/components/shared";
import { OnboardingStepper } from "@/components/app/onboarding/onboarding-stepper";
import { OnboardingProfileStep } from "@/components/app/onboarding/onboarding-profile-step";
import { OnboardingOrganizationStep } from "@/components/app/onboarding/onboarding-organization-step";
import { useOnboarding } from "@/hooks/useOnboarding";

function getDefaults() {
  if (typeof window === "undefined") {
    return { locale: "en", timezone: "UTC" };
  }

  return {
    locale: navigator.language ?? "en",
    timezone:
      Intl.DateTimeFormat().resolvedOptions().timeZone ?? "UTC",
  };
}

export function OnboardingScreen() {
  const [step, setStep] = useState(0);
  const { locale, timezone } = getDefaults();

  const [displayName, setDisplayName] = useState("");
  const [avatarUrl, setAvatarUrl] = useState("");
  const [orgName, setOrgName] = useState("");
  const [orgSlug, setOrgSlug] = useState("");

  const { complete, isPending, error } = useOnboarding();

  const handleContinue = useCallback(() => {
    setStep(1);
  }, []);

  const handleBack = useCallback(() => {
    setStep(0);
  }, []);

  const handleOrgSlugChange = useCallback((value: string) => {
    setOrgSlug(value);
  }, []);

  const handleComplete = useCallback(async () => {
    await complete({
      displayName: displayName.trim(),
      avatarUrl: avatarUrl.trim() || null,
      locale,
      timezone,
      organizationName: orgName.trim(),
      organizationSlug: orgSlug.trim(),
    });
  }, [complete, displayName, avatarUrl, locale, timezone, orgName, orgSlug]);

  return (
    <div className="min-h-screen bg-zinc-50 px-6 py-10">
      <div className="mx-auto flex min-h-[calc(100vh-5rem)] w-full max-w-6xl items-center justify-center">
        <Card className="w-full max-w-lg p-7">
          <div className="flex items-center justify-center gap-2">
            <EnmaLogo
              className="h-8 w-10 text-zinc-900"
              aria-hidden="true"
            />
            <div className="text-lg font-semibold tracking-tight">
              enma
            </div>
          </div>

          <div className="mt-6">
            <OnboardingStepper currentStep={step} />
          </div>

          <div className="mt-6 text-center">
            <h1 className="text-xl font-semibold text-zinc-900">
              {step === 0
                ? "Расскажите о себе"
                : "Создайте организацию"}
            </h1>
            <p className="mt-2 text-sm text-zinc-500">
              {step === 0
                ? "Укажите имя, которое будут видеть коллеги."
                : "Организация объединяет проекты и участников."}
            </p>
          </div>

          <div className="mt-6">
            {step === 0 ? (
              <OnboardingProfileStep
                displayName={displayName}
                onDisplayNameChange={setDisplayName}
                avatarUrl={avatarUrl}
                onAvatarUrlChange={setAvatarUrl}
                onContinue={handleContinue}
              />
            ) : (
              <OnboardingOrganizationStep
                orgName={orgName}
                onOrgNameChange={setOrgName}
                orgSlug={orgSlug}
                onOrgSlugChange={handleOrgSlugChange}
                onBack={handleBack}
                onComplete={handleComplete}
                isPending={isPending}
                error={error}
              />
            )}
          </div>
        </Card>
      </div>
    </div>
  );
}
