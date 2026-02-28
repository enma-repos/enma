"use client";

import { useCallback, useState } from "react";
import ExternalAuthService from "@/services/auth/externalAuthService";

type UseGoogleAuthResult = {
  startGoogleAuth: () => Promise<void>;
  isLoading: boolean;
  error: string | null;
};

type UseGoogleAuthOptions = {
  returnTo?: string | null;
};

const DEFAULT_SUCCESS_PATH = "/app/organizations";

function normalizeReturnTo(returnTo?: string | null) {
  if (!returnTo) return DEFAULT_SUCCESS_PATH;
  if (!returnTo.startsWith("/") || returnTo.startsWith("//")) return DEFAULT_SUCCESS_PATH;
  return returnTo;
}

export function useGoogleAuth(options: UseGoogleAuthOptions = {}): UseGoogleAuthResult {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const startGoogleAuth = useCallback(async () => {
    setIsLoading(true);
    setError(null);

    try {
      const successUrl = normalizeReturnTo(options.returnTo);
      const { url } = await new ExternalAuthService().startGoogleAuth({ successUrl });

      if (!url) throw new Error("Missing redirect url");
      window.location.assign(url);
    } catch {
      setError("Не удалось начать вход через Google. Попробуйте ещё раз.");
      setIsLoading(false);
    }
  }, [options.returnTo]);

  return { startGoogleAuth, isLoading, error };
}
