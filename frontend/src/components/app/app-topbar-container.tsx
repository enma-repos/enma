"use client";

import { useMe } from "@/hooks/useMe";
import { useLogout } from "@/hooks/useLogout";
import { AppTopbar } from "@/components/app/app-topbar";

export function AppTopbarContainer() {
  const meQuery = useMe();
  const displayName = meQuery.data?.user?.displayName ?? null;
  const logout = useLogout();
  return <AppTopbar displayName={displayName} onLogout={logout} />;
}

