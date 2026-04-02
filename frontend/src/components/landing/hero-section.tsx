import Link from "next/link";

export function HeroSection() {
  return (
    <section className="relative isolate overflow-hidden ">
      <div
        className="absolute inset-0 bg-cover bg-center opacity-65 grayscale"
        style={{
          backgroundImage:
            "url('/hero-background.png')",
        }}
      />
      <div className="absolute inset-0 bg-[linear-gradient(180deg,rgba(255,255,255,0.45),rgba(255,255,255,0.72))]" />
      <div className="absolute inset-x-0 bottom-0 h-40 bg-gradient-to-b from-transparent to-white" />

      <div className="relative mx-auto flex max-w-7xl flex-col items-center justify-center px-6 pb-52 pt-40 text-center">
        <h1 className="text-6xl font-semibold tracking-tight text-zinc-950 sm:text-7xl">
          Enma
        </h1>
        <p className="mt-4 max-w-3xl text-2xl leading-tight text-zinc-800 sm:text-[2rem]">
          Discover bottlenecks, map real processes, and optimize operations
        </p>

        <div className="mt-8 flex flex-wrap items-center justify-center gap-3">
          <Link
            href="/docs"
            className="rounded-lg border border-zinc-300 bg-zinc-100 px-5 py-2.5 text-sm font-medium text-zinc-800 transition-colors hover:bg-zinc-200"
          >
            Documentation
          </Link>
          <Link
            href="/app/organizations"
            className="rounded-lg bg-zinc-900 px-5 py-2.5 text-sm font-medium text-zinc-50 transition-colors hover:bg-zinc-700"
          >
            Get started
          </Link>
        </div>
      </div>
    </section>
  );
}
