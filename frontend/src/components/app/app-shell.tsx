import type { ReactNode } from "react";
import { AppSidebar } from "@/components/app/app-sidebar";
import { AppTopbarContainer } from "@/components/app/app-topbar-container";

export type AppShellProps = {
  children: ReactNode;
};

export function AppShell({ children }: AppShellProps) {
  return (
    <div className="min-h-screen bg-zinc-50 text-zinc-900">
      <div className="flex min-h-screen">
        <AppSidebar />
        <div className="flex min-w-0 flex-1 flex-col">
          <AppTopbarContainer />
          <main className="w-full px-6 py-6">{children}</main>
        </div>
      </div>
    </div>
  );
}
