import type { EventOffering } from "@ms/types/event.types";
import { AdminEventTable } from "./AdminEventTable/AdminEventTable";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteEvent } from "@ms/api/event.api";
import { useEffect, useRef, useState } from "react";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import {
  selectAdminState,
  setAdminDrawerOpen,
  setSelectedEvent,
  setSelectedEventOccurrence,
} from "@ms/redux/slices/adminSlice";
import { EventsHeader } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/Header/Header";
import type { ViewState } from "@ms/types/admin.types";
import { ConfirmationDialog } from "@ms/Components/Dialogs/Confirmation";
import { useAdminEventsData } from "@ms/hooks/useAdminEventsData";
import { AdminEventDrawer } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/AdminEventDrawer/AdminEventDrawer";

interface AdminEventViewProps {
  onViewStateChange: (value: ViewState) => void;
  viewState: ViewState;
}

const AdminEventView = ({
  onViewStateChange,
  viewState,
}: AdminEventViewProps) => {
  const [tabValue, setTabValue] = useState(0);
  const selectedEventIdRef = useRef<string | undefined>(undefined);
  const [searchString, setSearchString] = useState("");
  const [filterValue, setFilterValue] = useState("");
  const [filteredEvents, setFilteredEvents] = useState<EventOffering[]>([]);
  const [eventToDelete, setEventToDelete] = useState<
    EventOffering | undefined
  >();

  const { events, appMetaData } = useAdminEventsData();

  const queryClient = useQueryClient();
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

      queryClient.setQueryData(["events"], (oldEvents: EventOffering[]) => {
        if (!oldEvents) return oldEvents;
        return oldEvents.filter((evt) => evt.id !== eventToDelete.id);
      });

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

  const { selectedEvent, selectedEventOccurrence } =
    useAppSelector(selectAdminState);

  const handleEventSelect = (event: EventOffering) => {
    dispatch(setSelectedEvent(event));
    dispatch(setAdminDrawerOpen(true));
    setTabValue(0);
  };

  const handleCloseDrawer = () => {
    if (selectedEvent?.meta?.isNew) {
      dispatch(setSelectedEvent(undefined));
    }
    dispatch(setSelectedEventOccurrence(undefined));
    dispatch(setAdminDrawerOpen(false));
  };

  const handleDrawerClose = () => {
    handleCloseDrawer();
  };

  useEffect(() => {
    if (selectedEventIdRef.current === selectedEvent?.id) {
      return;
    }
    selectedEventIdRef.current = selectedEvent?.id;
    setTabValue(0);
  }, [selectedEvent]);

  useEffect(() => {
    if (selectedEventOccurrence && !selectedEventOccurrence.meta?.isNew) {
      setTabValue(1);
    }
  }, [selectedEventOccurrence]);

  const handleSearch = (value: string | undefined) => {
    setSearchString(value || "");
  };

  const handleFilterChange = (value: string) => {
    setFilterValue(value);
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

  return (
    <>
      <EventsHeader
        onSearch={handleSearch}
        eventTypes={appMetaData.eventTypes || []}
        onFilterChange={handleFilterChange}
        onSetViewState={onViewStateChange}
        viewState={viewState}
      />
      <AdminEventTable
        events={filteredEvents}
        onEventDelete={handleDeletClick}
        onEventSelect={handleEventSelect}
        selectedEvent={selectedEvent}
      />
      <AdminEventDrawer
        onDrawerClose={handleDrawerClose}
        selectedTab={tabValue}
        setSelectedTab={setTabValue}
      />

      <ConfirmationDialog
        open={!!eventToDelete}
        onCancel={handleCancelDeleteEvent}
        onConfirm={handleConfirmDeleteEvent}
        title="Delete Event"
        details="You sure? Because you cant come back from this!"
      />
    </>
  );
};

export { AdminEventView };
