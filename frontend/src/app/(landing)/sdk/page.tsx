import Link from "next/link";
import { Package, Github, ArrowRight } from "lucide-react";

export default function Page() {
  return (
    <div className="min-h-screen bg-zinc-50">
      {/* Hero */}
      <section className="border-b border-zinc-200 bg-white">
        <div className="mx-auto flex max-w-7xl flex-col items-center px-6 py-20 text-center">
          <h1 className="text-5xl font-semibold tracking-tight text-zinc-950 sm:text-6xl">
            enma SDK
          </h1>
          <p className="mt-4 max-w-2xl text-lg leading-relaxed text-zinc-500">
            .NET SDK for sending process events to enma. Integrate event
            tracking into your application with a few lines of code.
          </p>
          <Link
            href="/docs/sdk/overview"
            className="mt-8 inline-flex items-center gap-2 rounded-lg bg-zinc-900 px-5 py-2.5 text-sm font-medium text-zinc-50 transition-colors hover:bg-zinc-700"
          >
            Documentation
            <ArrowRight className="h-4 w-4" />
          </Link>
        </div>
      </section>

      {/* Installation */}
      <section className="mx-auto w-full max-w-7xl px-6 py-16">
        <h2 className="text-3xl font-semibold tracking-tight text-zinc-950">
          Installation
        </h2>
        <p className="mt-2 text-base text-zinc-500">
          Install the SDK via NuGet or build from source on GitHub.
        </p>

        <div className="mt-8 grid gap-6 sm:grid-cols-2">
          {/* NuGet */}
          <div className="rounded-lg border border-zinc-200 bg-white p-6">
            <div className="flex items-center gap-3">
              <Package className="h-6 w-6 text-zinc-800" />
              <h3 className="text-xl font-semibold text-zinc-900">NuGet</h3>
            </div>
            <p className="mt-3 text-sm text-zinc-500">
              Add the package to your .NET project using the CLI.
            </p>
            <pre className="mt-4 overflow-x-auto rounded-md bg-zinc-100 px-4 py-3 text-sm text-zinc-800">
              <code>dotnet add package Enma.Sdk</code>
            </pre>
          </div>

          {/* GitHub */}
          <div className="rounded-lg border border-zinc-200 bg-white p-6">
            <div className="flex items-center gap-3">
              <Github className="h-6 w-6 text-zinc-800" />
              <h3 className="text-xl font-semibold text-zinc-900">GitHub</h3>
            </div>
            <p className="mt-3 text-sm text-zinc-500">
              Clone the repository and build the SDK from source.
            </p>
            <a
              href="https://github.com/enma-repos/enma-sdk"
              target="_blank"
              rel="noopener noreferrer"
              className="mt-4 inline-flex items-center gap-2 rounded-lg border border-zinc-300 bg-zinc-100 px-4 py-2 text-sm font-medium text-zinc-800 transition-colors hover:bg-zinc-200"
            >
              View on GitHub
              <ArrowRight className="h-4 w-4" />
            </a>
          </div>
        </div>
      </section>

      {/* Quick example */}
      <section className="border-t border-zinc-200 bg-white">
        <div className="mx-auto w-full max-w-7xl px-6 py-16">
          <h2 className="text-3xl font-semibold tracking-tight text-zinc-950">
            Quick example
          </h2>
          <p className="mt-2 text-base text-zinc-500">
            Send your first event in just a few lines.
          </p>

          <pre className="mt-8 overflow-x-auto rounded-lg border border-zinc-200 bg-zinc-950 px-6 py-5 text-sm leading-relaxed text-zinc-100">
            <code>{`var client = new EnmaClient("<your-api-key>");

await client.TrackAsync("order_placed", new {
    UserId = "user-42",
    Amount = 99.90
});`}</code>
          </pre>
        </div>
      </section>
    </div>
  );
}
