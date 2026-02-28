"use client";

import { useMe } from "@/hooks/useMe";
import { AppTopbar } from "@/components/app/app-topbar";

export function AppTopbarContainer() {
  const meQuery = useMe();
  const displayName = meQuery.data?.user?.displayName ?? null;
  return <AppTopbar displayName={displayName} />;
}

