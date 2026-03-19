export function TerminalBlock({
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
