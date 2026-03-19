"use client";

import Link from "next/link";
import { useState } from "react";
import {
  Github,
  ArrowRight,
  Download,
  Settings,
  Radio,
} from "lucide-react";

const features = [
  "Async-first",
  ".NET 6+",
  "Strongly typed",
  "Auto-batching",
  "Zero dependencies",
];

const steps = [
  {
    number: "01",
    title: "Install",
    description: "Add the NuGet package to your .NET project.",
    icon: Download,
    iconColor: "text-blue-500",
    bgColor: "bg-blue-50",
    borderColor: "border-blue-100",
    tabs: [
      { label: ".NET CLI", code: "dotnet add package Enma.Sdk" },
      { label: "Git", code: "git clone https://github.com/enma-repos/enma-sdk.git" },
    ],
  },
  {
    number: "02",
    title: "Configure",
    description: "Initialize the client with your API key.",
    icon: Settings,
    iconColor: "text-amber-500",
    bgColor: "bg-amber-50",
    borderColor: "border-amber-100",
    code: `var client = new EnmaClient(options => {
    options.ApiKey = "<your-api-key>";
    options.ProjectId = "my-project";
    options.BatchSize = 50;
    options.FlushInterval = TimeSpan.FromSeconds(5);
});`,
  },
  {
    number: "03",
    title: "Track",
    description: "Send process events with a single method call.",
    icon: Radio,
    iconColor: "text-emerald-500",
    bgColor: "bg-emerald-50",
    borderColor: "border-emerald-100",
    code: 'await client.TrackAsync("order_placed", payload);',
  },
];

function TerminalBlock({
  title,
  children,
}: {
  title: string;
  children: React.ReactNode;
}) {
  return (
    <div className="overflow-hidden rounded-lg border border-zinc-200 transition-shadow duration-300 hover:shadow-md">
      <div className="flex items-center gap-2 border-b border-zinc-200 bg-zinc-100 px-4 py-2.5">
        <span className="h-3 w-3 rounded-full bg-[#ff5f57] transition-opacity hover:opacity-70" />
        <span className="h-3 w-3 rounded-full bg-[#febc2e] transition-opacity hover:opacity-70" />
        <span className="h-3 w-3 rounded-full bg-[#28c840] transition-opacity hover:opacity-70" />
        <span className="ml-2 text-xs font-medium text-zinc-500">{title}</span>
      </div>
      <pre className="overflow-x-auto bg-zinc-50 px-5 py-4 text-sm leading-relaxed text-zinc-800">
        <code>{children}</code>
      </pre>
    </div>
  );
}

/* Syntax-highlighted C# fragments for stepper */
function ConfigureCode() {
  return (
    <>
      <span className="text-blue-600">var</span>{" "}
      <span className="text-zinc-800">client</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-blue-600">new</span>{" "}
      <span className="text-emerald-600">EnmaClient</span>
      <span className="text-zinc-500">(</span>
      <span className="text-zinc-800">options</span>{" "}
      <span className="text-zinc-500">{"=>"}</span>{" "}
      <span className="text-zinc-500">{"{"}</span>
      {"\n"}
      {"    "}
      <span className="text-zinc-800">options</span>
      <span className="text-zinc-500">.</span>
      <span className="text-zinc-800">ApiKey</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-amber-600">&quot;&lt;your-api-key&gt;&quot;</span>
      <span className="text-zinc-500">;</span>
      {"\n"}
      {"    "}
      <span className="text-zinc-800">options</span>
      <span className="text-zinc-500">.</span>
      <span className="text-zinc-800">ProjectId</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-amber-600">&quot;my-project&quot;</span>
      <span className="text-zinc-500">;</span>
      {"\n"}
      {"    "}
      <span className="text-zinc-800">options</span>
      <span className="text-zinc-500">.</span>
      <span className="text-zinc-800">BatchSize</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-purple-600">50</span>
      <span className="text-zinc-500">;</span>
      {"\n"}
      {"    "}
      <span className="text-zinc-800">options</span>
      <span className="text-zinc-500">.</span>
      <span className="text-zinc-800">FlushInterval</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-emerald-600">TimeSpan</span>
      <span className="text-zinc-500">.</span>
      <span className="text-amber-700">FromSeconds</span>
      <span className="text-zinc-500">(</span>
      <span className="text-purple-600">5</span>
      <span className="text-zinc-500">);</span>
      {"\n"}
      <span className="text-zinc-500">{"}"});</span>
    </>
  );
}

