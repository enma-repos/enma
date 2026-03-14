"use client";

import { Check } from "lucide-react";

const steps = ["Профиль", "Организация"];

export type OnboardingStepperProps = {
  currentStep: number;
};

export function OnboardingStepper({ currentStep }: OnboardingStepperProps) {
  return (
    <div className="flex items-center justify-center gap-3">
      {steps.map((label, index) => {
        const isCompleted = index < currentStep;
        const isActive = index === currentStep;

        return (
          <div key={label} className="flex items-center gap-3">
            {index > 0 && (
              <div
                className={`h-px w-10 ${
                  isCompleted ? "bg-zinc-900" : "bg-zinc-200"
                }`}
              />
            )}

            <div className="flex items-center gap-2">
              <div
                className={`flex h-7 w-7 items-center justify-center rounded-full text-xs font-medium ${
                  isCompleted || isActive
                    ? "bg-zinc-900 text-white"
                    : "border border-zinc-300 text-zinc-400"
                }`}
              >
                {isCompleted ? (
                  <Check className="h-3.5 w-3.5" strokeWidth={3} />
                ) : (
                  index + 1
                )}
              </div>
              <span
                className={`text-sm ${
                  isCompleted || isActive
                    ? "font-medium text-zinc-900"
                    : "text-zinc-400"
                }`}
              >
                {label}
              </span>
            </div>
          </div>
        );
      })}
    </div>
  );
}
