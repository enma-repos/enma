"use client";

import { useMemo } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import NotificationsService from "@/services/admin/notificationsService";
import { useMe } from "@/hooks/useMe";

export function useUnreadCount() {
  const meQuery = useMe();
  const accountId = meQuery.data?.account.id;

  return useQuery({
    queryKey: ["notifications", "unread-count"],
    queryFn: () => new NotificationsService().getUnreadCount(),
    enabled: Boolean(accountId),
    staleTime: Infinity,
  });
}

export function useNotifications() {
  const queryClient = useQueryClient();
  const meQuery = useMe();
  const accountId = meQuery.data?.account.id;
  const service = useMemo(() => new NotificationsService(), []);

  const notificationsQuery = useQuery({
    queryKey: ["notifications", "list"],
    queryFn: () => service.list(),
    enabled: Boolean(accountId),
  });

  const markAsRead = useMutation({
    mutationFn: (notificationId: string) => service.markAsRead(notificationId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["notifications"] });
    },
  });

  const markAllAsRead = useMutation({
    mutationFn: () => service.markAllAsRead(),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["notifications"] });
    },
  });

  const deleteNotification = useMutation({
    mutationFn: (notificationId: string) => service.delete(notificationId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["notifications"] });
    },
  });

  return {
    notifications: notificationsQuery.data ?? [],
    isLoading: notificationsQuery.isLoading,
    markAsRead,
    markAllAsRead,
    deleteNotification,
  };
}