function TrackCode() {
  return (
    <>
      <span className="text-blue-600">await</span>{" "}
      <span className="text-zinc-800">client</span>
      <span className="text-zinc-500">.</span>
      <span className="text-amber-700">TrackAsync</span>
      <span className="text-zinc-500">(</span>
      <span className="text-amber-600">&quot;order_placed&quot;</span>
      <span className="text-zinc-500">,</span>{" "}
      <span className="text-blue-600">new</span>{" "}
      <span className="text-zinc-500">{"{"}</span>
      {"\n"}
      {"    "}
      <span className="text-zinc-800">UserId</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-amber-600">&quot;user-42&quot;</span>
      <span className="text-zinc-500">,</span>
      {"\n"}
      {"    "}
      <span className="text-zinc-800">Amount</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-purple-600">99.90</span>
      <span className="text-zinc-500">,</span>
      {"\n"}
      {"    "}
      <span className="text-zinc-800">Currency</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-amber-600">&quot;USD&quot;</span>
      {"\n"}
      <span className="text-zinc-500">{"}"});</span>
    </>
  );
}

function CSharpExample() {
  return (
    <TerminalBlock title="Program.cs">
      <span className="text-zinc-400">{"// 1. Create and configure the client"}</span>
      {"\n"}
      <span className="text-blue-600">var</span>{" "}
      <span className="text-zinc-800">client</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-blue-600">new</span>{" "}
      <span className="text-emerald-600">EnmaClient</span>
      <span className="text-zinc-500">(</span>
      <span className="text-zinc-800">options</span>{" "}
      <span className="text-zinc-500">{"=>"}</span>{" "}
      <span className="text-zinc-500">{"{"}</span>
      {"\n"}
      {"    "}
      <span className="text-zinc-800">options</span>
      <span className="text-zinc-500">.</span>
      <span className="text-zinc-800">ApiKey</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-amber-600">&quot;&lt;your-api-key&gt;&quot;</span>
      <span className="text-zinc-500">;</span>
      {"\n"}
      {"    "}
      <span className="text-zinc-800">options</span>
      <span className="text-zinc-500">.</span>
      <span className="text-zinc-800">ProjectId</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-amber-600">&quot;my-project&quot;</span>
      <span className="text-zinc-500">;</span>
      {"\n"}
      {"    "}
      <span className="text-zinc-800">options</span>
      <span className="text-zinc-500">.</span>
      <span className="text-zinc-800">BatchSize</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-purple-600">50</span>
      <span className="text-zinc-500">;</span>
      {"\n"}
      {"    "}
      <span className="text-zinc-800">options</span>
      <span className="text-zinc-500">.</span>
      <span className="text-zinc-800">FlushInterval</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-emerald-600">TimeSpan</span>
      <span className="text-zinc-500">.</span>
      <span className="text-amber-700">FromSeconds</span>
      <span className="text-zinc-500">(</span>
      <span className="text-purple-600">5</span>
      <span className="text-zinc-500">);</span>
      {"\n"}
      <span className="text-zinc-500">{"}"});</span>
      {"\n\n"}
      <span className="text-zinc-400">{"// 2. Define your event payload"}</span>
      {"\n"}
      <span className="text-blue-600">var</span>{" "}
      <span className="text-zinc-800">payload</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-blue-600">new</span>{" "}
      <span className="text-zinc-500">{"{"}</span>
      {"\n"}
      {"    "}
      <span className="text-zinc-800">UserId</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-amber-600">&quot;user-42&quot;</span>
      <span className="text-zinc-500">,</span>
      {"\n"}
      {"    "}
      <span className="text-zinc-800">Amount</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-purple-600">99.90</span>
      <span className="text-zinc-500">,</span>
      {"\n"}
      {"    "}
      <span className="text-zinc-800">Currency</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-amber-600">&quot;USD&quot;</span>
      <span className="text-zinc-500">,</span>
      {"\n"}
      {"    "}
      <span className="text-zinc-800">ItemCount</span>{" "}
      <span className="text-zinc-500">=</span>{" "}
      <span className="text-purple-600">3</span>
      {"\n"}
      <span className="text-zinc-500">{"};"}</span>
      {"\n\n"}
      <span className="text-zinc-400">{"// 3. Send the event"}</span>
      {"\n"}
      <span className="text-blue-600">await</span>{" "}
      <span className="text-zinc-800">client</span>
      <span className="text-zinc-500">.</span>
      <span className="text-amber-700">TrackAsync</span>
      <span className="text-zinc-500">(</span>
      <span className="text-amber-600">&quot;order_placed&quot;</span>
      <span className="text-zinc-500">,</span>{" "}
      <span className="text-zinc-800">payload</span>
      <span className="text-zinc-500">);</span>
      {"\n\n"}
      <span className="text-zinc-400">{"// 4. Flush remaining events on shutdown"}</span>
      {"\n"}
      <span className="text-blue-600">await</span>{" "}
      <span className="text-zinc-800">client</span>
      <span className="text-zinc-500">.</span>
      <span className="text-amber-700">FlushAsync</span>
      <span className="text-zinc-500">();</span>
    </TerminalBlock>
  );
}

