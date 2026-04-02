export function ContactHeroSection() {
  return (
    <section className="relative isolate overflow-hidden bg-white">
      <div
        className="absolute inset-0 opacity-30"
        style={{
          backgroundImage:
            "radial-gradient(circle, #a1a1aa 1.5px, transparent 1.5px)",
          backgroundSize: "24px 24px",
        }}
      />
      <div
        className="absolute inset-0"
        style={{
          background:
            "radial-gradient(800px circle at 50% 50%, transparent 50%, white 85%)",
        }}
      />
      <div className="absolute inset-x-0 bottom-0 h-24 bg-gradient-to-b from-transparent to-white" />

      <div className="relative mx-auto flex max-w-7xl flex-col items-center px-6 pb-52 pt-40 text-center">
        <h1 className="text-6xl font-semibold tracking-tight text-zinc-950 sm:text-7xl">
          Meet the team behind Enma
        </h1>
        <p className="mt-4 max-w-3xl text-2xl leading-tight text-zinc-800 sm:text-[2rem]">
          Enma is a process-mining platform developed as part of the XV Congress of Young Scientists at ITMO University
        </p>
      </div>
    </section>
  );
}
