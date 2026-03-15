"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { EnmaLogo } from "@/components/shared/enma-logo";
import { Avatar, Button, IconChevronDown } from "@/components/shared";
import { cn } from "@/components/shared/cn";
import { useMe } from "@/hooks/useMe";
import { useLogout } from "@/hooks/useLogout";
import { useCallback, useEffect, useRef, useState } from "react";

const navLinks = [
  { href: "/docs", label: "Docs" },
  { href: "/docs/sdk/overview", label: "SDK" },
  { href: "/docs/sdk/rest-api", label: "API Reference" },
];

function getInitials(displayName: string | null) {
  if (!displayName) return "U";
  const parts = displayName.trim().split(/\s+/).filter(Boolean);
  const first = parts[0]?.[0] ?? "";
  const second = parts.length > 1 ? parts[1]?.[0] ?? "" : (parts[0]?.[1] ?? "");
  return (first + second).toUpperCase() || "U";
}

function UserMenu({ displayName }: { displayName: string | null }) {
  const initials = getInitials(displayName);
  const [menuOpen, setMenuOpen] = useState(false);
  const menuRef = useRef<HTMLDivElement>(null);
  const logout = useLogout();

  const toggleMenu = useCallback(() => setMenuOpen((prev) => !prev), []);

  useEffect(() => {
    if (!menuOpen) return;
    function handleClickOutside(e: MouseEvent) {
      if (menuRef.current && !menuRef.current.contains(e.target as Node)) {
        setMenuOpen(false);
      }
    }
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, [menuOpen]);

  return (
    <div className="flex items-center gap-2">
      <div className="relative" ref={menuRef}>
        <Button
          variant="ghost"
          size="sm"
          className="max-w-[220px] cursor-pointer rounded-xl"
          onClick={toggleMenu}
        >
          <span className="truncate text-sm font-medium text-zinc-700">
            {displayName}
          </span>
          <IconChevronDown className="h-4 w-4 text-zinc-500" aria-hidden="true" />
        </Button>

        {menuOpen && (
          <div className="absolute right-0 top-full mt-1 min-w-[160px] rounded-xl border border-zinc-200 bg-white py-1 shadow-lg">
            <Link
              href="/app/organizations"
              className="block w-full px-4 py-2 text-left text-sm text-zinc-700 hover:bg-zinc-100"
              onClick={() => setMenuOpen(false)}
            >
              Dashboard
            </Link>
            <button
              type="button"
              className="w-full cursor-pointer px-4 py-2 text-left text-sm text-zinc-700 hover:bg-zinc-100"
              onClick={() => {
                setMenuOpen(false);
                logout();
              }}
            >
              Logout
            </button>
          </div>
        )}
      </div>

      <Avatar initials={initials} aria-label="User menu" />
    </div>
  );
}

export function DocsHeader() {
  const pathname = usePathname();
  const meQuery = useMe();
  const isAuthenticated = !!meQuery.data?.user;
  const displayName = meQuery.data?.user?.displayName ?? null;

  return (
    <header className="sticky top-0 z-40 border-b border-zinc-200 bg-white/95 backdrop-blur">
      <div className="flex h-14 items-center gap-6 px-6">
        <Link href="/" className="flex items-center gap-2" aria-label="Go to enma homepage">
          <EnmaLogo className="h-6 text-zinc-900" />
        </Link>

        <span className="text-zinc-300">/</span>

        <nav className="flex items-center gap-1 text-sm font-medium">
          {navLinks.map((link) => (
            <Link
              key={link.href}
              href={link.href}
              className={cn(
                "rounded-lg px-3 py-1.5 transition-colors",
                pathname === link.href
                  ? "bg-zinc-100 text-zinc-900"
                  : "text-zinc-600 hover:bg-zinc-50 hover:text-zinc-900",
              )}
            >
              {link.label}
            </Link>
          ))}
        </nav>

        <div className="ml-auto flex items-center gap-3">
          {isAuthenticated ? (
            <UserMenu displayName={displayName} />
          ) : (
            <Link
              href="/auth"
              className="rounded-lg border border-zinc-300 bg-zinc-50 px-3 py-1.5 text-sm font-medium text-zinc-700 transition-colors hover:bg-zinc-100"
            >
              Sign in
            </Link>
          )}
        </div>
      </div>
    </header>
  );
}
