import { deleteEvent } from "@ms/api/event.api";
import { ConfirmationDialog } from "@ms/Components/Dialogs/Confirmation";
import { useAdminEventsData } from "@ms/hooks/useAdminEventsData";
import { OccurrenceCalendarDetails } from "@ms/Pages/Admin/Events/AdminCalendarView/CalanderOccurrenceDetails/OccurrenceCalendarDetails";
import { EventsHeader } from "@ms/Pages/Admin/Events/Header/Header";
import { AdminCalendarView } from "@ms/Pages/Admin/Events/AdminCalendarView/AdminCalendarView";
import { AdminEventView } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventView";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import {
  selectAdminState,
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
  const { selectedEventOccurrence } = useAppSelector(selectAdminState);
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

      // Clear any selected event if it was the one deleted
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

  const handleViewStateChange = (value: ViewState) => {
    dispatch(setSelectedEventOccurrence(undefined));
    dispatch(setSelectedEvent(undefined));
    setViewState(value);
  };

  return (
    <div className="flex w-full h-full overflow-hidden pb-12">
      <div className="flex-grow flex-col w-full">
        <EventsHeader
          onSearch={handleSearch}
          eventTypes={appMetaData.eventTypes || []}
          onFilterChange={handleFilterChange}
          onSetViewState={handleViewStateChange}
          viewState={viewState}
        />
        {viewState === "calendar" ? (
          <AdminCalendarView selectedEventType={filterValue} />
        ) : (
          <AdminEventView
            events={filteredEvents}
            onEventDelete={handleDeletClick}
            eventTypes={appMetaData.eventTypes || []}
          />
        )}
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
