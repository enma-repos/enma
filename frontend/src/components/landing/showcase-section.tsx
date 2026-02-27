function ImagePlaceholderIcon() {
  return (
    <svg
      viewBox="0 0 160 120"
      aria-hidden="true"
      className="h-24 w-32 text-zinc-300"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
    >
      <rect
        x="16"
        y="16"
        width="128"
        height="88"
        rx="10"
        stroke="currentColor"
        strokeWidth="6"
      />
      <path
        d="M30 88L62 56L84 78L101 63L130 88"
        stroke="currentColor"
        strokeWidth="6"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
      <circle cx="110" cy="44" r="10" stroke="currentColor" strokeWidth="6" />
    </svg>
  );
}

export function ShowcaseSection() {
  return (
    <section className="mx-auto w-full max-w-7xl px-5 py-12 sm:px-8 sm:py-14">
      <div className="grid gap-6 md:grid-cols-2">
        {[1, 2].map((item) => (
          <div
            key={item}
            className="flex h-56 items-center justify-center border border-zinc-200 bg-zinc-100 sm:h-64"
          >
            <ImagePlaceholderIcon />
          </div>
        ))}
      </div>
    </section>
  );
}