export default function Page() {
  const [activeStep, setActiveStep] = useState(0);
  const [installTab, setInstallTab] = useState(0);

  return (
    <div className="min-h-screen bg-zinc-50">
      {/* Hero */}
      <section className="relative isolate overflow-hidden border-b border-zinc-200 bg-white">
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
        <div className="absolute inset-0 bg-[linear-gradient(180deg,rgba(255,255,255,0.1)_0%,rgba(255,255,255,0.7)_100%)]" />

        <div className="relative mx-auto flex max-w-7xl flex-col items-center px-6 py-24 text-center">
          <h1 className="text-6xl font-semibold tracking-tight text-zinc-950 sm:text-7xl">
            enma SDK
          </h1>
          <p className="mt-4 max-w-3xl text-2xl leading-tight text-zinc-800 sm:text-[2rem]">
            .NET SDK for sending process events to enma
          </p>

          {/* Feature badges */}
          <div className="mt-6 flex flex-wrap items-center justify-center gap-2">
            {features.map((f) => (
              <span
                key={f}
                className="rounded-full border border-blue-200 bg-blue-50 px-3 py-1 text-xs font-medium text-blue-700"
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

      {/* How it works — stepper + split */}
      <section className="mx-auto w-full max-w-7xl px-6 py-16">
        <h2 className="text-3xl font-semibold tracking-tight text-zinc-950">
          How it works
        </h2>
        <p className="mt-2 text-base text-zinc-500">
          Three steps to start tracking process events.
        </p>

        <div className="mt-10 grid items-start gap-10 sm:grid-cols-[280px_1fr]">
          {/* Left — stepper */}
          <div className="relative flex flex-col">
            {steps.map((step, i) => {
              const isActive = activeStep === i;
              return (
                <button
                  key={step.number}
                  onClick={() => setActiveStep(i)}
                  className="group relative flex cursor-pointer items-start gap-4 py-4 text-left"
                >
                  {/* Vertical line + dot */}
                  <div className="relative flex flex-col items-center">
                    <div
                      className={`z-10 flex h-10 w-10 shrink-0 items-center justify-center rounded-full border-2 transition-all duration-300 ${
                        isActive
                          ? `${step.borderColor} ${step.bgColor} scale-110`
                          : "border-zinc-200 bg-white group-hover:border-zinc-300 group-hover:scale-105"
                      }`}
                    >
                      <step.icon
                        className={`h-5 w-5 transition-colors duration-300 ${
                          isActive
                            ? step.iconColor
                            : "text-zinc-400 group-hover:text-zinc-500"
                        }`}
                      />
                    </div>
                    {i < steps.length - 1 && (
                      <div className="absolute top-10 h-full w-px bg-zinc-200" />
                    )}
                  </div>

                  {/* Text */}
                  <div className="pt-1">
                    <span
                      className={`text-xs font-semibold tracking-widest transition-colors duration-300 ${
                        isActive ? step.iconColor : "text-zinc-400"
                      }`}
                    >
                      STEP {step.number}
                    </span>
                    <h3
                      className={`mt-0.5 text-lg font-semibold transition-colors duration-300 ${
                        isActive ? "text-zinc-900" : "text-zinc-500 group-hover:text-zinc-700"
                      }`}
                    >
                      {step.title}
                    </h3>
                    <p
                      className={`mt-1 text-sm leading-relaxed transition-colors duration-300 ${
                        isActive ? "text-zinc-500" : "text-zinc-400"
                      }`}
                    >
                      {step.description}
                    </p>
                  </div>
                </button>
              );
            })}
          </div>

          {/* Right — code preview with animation */}
          <div className="sm:pt-4">
            <div
              key={activeStep}
              className="animate-[fadeIn_0.3s_ease-out]"
            >
              {activeStep === 0 && steps[0].tabs ? (
                <div>
                  <div className="inline-flex gap-1 rounded-lg bg-zinc-100 p-1">
                    {steps[0].tabs.map((tab, i) => (
                      <button
                        key={tab.label}
                        onClick={() => setInstallTab(i)}
                        className={`cursor-pointer rounded-md px-4 py-2 text-sm font-medium transition-colors ${
                          installTab === i
                            ? "bg-white text-zinc-900 shadow-sm"
                            : "text-zinc-500 hover:text-zinc-700"
                        }`}
                      >
                        {tab.label}
                      </button>
                    ))}
                  </div>
                  <div className="mt-3">
                    <TerminalBlock title="Terminal">
                      {steps[0].tabs[installTab].code}
                    </TerminalBlock>
                  </div>
                </div>
              ) : activeStep === 1 ? (
                <TerminalBlock title="Program.cs">
                  <ConfigureCode />
                </TerminalBlock>
              ) : (
                <TerminalBlock title="Program.cs">
                  <TrackCode />
                </TerminalBlock>
              )}
            </div>
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
            Full integration in one file.
          </p>

          <div className="mt-8 max-w-2xl">
            <CSharpExample />
          </div>
        </div>
      </section>
    </div>
  );
}
