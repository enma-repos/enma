"use client";

import { useMemo } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import OrganizationInvitesService from "@/services/admin/organizationInvitesService";
import { useMe } from "@/hooks/useMe";

export function usePendingInvites() {
  const queryClient = useQueryClient();
  const meQuery = useMe();
  const accountId = meQuery.data?.account.id;
  const service = useMemo(() => new OrganizationInvitesService(), []);

  const pendingQuery = useQuery({
    queryKey: ["invites", "pending"],
    queryFn: () => service.listPending(),
    enabled: Boolean(accountId),
  });

  const acceptInvite = useMutation({
    mutationFn: (args: { organizationId: string; inviteId: string }) =>
      service.accept(args.organizationId, args.inviteId, { acceptedUserId: accountId! }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["invites", "pending"] });
      queryClient.invalidateQueries({ queryKey: ["notifications"] });
      queryClient.invalidateQueries({ queryKey: ["organizations"] });
    },
  });

  const declineInvite = useMutation({
    mutationFn: (args: { organizationId: string; inviteId: string }) =>
      service.decline(args.organizationId, args.inviteId, { declinedUserId: accountId! }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["invites", "pending"] });
      queryClient.invalidateQueries({ queryKey: ["notifications"] });
    },
  });

  return {
    invites: pendingQuery.data ?? [],
    isLoading: pendingQuery.isLoading,
    acceptInvite,
    declineInvite,
  };
}
