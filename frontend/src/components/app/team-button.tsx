"use client";

import { useCallback, useEffect, useRef, useState } from "react";
import { Button, IconUsers } from "@/components/shared";
import { TeamPopup } from "./team-popup";
import type { Guid } from "@/types/admin.types";

export function TeamButton({ orgId }: { orgId: Guid | null }) {
  const [open, setOpen] = useState(false);
  const ref = useRef<HTMLDivElement>(null);

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

  if (!orgId) return null;

  return (
    <div ref={ref} className="relative">
      <Button
        variant="ghost"
        size="sm"
        className="h-10 w-10 rounded-xl p-0"
        aria-label="Team"
        onClick={toggle}
      >
        <IconUsers className="h-5 w-5" />
      </Button>

      {open && <TeamPopup orgId={orgId} />}
    </div>
  );
}
