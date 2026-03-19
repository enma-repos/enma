import Link from "next/link";
import { EnmaLogo } from "@/components/shared/enma-logo";
import { SocialLink } from "@/components/shared/social-link";
import { Github } from "lucide-react";

const footerLinks = [
  { href: "/product", label: "Product" },
  { href: "/sdk", label: "SDK" },
  { href: "/docs", label: "Documentation" },
  { href: "/contact", label: "Contact" },
];

export function LandingFooter() {
  return (
    <footer className="border-t border-zinc-200 bg-zinc-100">
      <div className="mx-auto flex w-full max-w-7xl flex-col gap-8 px-5 py-10 sm:px-8">
        <div className="flex flex-col items-start justify-between gap-6 sm:flex-row sm:items-center">
          <Link href="/" aria-label="Go to enma homepage">
            <EnmaLogo className="h-7 text-zinc-900" />
          </Link>

          <div className="flex items-center gap-1">
            <nav className="flex flex-wrap items-center gap-1 text-sm font-medium">
              {footerLinks.map(({ href, label }) => (
                <Link
                  key={label}
                  href={href}
                  className="rounded-lg px-3 py-2 text-zinc-600 transition-colors hover:bg-zinc-200 hover:text-zinc-900"
                >
                  {label}
                </Link>
              ))}
            </nav>

            <SocialLink
              href="https://github.com/enma-repos"
              icon={Github}
              label="GitHub"
            />
          </div>
        </div>

        <p className="text-sm text-zinc-500">
          &copy; {new Date().getFullYear()} enma. All rights reserved.
        </p>
      </div>
    </footer>
  );
}
