import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import "../globals.css";
import { DocsHeader } from "@/components/docs/docs-header";
import { DocsSidebar } from "@/components/docs/docs-sidebar";
import { ReactQueryProvider } from "@/components/shared/react-query-provider";

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "Documentation — enma",
  description: "enma documentation and API reference",
};

export default function DocsLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body
        className={`${geistSans.variable} ${geistMono.variable} antialiased bg-white`}
      >
        <ReactQueryProvider>
          <DocsHeader />
          <div className="flex min-h-[calc(100vh-3.5rem)]">
            <DocsSidebar />
            <main className="flex-1 overflow-y-auto bg-white">
              <div className="mx-auto max-w-3xl px-6 py-10">
                {children}
              </div>
            </main>
          </div>
        </ReactQueryProvider>
      </body>
    </html>
  );
}
