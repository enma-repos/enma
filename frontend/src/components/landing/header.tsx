"use client";

import { useState } from "react";
import Link from "next/link";
import { navigationLinks } from "@/components/landing/content";
import { EnmaLogoIcon } from "@/components/shared/enma-logo-icon";
import { usePathname } from "next/navigation";

export function LandingHeader() {
  const currentHref = usePathname();
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);

  return (
    <header className="sticky top-0 z-40 border-b border-zinc-200 bg-zinc-50/95 backdrop-blur">
      <div className="mx-auto flex h-16 w-full max-w-7xl items-center justify-between px-5 sm:px-8">
        <Link href="/" aria-label="Go to enma homepage">
          <EnmaLogoIcon className="h-7 text-zinc-900" />
        </Link>

        <div className="flex items-center gap-5">
          <nav className="hidden items-center gap-1 text-sm font-medium text-zinc-700 md:flex">
            {navigationLinks.map((item) => (
              <Link
                key={item.label}
                href={item.href}
                className={
                  currentHref === item.href
                    ? "rounded-lg bg-zinc-100 text-zinc-900 px-3 py-2 text-sm font-medium"
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

            <button
              type="button"
              onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
              className="inline-flex items-center justify-center rounded-lg p-2 text-zinc-600 hover:bg-zinc-100 hover:text-zinc-900 md:hidden"
              aria-label="Toggle menu"
            >
              {mobileMenuOpen ? (
                <svg className="h-5 w-5" fill="none" viewBox="0 0 24 24" strokeWidth={2} stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" d="M6 18L18 6M6 6l12 12" />
                </svg>
              ) : (
                <svg className="h-5 w-5" fill="none" viewBox="0 0 24 24" strokeWidth={2} stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" d="M3.75 9h16.5m-16.5 6.75h16.5" />
                </svg>
              )}
            </button>
          </div>
        </div>
      </div>

      {mobileMenuOpen && (
        <nav className="border-t border-zinc-200 bg-zinc-50 px-5 pb-4 pt-2 md:hidden">
          <div className="flex flex-col gap-1">
            {navigationLinks.map((item) => (
              <Link
                key={item.label}
                href={item.href}
                onClick={() => setMobileMenuOpen(false)}
                className={
                  currentHref === item.href
                    ? "rounded-lg bg-zinc-100 text-zinc-900 px-3 py-2 text-sm font-medium"
                    : "rounded-lg text-zinc-600 hover:bg-zinc-100 hover:text-zinc-900 px-3 py-2 text-sm font-medium"
                }
              >
                {item.label}
              </Link>
            ))}
          </div>
        </nav>
      )}
    </header>
  );
}
