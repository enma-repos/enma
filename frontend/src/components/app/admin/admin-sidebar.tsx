"use client";

import type { ReactNode } from "react";
import Link from "next/link";
import { usePathname } from "next/navigation";
import {
  ArrowLeft,
  BarChart3,
  Boxes,
  Building2,
  ClipboardList,
  LayoutDashboard,
  Users,
} from "lucide-react";
import { EnmaLogo, cn } from "@/components/shared";

type NavItem = {
  href: string;
  label: string;
  icon: ReactNode;
  matchPrefix?: string;
  disabled?: boolean;
};

const NAV_ITEMS: NavItem[] = [
  {
    href: "/admin",
    label: "Обзор",
    icon: <LayoutDashboard className="h-5 w-5" />,
  },
  {
    href: "/admin/users",
    label: "Пользователи",
    icon: <Users className="h-5 w-5" />,
    matchPrefix: "/admin/users",
  },
  {
    href: "/admin/organizations",
    label: "Организации",
    icon: <Building2 className="h-5 w-5" />,
    matchPrefix: "/admin/organizations",
  },
  {
    href: "/admin/projects",
    label: "Проекты",
    icon: <Boxes className="h-5 w-5" />,
    matchPrefix: "/admin/projects",
  },
  {
    href: "/admin/audit-logs",
    label: "Аудит-логи",
    icon: <ClipboardList className="h-5 w-5" />,
    matchPrefix: "/admin/audit-logs",
  },
  {
    href: "#",
    label: "Аналитика",
    icon: <BarChart3 className="h-5 w-5" />,
    disabled: true,
  },
];

function isActive(pathname: string, item: NavItem) {
  if (item.disabled) return false;
  if (item.matchPrefix) return pathname.startsWith(item.matchPrefix);
  return pathname === item.href;
}

export function AdminSidebar() {
  const pathname = usePathname() ?? "";

  return (
    <aside className="hidden w-64 shrink-0 border-r border-zinc-200 bg-white lg:flex lg:flex-col">
      <div className="flex h-20 items-end justify-center pr-2 pb-8">
        <Link href="/admin">
          <EnmaLogo className="h-7 text-zinc-900" aria-hidden="true" />
        </Link>
      </div>

      <nav className="flex-1 px-3 pb-6">
        <div className="space-y-1">
          {NAV_ITEMS.map((item) => {
            const active = isActive(pathname, item);
            const baseClasses =
              "flex items-center gap-3 rounded-xl px-3 py-2 text-sm font-medium transition-colors";

            if (item.disabled) {
              return (
                <div
                  key={item.label}
                  aria-disabled="true"
                  title="Скоро"
                  className={cn(
                    baseClasses,
                    "cursor-not-allowed pointer-events-none text-zinc-300",
                  )}
                >
                  <span className="text-zinc-300">{item.icon}</span>
                  <span className="truncate">{item.label}</span>
                  <span className="ml-auto text-[10px] uppercase tracking-wide text-zinc-300">
                    Скоро
                  </span>
                </div>
              );
            }

            return (
              <Link
                key={item.label}
                href={item.href}
                className={cn(
                  baseClasses,
                  active
                    ? "bg-zinc-100 text-zinc-900"
                    : "text-zinc-600 hover:bg-zinc-50 hover:text-zinc-900",
                )}
              >
                <span className="text-zinc-500">{item.icon}</span>
                <span className="truncate">{item.label}</span>
              </Link>
            );
          })}
        </div>
      </nav>

      <div className="border-t border-zinc-100 px-3 py-4">
        <Link
          href="/app"
          className="flex items-center gap-3 rounded-xl px-3 py-2 text-sm font-medium text-zinc-600 transition-colors hover:bg-zinc-50 hover:text-zinc-900"
        >
          <ArrowLeft className="h-5 w-5 text-zinc-500" />
          <span className="truncate">Вернуться в приложение</span>
        </Link>
      </div>
    </aside>
  );
}
