import Link from "next/link";
import { Github, ArrowRight } from "lucide-react";

const features = [".NET 6+", "Auto-batching", "Zero dependencies"];

export function SdkHeroSection() {
  return (
    <section className="relative isolate overflow-hidden bg-white">
      {/* Grid pattern */}
      <div
        className="absolute inset-0 opacity-[0.25]"
        style={{
          backgroundImage:
            "linear-gradient(#a1a1aa 1px, transparent 1px), linear-gradient(90deg, #a1a1aa 1px, transparent 1px)",
          backgroundSize: "48px 48px",
        }}
      />
      {/* Radial glow */}
      <div
        className="absolute inset-0"
        style={{
          background:
            "radial-gradient(600px circle at 50% 50%, rgba(161,161,170,0.25), transparent 70%)",
        }}
      />
      <div className="absolute inset-0 bg-[linear-gradient(180deg,rgba(255,255,255,0.1)_0%,rgba(249,250,251,1)_100%)]" />

      <div className="relative mx-auto flex max-w-7xl flex-col items-center px-6 pb-52 pt-40 text-center">
        <h1 className="text-6xl font-semibold tracking-tight text-zinc-950 sm:text-7xl">
          Enma SDK
        </h1>
        <p className="mt-4 max-w-3xl text-2xl leading-tight text-zinc-800 sm:text-[2rem]">
          .NET SDK for sending process events to Enma
        </p>

        {/* Feature badges */}
        <div className="mt-6 flex flex-wrap items-center justify-center gap-2">
          {features.map((f, i) => (
            <span
              key={f}
              className="rounded-full border border-blue-200 bg-blue-50 px-3 py-1 text-xs font-medium text-blue-700 opacity-0"
              style={{
                animation: `staggerIn 0.4s ease-out ${0.3 + i * 0.1}s both`,
              }}
            >
              {f}
            </span>
          ))}
        </div>

        <div className="mt-8 flex flex-wrap items-center justify-center gap-3">
          <a
            href="https://github.com/enma-repos/enma-sdk"
            target="_blank"
            rel="noopener noreferrer"
            className="inline-flex items-center gap-2 rounded-lg border border-zinc-300 bg-zinc-100 px-5 py-2.5 text-sm font-medium text-zinc-800 transition-colors hover:bg-zinc-200"
          >
            <Github className="h-4 w-4" />
            GitHub
          </a>
          <Link
            href="/docs/sdk/overview"
            className="inline-flex items-center gap-2 rounded-lg bg-zinc-900 px-5 py-2.5 text-sm font-medium text-zinc-50 transition-colors hover:bg-zinc-700"
          >
            Documentation
            <ArrowRight className="h-4 w-4" />
          </Link>
        </div>
      </div>
    </section>
  );
}
