"use client";

import type { ReactNode } from "react";
import { useMemo } from "react";
import Link from "next/link";
import { usePathname } from "next/navigation";
import {
  EnmaLogo,
  IconBolt,
  IconBox,
  IconChart,
  IconGear,
  IconHome,
  IconList,
  IconUsers,
  cn,
} from "@/components/shared";

type NavItem = {
  href: string;
  label: string;
  icon: ReactNode;
};

function parseProjectBase(pathname: string): string | null {
  const match = pathname.match(
    /^\/app\/organizations\/([^/]+)\/projects\/([^/]+)/,
  );
  if (!match) return null;
  return `/app/organizations/${match[1]}/projects/${match[2]}`;
}

function buildNavItems(base: string) {
  const primary: NavItem[] = [
    { href: base, label: "Главная", icon: <IconHome className="h-5 w-5" /> },
  ];

  const analytics: NavItem[] = [
    { href: `${base}/analytics/summary`, label: "Сводка", icon: <IconList className="h-5 w-5" /> },
    { href: `${base}/analytics/paths`, label: "Пути", icon: <IconList className="h-5 w-5" /> },
    { href: `${base}/analytics/users`, label: "Пользователи", icon: <IconUsers className="h-5 w-5" /> },
  ];

  const product: NavItem[] = [
    { href: `${base}/events`, label: "События", icon: <IconBolt className="h-5 w-5" /> },
    { href: `${base}/apps`, label: "Приложения", icon: <IconBox className="h-5 w-5" /> },
    { href: `${base}/processes`, label: "Процессы", icon: <IconGear className="h-5 w-5" /> },
  ];

  const footer: NavItem[] = [
    { href: "/docs", label: "Documentation", icon: <IconList className="h-5 w-5" /> },
    { href: `${base}/access`, label: "Manage access", icon: <IconUsers className="h-5 w-5" /> },
    { href: `${base}/logs`, label: "Logs", icon: <IconList className="h-5 w-5" /> },
  ];

  return { primary, analytics, product, footer };
}

export type AppSidebarProps = {
  activeHref?: string;
};

export function AppSidebar({ activeHref }: AppSidebarProps) {
  const pathname = usePathname();
  const base = useMemo(() => parseProjectBase(pathname ?? ""), [pathname]);
  const nav = useMemo(() => buildNavItems(base ?? ""), [base]);

  const currentHref = activeHref ?? pathname ?? "";
  const isAnalyticsActive = base ? currentHref.startsWith(`${base}/analytics`) : false;

  return (
    <aside className="hidden w-72 shrink-0 border-r border-zinc-200 bg-white lg:block">
      <div className="flex h-16 items-center gap-3 px-5">
        <EnmaLogo className="h-8 w-10 text-zinc-900" aria-hidden="true" />
        <span className="text-lg font-semibold tracking-tight">enma</span>
      </div>

      <nav className="px-3 pb-6">
        <div className="mt-2 space-y-1">
          {nav.primary.map((item) => (
            <Link
              key={item.href}
              href={item.href}
              className={cn(
                "flex items-center gap-3 rounded-xl px-3 py-2 text-sm font-medium transition-colors",
                currentHref === item.href
                  ? "bg-zinc-100 text-zinc-900"
                  : "text-zinc-600 hover:bg-zinc-50 hover:text-zinc-900",
              )}
            >
              <span className="text-zinc-500">{item.icon}</span>
              <span className="truncate">{item.label}</span>
            </Link>
          ))}
        </div>

        <div className="mt-4">
          <Link
            href={base ? `${base}/analytics/summary` : ""}
            className={cn(
              "flex items-center gap-3 rounded-xl px-3 py-2 text-sm font-medium transition-colors",
              isAnalyticsActive
                ? "bg-zinc-100 text-zinc-900"
                : "text-zinc-600 hover:bg-zinc-50 hover:text-zinc-900",
            )}
          >
            <span className="text-zinc-500">
              <IconChart className="h-5 w-5" />
            </span>
            <span className="truncate">Аналитика</span>
          </Link>

          <div className="mt-2 space-y-1 pl-2">
            {nav.analytics.map((item) => (
              <Link
                key={item.href}
                href={item.href}
                className={cn(
                  "flex items-center gap-3 rounded-xl px-3 py-2 text-sm font-medium transition-colors",
                  currentHref === item.href
                    ? "bg-zinc-100 text-zinc-900"
                    : "text-zinc-600 hover:bg-zinc-50 hover:text-zinc-900",
                )}
              >
                <span className="text-zinc-500">{item.icon}</span>
                <span className="truncate">{item.label}</span>
              </Link>
            ))}
          </div>
        </div>

        <div className="mt-4 space-y-1 border-t border-zinc-100 pt-4">
          {nav.product.map((item) => (
            <Link
              key={item.href}
              href={item.href}
              className={cn(
                "flex items-center gap-3 rounded-xl px-3 py-2 text-sm font-medium transition-colors",
                currentHref === item.href
                  ? "bg-zinc-100 text-zinc-900"
                  : "text-zinc-600 hover:bg-zinc-50 hover:text-zinc-900",
              )}
            >
              <span className="text-zinc-500">{item.icon}</span>
              <span className="truncate">{item.label}</span>
            </Link>
          ))}
        </div>

        <div className="mt-6 space-y-1 border-t border-zinc-100 pt-4">
          {nav.footer.map((item) => (
            <Link
              key={item.href}
              href={item.href}
              className={cn(
                "flex items-center gap-3 rounded-xl px-3 py-2 text-sm font-medium transition-colors",
                currentHref === item.href
                  ? "bg-zinc-100 text-zinc-900"
                  : "text-zinc-600 hover:bg-zinc-50 hover:text-zinc-900",
              )}
            >
              <span className="text-zinc-500">{item.icon}</span>
              <span className="truncate">{item.label}</span>
            </Link>
          ))}
        </div>
      </nav>
    </aside>
  );
}
