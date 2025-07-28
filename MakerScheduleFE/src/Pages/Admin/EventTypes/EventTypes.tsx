import { deleteEventType } from "@ms/api/eventTypes.api";
import { ConfirmationDialog } from "@ms/Components/Dialogs/Confirmatoin";
import { useAdminEventTypeData } from "@ms/hooks/useAdminEventTypesData";
import { EventTypeDetails } from "@ms/Pages/Admin/EventTypes/EventTypeDetails/EventTypeDetails";

import { EventTypesHeader } from "@ms/Pages/Admin/EventTypes/Header/Header";
import { AdminEventTypesTable } from "@ms/Pages/Admin/EventTypes/Table/Table";
import type { EventType } from "@ms/types/event.types";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { AxiosResponse } from "axios";
import { useEffect, useState, type TransitionEvent } from "react";

const AdminEventTypes = () => {
  const [isDrawerOpen, setIsDrawerOpen] = useState(false);
  const [refreshData, setRefreshData] = useState(false);
  const [selectedEventType, setSelectedEventType] = useState<
    EventType | undefined
  >(undefined);
  const [eventTypeToDelete, setEventTypeToDelete] = useState<
    EventType | undefined
  >();

  const { eventTypes } = useAdminEventTypeData();
  const [filteredEventTypes, setFilteredEventTypes] = useState<EventType[]>([]);

  const queryClient = useQueryClient();

  useEffect(() => {
    if (!eventTypes) return;
    setFilteredEventTypes(structuredClone(eventTypes));
  }, [eventTypes]);

  const { mutate: deleteEventMutation } = useMutation({
    mutationKey: ["deleteEvent"],
    mutationFn: deleteEventType,
    onSuccess: (res, deleteEventId) => {
      if (!eventTypeToDelete) return;

      queryClient.setQueryData(
        ["eventTypes"],
        (oldEvents: AxiosResponse<EventType[]>) => {
          if (!oldEvents) return oldEvents;
          return {
            ...oldEvents,
            data: oldEvents.data.filter((evt) => evt.id !== deleteEventId),
          };
        }
      );

      if (selectedEventType?.id === deleteEventId) {
        setSelectedEventType(undefined);
      }

      setEventTypeToDelete(undefined);
    },
  });

  const handleDrawerClose = (refreshData: boolean) => {
    setRefreshData(refreshData);
    setIsDrawerOpen(false);
  };

  const handleEventEdit = (event: EventType) => {
    setSelectedEventType(event);
    handleDrawerOpen();
  };

  const handleEventTypeCreate = () => {
    const newEvent: EventType = {
      name: "",
      meta: {
        isNew: true,
      },
    };
    setSelectedEventType(newEvent);
    handleDrawerOpen();
  };
  const handleDrawerOpen = () => {
    setRefreshData(false);
    setIsDrawerOpen(true);
  };

  const handlePanelTransitionEnd = (event: TransitionEvent<HTMLDivElement>) => {
    if (
      event.propertyName === "transform" &&
      event.target === event.currentTarget &&
      refreshData
    ) {
      queryClient.invalidateQueries({
        queryKey: ["events"],
      });
    }
  };

  const handleDeletClick = (event: EventType) => {
    setEventTypeToDelete(event);
  };

  const handleCancelDeleteEvent = () => setEventTypeToDelete(undefined);
  const handleConfirmDeleteEvent = () => {
    if (!eventTypeToDelete?.id) return;
    deleteEventMutation(eventTypeToDelete.id);
  };

  const handleSearch = (searchValue: string | undefined) => {
    let filteredEvents = filteredEventTypes.filter((eventType) => {
      return eventType.name
        .toLowerCase()
        .includes(searchValue?.toLowerCase() || "");
    });
    setFilteredEventTypes(filteredEvents);
  };

  return (
    <div className="flex w-full h-full overflow-hidden pb-12">
      <div
        className={`flex-grow basis-0 transition-all duration-300  flex-col ${isDrawerOpen ? "hidden md:flex" : ""}`}
        style={{
          marginRight: isDrawerOpen ? "var(--create-drawer-width)" : "",
        }}
      >
        <EventTypesHeader
          onCreateEventType={handleEventTypeCreate}
          onSearch={handleSearch}
        />
        <AdminEventTypesTable
          eventTypes={filteredEventTypes}
          onEdit={handleEventEdit}
          selectedEventType={selectedEventType}
          onEventTypeDelete={handleDeletClick}
        />
      </div>
      <div
        className="fixed top-0 right-0 h-full bg-white shadow-lg z-50 transition-transform duration-300 w-full md:w-[var(--create-drawer-width)]"
        style={{
          willChange: "transform",
          transform: isDrawerOpen ? "translateX(0)" : "translateX(100%)",
        }}
        onTransitionEnd={handlePanelTransitionEnd}
      >
        <div className="p-6 h-full">
          {selectedEventType && (
            <EventTypeDetails
              onClose={handleDrawerClose}
              selectedEventType={selectedEventType}
              eventTypes={eventTypes}
            />
          )}
        </div>
      </div>
      <ConfirmationDialog
        open={!!eventTypeToDelete}
        onCancel={handleCancelDeleteEvent}
        onConfirm={handleConfirmDeleteEvent}
        title="Delete Event"
        details="You sure? Because you cant come back from this!"
      />
    </div>
  );
};

export { AdminEventTypes };
