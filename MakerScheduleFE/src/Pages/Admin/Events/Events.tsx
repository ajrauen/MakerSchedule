import { deleteEvent } from "@ms/api/event.api";
import { ConfirmationDialog } from "@ms/Components/Dialogs/Confirmatoin";
import { useAdminEventsData } from "@ms/hooks/useAdminEventsData";
import { EventDetails } from "@ms/Pages/Admin/Events/EventDetails/EventDetails";
import { EventsHeader } from "@ms/Pages/Admin/Events/Header/Header";
import { AdminEventsTable } from "@ms/Pages/Admin/Events/Table/Table";
import type { EventOffering } from "@ms/types/event.types";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { AxiosResponse } from "axios";
import { useEffect, useState, type TransitionEvent } from "react";

const AdminEvents = () => {
  const [isDrawerOpen, setIsDrawerOpen] = useState(false);
  const [refreshData, setRefreshData] = useState(false);
  const [selectedEvent, setSelectedEvent] = useState<EventOffering | undefined>(
    undefined
  );
  const [eventToDelete, setEventToDelete] = useState<
    EventOffering | undefined
  >();

  const { events, appMetaData } = useAdminEventsData();
  const [filteredEvents, setFilteredEvents] = useState<EventOffering[]>([]);
  const [searchString, setSearchString] = useState("");
  const [filterValue, setFilterValue] = useState("");

  const queryClient = useQueryClient();

  useEffect(() => {
    if (!events) return;
    setFilteredEvents(structuredClone(events));
  }, [events]);

  const { mutate: deleteEventMutation } = useMutation({
    mutationKey: ["deleteEvent"],
    mutationFn: deleteEvent,
    onSuccess: () => {
      if (!eventToDelete) return;

      queryClient.setQueryData(
        ["events"],
        (oldEvents: AxiosResponse<EventOffering[]>) => {
          if (!oldEvents) return oldEvents;
          return {
            ...oldEvents,
            data: oldEvents.data.filter((evt) => evt.id !== eventToDelete.id),
          };
        }
      );

      if (selectedEvent?.id === eventToDelete.id) {
        setSelectedEvent(undefined);
      }

      setEventToDelete(undefined);
    },
  });

  const handleDrawerClose = (refreshData: boolean) => {
    setRefreshData(refreshData);
    setIsDrawerOpen(false);
  };

  const handleEventEdit = (event: EventOffering) => {
    setSelectedEvent(event);
    handleDrawerOpen();
  };

  const handleEventCreate = () => {
    const newEvent: EventOffering = {
      description: "",
      eventName: "",

      meta: {
        isNew: true,
      },
    };
    setSelectedEvent(newEvent);
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

  const handleDeletClick = (event: EventOffering) => {
    setEventToDelete(event);
  };

  const handleCancelDeleteEvent = () => setEventToDelete(undefined);
  const handleConfirmDeleteEvent = () => {
    if (!eventToDelete?.id) return;

    deleteEventMutation(eventToDelete.id);
  };

  useEffect(() => {
    let filtered =
      events?.filter((event) => {
        return (
          event.eventName.toLowerCase().includes(searchString.toLowerCase()) ||
          event.description.toLowerCase().includes(searchString.toLowerCase())
        );
      }) || [];

    if (filterValue) {
      filtered = filtered.filter((event) =>
        event.eventType?.includes(filterValue)
      );
    }

    setFilteredEvents(filtered);
  }, [searchString, filterValue, events]);

  const handleSearch = (value: string | undefined) => {
    setSearchString(value || "");
  };

  const handleFilterChange = (value: string) => {
    setFilterValue(value);
  };

  return (
    <div className="flex w-full h-full overflow-hidden pb-12">
      <div
        className={`flex-grow basis-0 transition-all duration-300  flex-col ${isDrawerOpen ? "hidden md:flex" : ""}`}
        style={{
          marginRight: isDrawerOpen ? "var(--create-drawer-width)" : "",
        }}
      >
        <EventsHeader
          onCreateEvent={handleEventCreate}
          onSearch={handleSearch}
          eventTypes={appMetaData.eventTypes || []}
          onFilterChange={handleFilterChange}
        />
        <AdminEventsTable
          events={filteredEvents}
          onEdit={handleEventEdit}
          selectedEvent={selectedEvent}
          onEventDelete={handleDeletClick}
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
          {selectedEvent && (
            <EventDetails
              onClose={handleDrawerClose}
              selectedEvent={selectedEvent}
              eventTypes={appMetaData.eventTypes}
            />
          )}
        </div>
      </div>
      <ConfirmationDialog
        open={!!eventToDelete}
        onCancel={handleCancelDeleteEvent}
        onConfirm={handleConfirmDeleteEvent}
        title="Delete Event"
        details="You sure? Because you cant come back from this!"
      />
    </div>
  );
};

export { AdminEvents };
