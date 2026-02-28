"use client";

import type { ReactNode } from "react";
import { Card, EnmaLogo } from "@/components/shared";

export type AuthCardProps = {
  title: string;
  description?: string;
  children: ReactNode;
};

export function AuthCard({ title, description, children }: AuthCardProps) {
  return (
    <Card className="w-full max-w-md p-7">
      <div className="flex items-center justify-center gap-2">
        <EnmaLogo className="h-8 w-10 text-zinc-900" aria-hidden="true" />
        <div className="text-lg font-semibold tracking-tight">enma</div>
      </div>

      <div className="mt-6 text-center">
        <h1 className="text-xl font-semibold text-zinc-900">{title}</h1>
        {description ? (
          <p className="mt-2 text-sm text-zinc-500">{description}</p>
        ) : null}
      </div>

      <div className="mt-6">{children}</div>
    </Card>
  );
}

