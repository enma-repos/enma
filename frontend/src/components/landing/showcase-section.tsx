import { Image } from "lucide-react";

export function ShowcaseSection() {
  return (
    <section className="mx-auto w-full max-w-7xl px-5 py-12 sm:px-8 sm:py-14">
      <div className="grid gap-6 md:grid-cols-2">
        {[1, 2].map((item) => (
          <div
            key={item}
            className="flex h-56 items-center justify-center border border-zinc-200 bg-zinc-100 sm:h-64"
          >
            <Image
              className="h-24 w-24 text-zinc-300"
              aria-hidden="true"
              strokeWidth={1}
            />
          </div>
        ))}
      </div>
    </section>
  );
}
