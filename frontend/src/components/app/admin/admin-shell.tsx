import type { ReactNode } from "react";
import { AdminSidebar } from "@/components/app/admin/admin-sidebar";
import { AdminTopbar } from "@/components/app/admin/admin-topbar";

export type AdminShellProps = {
  children: ReactNode;
};

export function AdminShell({ children }: AdminShellProps) {
  return (
    <div className="min-h-screen bg-white text-zinc-900">
      <div className="flex min-h-screen">
        <AdminSidebar />
        <div className="flex min-w-0 flex-1 flex-col">
          <AdminTopbar />
          <main className="w-full px-6 py-6">{children}</main>
        </div>
      </div>
    </div>
  );
}
