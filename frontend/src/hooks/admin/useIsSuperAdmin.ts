"use client";

import { useMe } from "@/hooks/useMe";

export function useIsSuperAdmin(): boolean {
  const { data } = useMe();
  return data?.role === "SuperAdmin";
}
