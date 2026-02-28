"use client";

import { AuthCard } from "@/components/app/auth/auth-card";
import { GoogleAuthButton } from "@/components/app/auth/google-auth-button";
import { useGoogleAuth } from "@/hooks/useGoogleAuth";
import { useSearchParams } from "next/navigation";

export function AuthScreen() {
  const searchParams = useSearchParams();
  const returnTo = searchParams.get("returnTo");
  const { startGoogleAuth, isLoading, error } = useGoogleAuth({ returnTo });

  return (
    <div className="min-h-screen bg-zinc-50 px-6 py-10">
      <div className="mx-auto flex min-h-[calc(100vh-5rem)] w-full max-w-6xl items-center justify-center">
        <AuthCard
          title="Вход в enma"
          description="Один шаг до аналитики. Продолжите с помощью Google."
        >
          <GoogleAuthButton onClick={startGoogleAuth} isLoading={isLoading} />

          {error ? (
            <p className="mt-3 text-center text-sm text-red-600">{error}</p>
          ) : null}

          <p className="mt-5 text-center text-xs text-zinc-400">
            Продолжая, вы соглашаетесь с условиями использования и политикой конфиденциальности.
          </p>
        </AuthCard>
      </div>
    </div>
  );
}
