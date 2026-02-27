import Link from "next/link";
import { footerColumns } from "@/components/landing/content";
import { EnmaLogo } from "@/components/shared/enma-logo";

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

function InstagramIcon() {
  return (
    <svg viewBox="0 0 24 24" aria-hidden="true" className="h-5 w-5" fill="none">
      <rect
        x="4"
        y="4"
        width="16"
        height="16"
        rx="5"
        stroke="currentColor"
        strokeWidth="2"
      />
      <circle cx="12" cy="12" r="4" stroke="currentColor" strokeWidth="2" />
      <circle cx="17" cy="7" r="1.2" fill="currentColor" />
    </svg>
  );
}

function YouTubeIcon() {
  return (
    <svg viewBox="0 0 24 24" aria-hidden="true" className="h-5 w-5" fill="none">
      <rect
        x="3"
        y="6"
        width="18"
        height="12"
        rx="3"
        stroke="currentColor"
        strokeWidth="2"
      />
      <path
        d="M10 9L15 12L10 15V9Z"
        fill="currentColor"
        stroke="currentColor"
        strokeWidth="1"
        strokeLinejoin="round"
      />
    </svg>
  );
}

function LinkedInIcon() {
  return (
    <svg viewBox="0 0 24 24" aria-hidden="true" className="h-5 w-5" fill="none">
      <rect
        x="3"
        y="3"
        width="18"
        height="18"
        rx="3"
        stroke="currentColor"
        strokeWidth="2"
      />
      <path
        d="M8 10V16M8 8.1V8M12 16V12.9C12 11.8 12.9 11 14 11C15.1 11 16 11.8 16 12.9V16"
        stroke="currentColor"
        strokeWidth="2"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  );
}

const socialLinks = [
  { href: "#", label: "X", icon: XIcon },
  { href: "#", label: "Instagram", icon: InstagramIcon },
  { href: "#", label: "YouTube", icon: YouTubeIcon },
  { href: "#", label: "LinkedIn", icon: LinkedInIcon },
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
                <Icon />
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
