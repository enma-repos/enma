"use client";

import { useEffect, useRef } from "react";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import AuthService from "@/services/auth/authService";
import { AuthCard } from "@/components/app/auth/auth-card";

function buildReturnTo(pathname: string, searchParams: URLSearchParams) {
  const query = searchParams.toString();
  return query ? `${pathname}?${query}` : pathname;
}

export function AuthRefreshGate() {
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();
  const startedRef = useRef(false);

  useEffect(() => {
    if (startedRef.current) return;
    startedRef.current = true;

    const run = async () => {
      try {
        await new AuthService().refresh();
        router.refresh();
      } catch {
        const returnTo = buildReturnTo(pathname, searchParams);
        window.location.assign(`/auth?returnTo=${encodeURIComponent(returnTo)}`);
      }
    };

    void run();
  }, [pathname, router, searchParams]);

  return (
    <div className="min-h-screen bg-zinc-50 px-6 py-10">
      <div className="mx-auto flex min-h-[calc(100vh-5rem)] w-full max-w-6xl items-center justify-center">
        <AuthCard
          title="Восстанавливаем сессию"
          description="Это займёт пару секунд. Если не получится — попросим войти заново."
        >
          <div className="flex items-center justify-center gap-3 rounded-xl border border-zinc-200 bg-zinc-50 px-4 py-3 text-sm text-zinc-600">
            <span className="h-4 w-4 animate-spin rounded-full border-2 border-zinc-300 border-t-zinc-900" />
            Проверяем токены…
          </div>
        </AuthCard>
      </div>
    </div>
  );
}

