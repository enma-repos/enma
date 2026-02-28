import type { ReactNode } from "react";
import { headers } from "next/headers";
import { redirect } from "next/navigation";
import { AppShell } from "@/components/app/app-shell";
import { AuthRefreshGate } from "@/components/app/auth/auth-refresh-gate";

const DEFAULT_API_GATEWAY_URL = "http://localhost:8080";

function getApiGatewayUrl() {
  return process.env.NEXT_PUBLIC_API_GATEWAY_URL ?? DEFAULT_API_GATEWAY_URL;
}

export default async function Layout({ children }: { children: ReactNode }) {
  const apiGatewayUrl = getApiGatewayUrl();
  const meUrl = new URL("/api/auth/v1/me", apiGatewayUrl);
  const requestHeaders = await headers();
  const cookieHeader = requestHeaders.get("cookie") ?? "";

  let response: Response;
  try {
    response = await fetch(meUrl, {
      method: "GET",
      headers: {
        accept: "application/json",
        cookie: cookieHeader,
      },
      cache: "no-store",
    });
  } catch {
    return (
      <div className="min-h-screen bg-zinc-50 px-6 py-10">
        <div className="mx-auto flex min-h-[calc(100vh-5rem)] w-full max-w-6xl items-center justify-center">
          <div className="w-full max-w-md rounded-2xl border border-zinc-200 bg-white p-7 shadow-sm">
            <h1 className="text-xl font-semibold text-zinc-900">Ошибка подключения</h1>
            <p className="mt-2 text-sm text-zinc-500">
              Не удалось связаться с сервером. Попробуйте обновить страницу.
            </p>
          </div>
        </div>
      </div>
    );
  }

  if (response.status === 200) {
    return <div>{children}</div>;
  }

  if (response.status === 403) {
    redirect("/forbidden");
  }

  if (response.status === 401) {
    return <AuthRefreshGate />;
  }

  return (
    <div className="min-h-screen bg-zinc-50 px-6 py-10">
      <div className="mx-auto flex min-h-[calc(100vh-5rem)] w-full max-w-6xl items-center justify-center">
        <div className="w-full max-w-md rounded-2xl border border-zinc-200 bg-white p-7 shadow-sm">
          <h1 className="text-xl font-semibold text-zinc-900">Ошибка</h1>
          <p className="mt-2 text-sm text-zinc-500">
            Не удалось проверить авторизацию. Код ответа: {response.status}.
          </p>
        </div>
      </div>
    </div>
  );
}
