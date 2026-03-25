"use client";

import { useNotifications } from "@/hooks/useNotifications";
import { usePendingInvites } from "@/hooks/usePendingInvites";
import { Button } from "@/components/shared";
import type { NotificationDto, OrganizationInviteDto } from "@/types/admin.types";
import { NotificationType, OrganizationRole } from "@/types/admin.types";

const ROLE_LABELS: Record<OrganizationRole, string> = {
  [OrganizationRole.Owner]: "Owner",
  [OrganizationRole.Admin]: "Admin",
  [OrganizationRole.Analyst]: "Analyst",
  [OrganizationRole.Developer]: "Developer",
  [OrganizationRole.Viewer]: "Viewer",
};

function formatTimeAgo(dateStr: string): string {
  const diff = Date.now() - new Date(dateStr).getTime();
  const minutes = Math.floor(diff / 60000);
  if (minutes < 1) return "just now";
  if (minutes < 60) return `${minutes}m ago`;
  const hours = Math.floor(minutes / 60);
  if (hours < 24) return `${hours}h ago`;
  const days = Math.floor(hours / 24);
  return `${days}d ago`;
}

function PendingInviteItem({
  invite,
  onAccept,
  onDecline,
  isAccepting,
  isDeclining,
}: {
  invite: OrganizationInviteDto;
  onAccept: () => void;
  onDecline: () => void;
  isAccepting: boolean;
  isDeclining: boolean;
}) {
  return (
    <div className="flex flex-col gap-2 border-b border-zinc-100 px-4 py-3 last:border-b-0">
      <div>
        <p className="text-sm font-medium text-zinc-900">
          Приглашение в {invite.organizationName || "организацию"}
        </p>
        <p className="mt-0.5 text-xs text-zinc-500">
          Роль: {ROLE_LABELS[invite.role]}
        </p>
      </div>
      <div className="flex items-center gap-2">
        <button
          className="cursor-pointer rounded-md bg-zinc-900 px-3 py-1 text-xs font-medium text-white hover:bg-zinc-700 disabled:opacity-50"
          onClick={onAccept}
          disabled={isAccepting || isDeclining}
        >
          {isAccepting ? "..." : "Принять"}
        </button>
        <button
          className="cursor-pointer rounded-md border border-zinc-200 px-3 py-1 text-xs text-zinc-600 hover:bg-zinc-50 disabled:opacity-50"
          onClick={onDecline}
          disabled={isAccepting || isDeclining}
        >
          {isDeclining ? "..." : "Отклонить"}
        </button>
      </div>
    </div>
  );
}

function NotificationItem({
  notification,
  onRead,
  onDelete,
}: {
  notification: NotificationDto;
  onRead: (id: string) => void;
  onDelete: (id: string) => void;
}) {
  return (
    <div
      className={`flex flex-col gap-1 border-b border-zinc-100 px-4 py-3 last:border-b-0 ${
        notification.isRead ? "opacity-60" : ""
      }`}
    >
      <div className="flex items-start justify-between gap-2">
        <div className="flex-1 min-w-0">
          <p className="text-sm font-medium text-zinc-900 truncate">
            {notification.title}
          </p>
          <p className="text-xs text-zinc-500 mt-0.5">
            {notification.message}
          </p>
        </div>
        <span className="text-[10px] text-zinc-400 whitespace-nowrap mt-0.5">
          {formatTimeAgo(notification.createdAt)}
        </span>
      </div>
      <div className="flex items-center gap-2 mt-1">
        {!notification.isRead && (
          <button
            className="cursor-pointer text-[11px] text-zinc-500 hover:text-zinc-700"
            onClick={() => onRead(notification.id)}
          >
            Прочитано
          </button>
        )}
        <button
          className="cursor-pointer text-[11px] text-zinc-400 hover:text-zinc-600"
          onClick={() => onDelete(notification.id)}
        >
          Убрать
        </button>
      </div>
    </div>
  );
}

export function NotificationPanel({ onClose }: { onClose: () => void }) {
  const { notifications: rawNotifications, isLoading, markAsRead, markAllAsRead, deleteNotification } =
    useNotifications();
  const { invites, isLoading: invitesLoading, acceptInvite, declineInvite } =
    usePendingInvites();

  // Filter out invite-received notifications — pending invites are shown separately
  const notifications = rawNotifications.filter(
    (n) => n.type !== NotificationType.OrganizationInviteReceived,
  );

  return (
    <div className="absolute right-0 top-full mt-2 z-50 w-80 rounded-xl border border-zinc-200 bg-white shadow-xl">
      <div className="flex items-center justify-between border-b border-zinc-100 px-4 py-3">
        <h3 className="text-sm font-semibold text-zinc-900">Уведомления</h3>
        {notifications.length > 0 && (
          <button
            className="cursor-pointer text-[11px] text-zinc-500 hover:text-zinc-700"
            onClick={() => markAllAsRead.mutate()}
          >
            Прочитать все
          </button>
        )}
      </div>

      <div className="max-h-80 overflow-y-auto">
        {/* Pending invites section */}
        {invites.length > 0 && (
          <div>
            <div className="bg-zinc-50 px-4 py-1.5">
              <span className="text-[11px] font-medium uppercase tracking-wide text-zinc-400">
                Приглашения
              </span>
            </div>
            {invites.map((invite) => (
              <PendingInviteItem
                key={invite.id}
                invite={invite}
                onAccept={() =>
                  acceptInvite.mutate({
                    organizationId: invite.organizationId,
                    inviteId: invite.id,
                  })
                }
                onDecline={() =>
                  declineInvite.mutate({
                    organizationId: invite.organizationId,
                    inviteId: invite.id,
                  })
                }
                isAccepting={acceptInvite.isPending}
                isDeclining={declineInvite.isPending}
              />
            ))}
          </div>
        )}

        {/* Regular notifications */}
        {isLoading || invitesLoading ? (
          <p className="px-4 py-6 text-center text-xs text-zinc-400">Загрузка...</p>
        ) : notifications.length === 0 && invites.length === 0 ? (
          <p className="px-4 py-6 text-center text-xs text-zinc-400">
            Нет уведомлений
          </p>
        ) : (
          notifications.map((n) => (
            <NotificationItem
              key={n.id}
              notification={n}
              onRead={(id) => markAsRead.mutate(id)}
              onDelete={(id) => deleteNotification.mutate(id)}
            />
          ))
        )}
      </div>
    </div>
  );
}
