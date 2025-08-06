import { deleteEvent } from "@ms/api/event.api";
import { ConfirmationDialog } from "@ms/Components/Dialogs/Confirmation";
import { useAdminEventsData } from "@ms/hooks/useAdminEventsData";
import { EventCalendar } from "@ms/Pages/Admin/Events/Calendar/EventsCalendar";
import { EventDetails } from "@ms/Pages/Admin/Events/EventDetails/EventDetails";
import { OccurrenceView } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/OccurrenceView/OccurrenceView";
import { EventsHeader } from "@ms/Pages/Admin/Events/Header/Header";
import { AdminEventsTable } from "@ms/Pages/Admin/Events/Table/Table";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import {
  selectAdminState,
  setAdminDrawerOpen,
  setSelectedEvent,
  setSelectedEventOccurrence,
} from "@ms/redux/slices/adminSlice";
import type { ViewState } from "@ms/types/admin.types";
import type { EventOffering } from "@ms/types/event.types";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { AxiosResponse } from "axios";
import { useEffect, useState } from "react";

const AdminEvents = () => {
  const [eventToDelete, setEventToDelete] = useState<
    EventOffering | undefined
  >();

  const { events, appMetaData } = useAdminEventsData();
  const [filteredEvents, setFilteredEvents] = useState<EventOffering[]>([]);
  const [searchString, setSearchString] = useState("");
  const [filterValue, setFilterValue] = useState("");
  const [viewState, setViewState] = useState<ViewState>("table");

  const queryClient = useQueryClient();
  const { selectedEvent, adminDrawerOpen, selectedEventOccurrence } =
    useAppSelector(selectAdminState);
  const dispatch = useAppDispatch();

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
        dispatch(setSelectedEvent(undefined));
      }

      setEventToDelete(undefined);
    },
  });

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
      filtered = filtered.filter(
        (event) => event.eventType?.id === filterValue
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
  const handleCaledarDrawerClose = () => {
    dispatch(setSelectedEventOccurrence(undefined));
    dispatch(setAdminDrawerOpen(false));
  };

  const handleViewStateChange = (value: ViewState) => {
    dispatch(setSelectedEventOccurrence(undefined));
    dispatch(setSelectedEvent(undefined));
    dispatch(setAdminDrawerOpen(false));
    setViewState(value);
  };

  return (
    <div className="flex w-full h-full overflow-hidden pb-12">
      <div
        className={`flex-grow basis-0 transition-all duration-300  flex-col ${adminDrawerOpen ? "hidden md:flex" : ""}`}
        style={{
          marginRight: adminDrawerOpen ? "var(--create-drawer-width)" : "",
        }}
      >
        <EventsHeader
          onSearch={handleSearch}
          eventTypes={appMetaData.eventTypes || []}
          onFilterChange={handleFilterChange}
          onSetViewState={handleViewStateChange}
          viewState={viewState}
        />
        {viewState === "calendar" ? (
          <EventCalendar selectedEventType={filterValue} />
        ) : (
          <AdminEventsTable
            events={filteredEvents}
            onEventDelete={handleDeletClick}
          />
        )}
      </div>
      <div
        className="fixed top-0 right-0 h-full bg-white shadow-lg z-50 transition-transform duration-300 w-full md:w-[var(--create-drawer-width)]"
        style={{
          willChange: "transform",
          transform: adminDrawerOpen ? "translateX(0)" : "translateX(100%)",
        }}
      >
        <div className="p-6 h-full">
          {selectedEventOccurrence &&
          selectedEventOccurrence.meta?.componentOrigin ===
            "occurrenceCalendar" ? (
            <OccurrenceView onBack={handleCaledarDrawerClose} />
          ) : (
            selectedEvent && (
              <EventDetails eventTypes={appMetaData.eventTypes} />
            )
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
