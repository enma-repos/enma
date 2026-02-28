"use client";

import { useQuery } from "@tanstack/react-query";
import AuthService from "@/services/auth/authService";

export function useMe() {
  return useQuery({
    queryKey: ["me"],
    queryFn: () => new AuthService().getMe(),
    staleTime: 30_000,
  });
}

