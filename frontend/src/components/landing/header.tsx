"use client";

import Link from "next/link";
import { navigationLinks } from "@/components/landing/content";
import { EnmaLogo } from "@/components/shared/enma-logo";
import {usePathname} from "next/navigation";

export function LandingHeader() {
  const currentHref = usePathname();

  return (
    <header className="sticky top-0 z-40 border-b border-zinc-200 bg-zinc-50/95 backdrop-blur">
      <div className="mx-auto flex h-16 w-full max-w-7xl items-center justify-between px-5 sm:px-8">
        <Link href="/" aria-label="Go to enma homepage">
          <EnmaLogo className="h-8 w-10 text-zinc-900" />
        </Link>

        <div className="flex items-center gap-5">
          <nav className="hidden items-center gap-1 text-sm font-medium text-zinc-700 md:flex">
            {navigationLinks.map((item) => (
              <Link
                key={item.label}
                href={item.href}
                className={
                  currentHref === item.href
                      ? "rounded-lg bg-zinc-100 text-zinc-900 px-3 py-2 text-sm font-medium "
                      : "rounded-lg text-zinc-600 hover:bg-zinc-100 hover:text-zinc-900 px-3 py-2 text-sm font-medium"
                }
              >
                {item.label}
              </Link>
            ))}
          </nav>

          <div className="flex items-center gap-2">
            <Link
              href="/auth"
              className="rounded-lg border border-zinc-300 bg-zinc-100 px-3 py-2 text-sm font-medium text-zinc-700 transition-colors hover:bg-zinc-200"
            >
              Sign in
            </Link>

          </div>
        </div>
      </div>
    </header>
  );
}
