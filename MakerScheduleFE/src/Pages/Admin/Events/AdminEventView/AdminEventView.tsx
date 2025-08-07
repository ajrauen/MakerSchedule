import { IconButton, Tab, Tabs, Drawer } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import type { EventOffering, EventType } from "@ms/types/event.types";
import { TabPanel } from "@ms/Components/LayoutComponents/TabPanel/TabPanel";
import { BasicEventDetails } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/EventDetails/BasicDetails/BasicEventDetails";
import { EventOccurrences } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/EventDetails/EventOccurrences/EventOccurrences";
import { AdminEventTable } from "./AdminEventTable/AdminEventTable";
import { useQuery } from "@tanstack/react-query";
import { getEvent } from "@ms/api/event.api";
import { useEffect, useMemo, useRef, useState } from "react";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import {
  selectAdminState,
  setSelectedEvent,
  setSelectedEventOccurrence,
} from "@ms/redux/slices/adminSlice";

interface AdminEventViewProps {
  events: EventOffering[];
  onEventDelete: (event: EventOffering) => void;
  eventTypes: EventType[];
}

const AdminEventView = ({
  onEventDelete,
  events = [],
  eventTypes,
}: AdminEventViewProps) => {
  const [drawerOpen, setDrawerOpen] = useState(false);
  const [tabValue, setTabValue] = useState(0);
  const selectedEventIdRef = useRef<string | undefined>(undefined);

  const dispatch = useAppDispatch();
  const { selectedEvent, selectedEventOccurrence } =
    useAppSelector(selectAdminState);

  const handleEventSelect = (event: EventOffering) => {
    dispatch(setSelectedEvent(event));
    setDrawerOpen(true);
    setTabValue(0);
  };

  const handleTabChange = (_event: React.SyntheticEvent, newValue: number) => {
    setTabValue(newValue);
  };

  const handleCloseDrawer = () => {
    if (selectedEvent?.meta?.isNew) {
      dispatch(setSelectedEvent(undefined));
    }
    dispatch(setSelectedEventOccurrence(undefined));
    setDrawerOpen(false);
  };

  const handleDrawerClose = () => {
    handleCloseDrawer();
  };

  const handleIconClose = () => {
    handleCloseDrawer();
  };

  const { data: eventResponse } = useQuery({
    queryKey: ["event", selectedEvent?.id],
    queryFn: async () => {
      return getEvent(selectedEvent!.id!);
    },
    enabled: !!selectedEvent?.id && drawerOpen,
  });

  const detailedEvent = useMemo(() => {
    if (!eventResponse?.data) return selectedEvent;

    if (JSON.stringify(eventResponse.data) === JSON.stringify(selectedEvent)) {
      return selectedEvent;
    }

    const updatedEvent: EventOffering = {
      ...selectedEvent,
      ...eventResponse.data,
    };

    dispatch(setSelectedEvent(updatedEvent));
    return updatedEvent;
  }, [eventResponse?.data, selectedEvent, dispatch]);

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

  function a11yProps(index: number) {
    return {
      id: `vertical-tab-${index}`,
      "aria-controls": `vertical-tabpanel-${index}`,
    };
  }

  return (
    <>
      <AdminEventTable
        events={events}
        onEventDelete={onEventDelete}
        onEventSelect={handleEventSelect}
        selectedEvent={selectedEvent}
      />

      <Drawer
        anchor="right"
        open={drawerOpen}
        onClose={handleDrawerClose}
        slotProps={{
          paper: {
            sx: {
              width: "min(90vw, 800px)",
              height: "100vh",
            },
          },
        }}
      >
        <div className="flex flex-col h-full w-full">
          <div className="ml-auto absolute right-2 top-2 z-10">
            <IconButton onClick={handleIconClose}>
              <CloseIcon />
            </IconButton>
          </div>

          {detailedEvent && (
            <>
              <Tabs
                value={tabValue}
                onChange={handleTabChange}
                aria-label="Event details tabs"
              >
                <Tab label="Details" {...a11yProps(0)} />
                {!detailedEvent.meta?.isNew && (
                  <Tab label="Occurrences" {...a11yProps(1)} />
                )}
              </Tabs>

              <TabPanel index={0} value={tabValue}>
                <BasicEventDetails
                  onClose={handleCloseDrawer}
                  eventTypes={eventTypes}
                />
              </TabPanel>

              <TabPanel index={1} value={tabValue}>
                <EventOccurrences />
              </TabPanel>
            </>
          )}
        </div>
      </Drawer>
    </>
  );
};

export { AdminEventView };
