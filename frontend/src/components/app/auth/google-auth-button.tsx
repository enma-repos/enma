"use client";

import { Button, IconGoogle } from "@/components/shared";

export type GoogleAuthButtonProps = {
  onClick: () => void | Promise<void>;
  isLoading?: boolean;
};

export function GoogleAuthButton({ onClick, isLoading }: GoogleAuthButtonProps) {
  return (
    <Button
      onClick={onClick}
      disabled={isLoading}
      className="h-11 w-full justify-center rounded-xl"
    >
      <IconGoogle size={18} aria-hidden="true" />
      {isLoading ? "Перенаправляем..." : "Зарегистрироваться через Google"}
    </Button>
  );
}
