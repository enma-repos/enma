"use client";

import { useState } from "react";
import type { EventDefinitionDto } from "@/types/admin.types";
import { useEventDefinitions } from "@/hooks/useEventDefinitions";
import { ConfirmDialog } from "@/components/shared";
import { EventsEmpty } from "@/components/app/events/events-empty";
import { EventsSkeleton } from "@/components/app/events/events-skeleton";
import { EventsTable } from "@/components/app/events/events-table";
import { EventsToolbar } from "@/components/app/events/events-toolbar";
import { CreateEventDialog } from "@/components/app/events/create-event-dialog";
import { EventDetailDialog } from "@/components/app/events/event-detail-dialog";

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return "Не удалось загрузить события.";
}

export type EventsScreenProps = {
  organizationSlug: string;
  projectKey: string;
};

export function EventsScreen({ organizationSlug, projectKey }: EventsScreenProps) {
  const [isCreateOpen, setIsCreateOpen] = useState(false);
  const [selectedEvent, setSelectedEvent] = useState<EventDefinitionDto | null>(null);
  const [eventToDelete, setEventToDelete] = useState<EventDefinitionDto | null>(null);

  const {
    eventDefinitions,
    isLoading,
    error,
    createEvent,
    isCreating,
    createError,
    deleteEvent,
    isDeleting,
    setEventDescription,
    isSavingDescription,
  } = useEventDefinitions(organizationSlug, projectKey);

  const handleDelete = async () => {
    if (!eventToDelete) return;
    try {
      await deleteEvent(eventToDelete.id);
      setEventToDelete(null);
    } catch {
      // error stays visible via isDeleting state
    }
  };

  return (
    <div className="mx-auto w-full max-w-6xl mt-6">
      <EventsToolbar onCreate={() => setIsCreateOpen(true)} />

      <div className="mt-6">
        {isLoading ? <EventsSkeleton /> : null}

        {!isLoading && error ? (
          <div className="rounded-2xl border border-red-200 bg-red-50 p-5 text-sm text-red-700">
            {getErrorMessage(error)}
          </div>
        ) : null}

        {!isLoading && !error && eventDefinitions.length === 0 ? (
          <EventsEmpty />
        ) : null}

        {!isLoading && !error && eventDefinitions.length > 0 ? (
          <EventsTable
            events={eventDefinitions}
            onSelect={setSelectedEvent}
            onDelete={setEventToDelete}
          />
        ) : null}
      </div>

      <CreateEventDialog
        open={isCreateOpen}
        onClose={() => setIsCreateOpen(false)}
        onCreate={createEvent}
        isCreating={isCreating}
        error={createError}
      />

      <EventDetailDialog
        event={selectedEvent}
        open={selectedEvent !== null}
        onClose={() => setSelectedEvent(null)}
        onSetDescription={setEventDescription}
        isSavingDescription={isSavingDescription}
      />

      <ConfirmDialog
        open={eventToDelete !== null}
        onClose={() => setEventToDelete(null)}
        onConfirm={handleDelete}
        title="Удалить событие"
        description={`Вы уверены, что хотите удалить событие «${eventToDelete?.name ?? ""}»? Это действие нельзя отменить.`}
        confirmLabel="Удалить"
        isConfirming={isDeleting}
      />
    </div>
  );
}
