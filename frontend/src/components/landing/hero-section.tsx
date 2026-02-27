import Link from "next/link";

export function HeroSection() {
  return (
    <section className="relative isolate overflow-hidden border-b border-zinc-200">
      <div
        className="absolute inset-0 bg-cover bg-center opacity-65 grayscale"
        style={{
          backgroundImage:
            "radial-gradient(circle at 30% 10%, #eef1f5 0%, #dde2e8 38%, #f5f6f8 100%), url('/hero-background.jpg')",
        }}
      />
      <div className="absolute inset-0 bg-[linear-gradient(180deg,rgba(255,255,255,0.45),rgba(255,255,255,0.72))]" />

      <div className="relative mx-auto flex min-h-[420px] max-w-7xl flex-col items-center justify-center px-6 py-20 text-center">
        <h1 className="text-6xl font-semibold tracking-tight text-zinc-950 sm:text-7xl">
          enma
        </h1>
        <p className="mt-4 max-w-3xl text-2xl leading-tight text-zinc-800 sm:text-[2rem]">
          The best process-mining solution for your company
        </p>

        <div className="mt-8 flex flex-wrap items-center justify-center gap-3">
          <Link
            href="/docs"
            className="rounded-lg border border-zinc-300 bg-zinc-100 px-5 py-2.5 text-sm font-medium text-zinc-800 transition-colors hover:bg-zinc-200"
          >
            Documentation
          </Link>
          <Link
            href="/auth"
            className="rounded-lg bg-zinc-900 px-5 py-2.5 text-sm font-medium text-zinc-50 transition-colors hover:bg-zinc-700"
          >
            Get started
          </Link>
        </div>
      </div>
    </section>
  );
}
