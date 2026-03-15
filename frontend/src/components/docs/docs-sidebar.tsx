"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { cn } from "@/components/shared/cn";
import { docsSidebarSections } from "./docs-content";

export function DocsSidebar() {
  const pathname = usePathname();

  return (
    <aside className="hidden w-64 shrink-0 border-r border-zinc-200 bg-white lg:block">
      <nav className="sticky top-14 h-[calc(100vh-3.5rem)] overflow-y-auto px-4 py-6">
        {docsSidebarSections.map((section) => (
          <div key={section.title} className="mb-6">
            <h4 className="mb-2 px-2 text-xs font-semibold uppercase tracking-wider text-zinc-500">
              {section.title}
            </h4>
            <ul className="space-y-0.5">
              {section.items.map((item) => (
                <li key={item.href}>
                  <Link
                    href={item.href}
                    className={cn(
                      "block rounded-lg px-2 py-1.5 text-sm transition-colors",
                      pathname === item.href
                        ? "bg-zinc-100 font-medium text-zinc-900"
                        : "text-zinc-600 hover:bg-zinc-50 hover:text-zinc-900",
                    )}
                  >
                    {item.title}
                  </Link>
                </li>
              ))}
            </ul>
          </div>
        ))}
      </nav>
    </aside>
  );
}
