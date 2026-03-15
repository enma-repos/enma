"use client";

import { useCallback } from "react";
import AuthService from "@/services/auth/authService";

export function useLogout() {
  return useCallback(async () => {
    try {
      await new AuthService().logout({ refreshToken: "" });
    } catch {
      // ignore — server may have already invalidated the session
    }
    window.location.assign("/auth");
  }, []);
}
