import { Mail } from "lucide-react";

export function ContactInfoSection() {
  return (
    <section className="mx-auto w-full max-w-7xl px-5 py-16 sm:px-8 sm:py-20">
      <div className="relative overflow-hidden rounded-3xl bg-zinc-950 px-8 py-16 text-center sm:px-16 sm:py-20">
        <div
          className="absolute inset-0 opacity-[0.06]"
          style={{
            backgroundImage:
              "linear-gradient(#fff 1px, transparent 1px), linear-gradient(90deg, #fff 1px, transparent 1px)",
            backgroundSize: "32px 32px",
          }}
        />
        <div
          className="absolute inset-0"
          style={{
            background:
              "radial-gradient(500px circle at 50% 50%, rgba(59,130,246,0.12), transparent 70%)",
          }}
        />

        <div className="relative">
          <h2 className="text-3xl font-semibold tracking-tight text-white sm:text-4xl">
            Get in touch
          </h2>
          <p className="mx-auto mt-4 max-w-xl text-lg text-zinc-400">
            Have questions about Enma? Reach out and we'll get back to you.
          </p>

          <a
            href="mailto:enma-inbox@proton.me"
            className="mt-8 inline-flex items-center gap-2 rounded-lg bg-white px-6 py-3 text-sm font-medium text-zinc-900 transition-colors hover:bg-zinc-100"
          >
            <Mail className="h-4 w-4" />
            enma-inbox@proton.me
          </a>
        </div>
      </div>
    </section>
  );
}
