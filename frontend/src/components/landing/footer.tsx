import Link from "next/link";
import { footerColumns } from "@/components/landing/content";
import { EnmaLogo } from "@/components/shared/enma-logo";
import { Instagram, Linkedin, Youtube } from "lucide-react";

function XIcon() {
  return (
    <svg viewBox="0 0 24 24" aria-hidden="true" className="h-5 w-5" fill="none">
      <path
        d="M4 4L20 20M20 4L4 20"
        stroke="currentColor"
        strokeWidth="2"
        strokeLinecap="round"
      />
    </svg>
  );
}

const socialLinks = [
  { href: "#", label: "X", icon: XIcon },
  { href: "#", label: "Instagram", icon: Instagram },
  { href: "#", label: "YouTube", icon: Youtube },
  { href: "#", label: "LinkedIn", icon: Linkedin },
];

export function LandingFooter() {
  return (
    <footer className="border-t border-zinc-200 bg-zinc-100">
      <div className="mx-auto grid w-full max-w-7xl gap-10 px-5 py-10 sm:px-8 md:grid-cols-2 lg:grid-cols-4">
        <div className="space-y-5">
          <Link href="/" aria-label="Go to enma homepage">
            <EnmaLogo className="h-8 w-10 text-zinc-900" />
          </Link>

          <div className="flex items-center gap-3 text-zinc-800">
            {socialLinks.map(({ href, label, icon: Icon }) => (
              <Link
                key={label}
                href={href}
                aria-label={label}
                className="rounded-md p-1 transition-colors hover:bg-zinc-200"
              >
                <Icon className="h-5 w-5" />
              </Link>
            ))}
          </div>
        </div>

        {footerColumns.map((column) => (
          <div key={column.title}>
            <h3 className="text-lg font-semibold text-zinc-900">{column.title}</h3>
            <ul className="mt-4 space-y-2 text-zinc-700">
              {column.links.map((label) => (
                <li key={label}>
                  <Link href="#" className="transition-colors hover:text-zinc-950">
                    {label}
                  </Link>
                </li>
              ))}
            </ul>
          </div>
        ))}
      </div>
    </footer>
  );
}
