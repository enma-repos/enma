"use client";

import { useCallback, useEffect, useRef, useState } from "react";
import { Button, IconBell } from "@/components/shared";
import { useUnreadCount, useNotifications } from "@/hooks/useNotifications";
import { usePendingInvites } from "@/hooks/usePendingInvites";
import { NotificationPanel } from "./notification-panel";

export function NotificationBell() {
  const [open, setOpen] = useState(false);
  const ref = useRef<HTMLDivElement>(null);
  const { data: unreadCount } = useUnreadCount();
  const { invites } = usePendingInvites();
  const badgeCount = (unreadCount ?? 0) + invites.length;

  const toggle = useCallback(() => setOpen((p) => !p), []);

  useEffect(() => {
    if (!open) return;
    function handleClick(e: MouseEvent) {
      if (ref.current && !ref.current.contains(e.target as Node)) {
        setOpen(false);
      }
    }
    document.addEventListener("mousedown", handleClick);
    return () => document.removeEventListener("mousedown", handleClick);
  }, [open]);

  return (
    <div ref={ref} className="relative">
      <Button
        variant="ghost"
        size="sm"
        className="h-10 w-10 rounded-xl p-0"
        aria-label="Notifications"
        onClick={toggle}
      >
        <IconBell className="h-5 w-5" />
        {badgeCount > 0 && (
          <span className="absolute -top-0.5 -right-0.5 flex h-4 min-w-4 items-center justify-center rounded-full bg-red-500 px-1 text-[10px] font-medium text-white">
            {badgeCount > 99 ? "99+" : badgeCount}
          </span>
        )}
      </Button>

      {open && <NotificationPanel onClose={() => setOpen(false)} />}
    </div>
  );
}
