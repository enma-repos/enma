"use client";

import { useState } from "react";
import { useOrgMembers, useMemberProjects, useTeamMutations } from "@/hooks/useTeam";
import { useProjects } from "@/hooks/useProjects";
import { useMe } from "@/hooks/useMe";
import { TeamInvitePanel } from "./team-invite-panel";
import { TeamMembersPanel } from "./team-members-panel";
import { TeamMemberDetail } from "./team-member-detail";
import type { Guid, OrganizationRole, ProjectRole } from "@/types/admin.types";

type Screen = "invite" | "members" | { view: "detail"; userId: string };

export function TeamPopup({ orgId }: { orgId: Guid }) {
  const [screen, setScreen] = useState<Screen>("invite");

  const meQuery = useMe();
  const currentUserId = meQuery.data?.account.id;

  const membersQuery = useOrgMembers(orgId);
  const members = membersQuery.data ?? [];

  const { projects } = useProjects(orgId);

  const selectedUserId = typeof screen === "object" ? screen.userId : null;
  const selectedMember = members.find((m) => m.userId === selectedUserId) ?? null;
  const memberProjectsQuery = useMemberProjects(orgId, selectedUserId);

  const mutations = useTeamMutations(orgId);

  const activeTab = typeof screen === "object" ? "members" : screen;

  return (
    <div className="absolute right-0 top-full mt-2 z-50 w-[380px] rounded-xl border border-zinc-200 bg-white shadow-xl">
      {/* Tabs */}
      {typeof screen !== "object" && (
        <div className="flex border-b border-zinc-100">
          <button
            type="button"
            className={`flex-1 cursor-pointer py-3 text-center text-[13px] transition-colors ${
              activeTab === "invite"
                ? "font-medium text-zinc-900 border-b-2 border-zinc-900"
                : "text-zinc-400 hover:text-zinc-600"
            }`}
            onClick={() => setScreen("invite")}
          >
            Пригласить
          </button>
          <button
            type="button"
            className={`flex-1 cursor-pointer py-3 text-center text-[13px] transition-colors ${
              activeTab === "members"
                ? "font-medium text-zinc-900 border-b-2 border-zinc-900"
                : "text-zinc-400 hover:text-zinc-600"
            }`}
            onClick={() => setScreen("members")}
          >
            Участники
          </button>
        </div>
      )}

      {/* Panels */}
      {screen === "invite" && (
        <TeamInvitePanel
          projects={projects}
          isSending={mutations.sendInvite.isPending}
          onSendInvite={(email, role, projectIds) => {
            if (!currentUserId) return;
            mutations.sendInvite.mutate({
              email,
              role,
              createdByUserId: currentUserId,
            });
          }}
        />
      )}

      {screen === "members" && (
        <TeamMembersPanel
          members={members}
          isLoading={membersQuery.isLoading}
          onSelectMember={(userId) => setScreen({ view: "detail", userId })}
        />
      )}

      {typeof screen === "object" && selectedMember && (
        <TeamMemberDetail
          member={selectedMember}
          projectsData={memberProjectsQuery.data}
          isLoadingProjects={memberProjectsQuery.isLoading}
          onBack={() => setScreen("members")}
          onSetOrgRole={(role: OrganizationRole) =>
            mutations.setMemberRole.mutate({ userId: selectedMember.userId, role })
          }
          onRemoveMember={() => {
            mutations.removeMember.mutate(selectedMember.userId);
            setScreen("members");
          }}
          onAddToProject={(projectId: Guid, role: ProjectRole) =>
            mutations.addToProject.mutate({ projectId, userId: selectedMember.userId, role })
          }
          onRemoveFromProject={(projectId: Guid) =>
            mutations.removeFromProject.mutate({ projectId, userId: selectedMember.userId })
          }
          onSetProjectRole={(projectId: Guid, role: ProjectRole) =>
            mutations.setProjectRole.mutate({ projectId, userId: selectedMember.userId, role })
          }
        />
      )}
    </div>
  );
}
