"use client";

import { useState } from "react";
import { Download, Settings, Radio } from "lucide-react";
import { TerminalBlock } from "@/components/shared/terminal-block";

const steps = [
  {
    number: "01",
    title: "Install",
    description: "Add the NuGet package to your .NET project.",
    icon: Download,
    iconColor: "text-blue-500",
    bgColor: "bg-blue-50",
    borderColor: "border-blue-100",
    gradient: "from-blue-50/30 to-white",
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
    gradient: "from-amber-50/30 to-white",
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
    gradient: "from-emerald-50/30 to-white",
    code: 'await client.TrackAsync("order_placed", payload);',
  },
];

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

export function SdkStepperSection() {
  const [activeStep, setActiveStep] = useState(0);
  const [installTab, setInstallTab] = useState(0);

  return (
    <section className="relative -mt-16 mx-auto w-full max-w-7xl px-6 pt-16 pb-2">
      <div className="grid items-center gap-10 sm:grid-cols-[1fr_600px]">
        {/* Left — title + horizontal steps */}
        <div>
          <h2 className="text-3xl font-semibold leading-tight tracking-tight text-zinc-950 sm:text-4xl">
            Start tracking events
            <br />
            in <span className="text-blue-600">three steps</span>
          </h2>

          <div className="mt-14 grid grid-cols-3 gap-3">
            {steps.map((step, i) => {
              const isActive = activeStep === i;
              return (
                <button
                  key={step.number}
                  onClick={() => setActiveStep(i)}
                  className={`group cursor-pointer rounded-xl border p-4 text-left transition-all duration-300 bg-gradient-to-b ${
                    isActive
                      ? `${step.gradient} border-zinc-200 shadow-sm`
                      : "from-transparent to-white border-zinc-100 hover:border-zinc-200 hover:shadow-sm"
                  }`}
                >
                  <div className="flex items-center gap-3">
                    <div
                      className={`flex h-10 w-10 shrink-0 items-center justify-center rounded-lg transition-all duration-300 ${
                        isActive
                          ? `${step.bgColor}`
                          : "bg-zinc-100 group-hover:bg-zinc-200"
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
                    <h3
                      className={`text-lg font-semibold transition-colors duration-300 ${
                        isActive ? "text-zinc-900" : "text-zinc-500 group-hover:text-zinc-700"
                      }`}
                    >
                      {step.title}
                    </h3>
                  </div>
                  <p
                    className={`mt-1 text-xs leading-relaxed transition-colors duration-300 ${
                      isActive ? "text-zinc-500" : "text-zinc-400"
                    }`}
                  >
                    {step.description}
                  </p>
                </button>
              );
            })}
          </div>
        </div>

        {/* Right — tabs + centered terminal */}
        <div className="flex flex-col self-stretch">
          {/* Tabs — always at top */}
          <div className={`${activeStep === 0 ? "opacity-100" : "pointer-events-none opacity-0"} self-start inline-flex gap-1 rounded-lg bg-zinc-100 p-1 transition-opacity duration-300`}>
            {steps[0].tabs?.map((tab, i) => (
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

          {/* Terminal — centered */}
          <div className="flex flex-1 items-center">
            <div
              key={activeStep}
              className="w-full animate-fade-in"
            >
              {activeStep === 0 && steps[0].tabs ? (
                <TerminalBlock title="Terminal">
                  {steps[0].tabs[installTab].code}
                </TerminalBlock>
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
      </div>
    </section>
  );
}
