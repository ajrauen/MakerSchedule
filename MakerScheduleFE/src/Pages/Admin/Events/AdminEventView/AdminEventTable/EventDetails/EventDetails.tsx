import type { EventOffering } from "@ms/types/event.types";

import { BasicEventDetails } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/EventDetails/BasicDetails/BasicEventDetails";
import { IconButton, Tab, Tabs } from "@mui/material";
import { useEffect, useMemo, useRef, useState } from "react";
import { TabPanel } from "@ms/Components/LayoutComponents/TabPanel/TabPanel";
import CloseIcon from "@mui/icons-material/Close";
import { useQuery } from "@tanstack/react-query";
import { getEvent } from "@ms/api/event.api";
import {
  selectAdminState,
  setAdminDrawerOpen,
  setSelectedEvent,
} from "@ms/redux/slices/adminSlice";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import { EventOccurrences } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/EventDetails/EventOccurrences/EventOccurrences";

const EventDetails = () => {
  const [value, setValue] = useState(0);
  // store a ref so that if an event is updated via API, we dont automatically switch tabs
  const selectedEventIdRef = useRef<string | undefined>(undefined);

  const handleChange = (_event: React.SyntheticEvent, newValue: number) => {
    setValue(newValue);
  };

  const { selectedEvent, selectedEventOccurrence, adminDrawerOpen } =
    useAppSelector(selectAdminState);
  const dispatch = useAppDispatch();

  useEffect(() => {
    if (selectedEventIdRef.current === selectedEvent?.id) {
      return;
    }

    selectedEventIdRef.current = selectedEvent?.id;
    setValue(0);
  }, [selectedEvent]);

  useEffect(() => {
    if (selectedEventOccurrence) {
      setValue(1);
    }
  }, [selectedEventOccurrence]);

  const { data: eventData } = useQuery({
    queryKey: ["event", selectedEvent?.id],
    queryFn: async () => {
      return getEvent(selectedEvent!.id!);
    },
    enabled: !!selectedEvent?.id,
  });

  function a11yProps(index: number) {
    return {
      id: `vertical-tab-${index}`,
      "aria-controls": `vertical-tabpanel-${index}`,
    };
  }

  const event = useMemo(() => {
    if (!eventData) return selectedEvent;

    if (JSON.stringify(eventData) === JSON.stringify(selectedEvent)) {
      return selectedEvent;
    }

    const updatedEvent: EventOffering = {
      ...selectedEvent,
      ...eventData,
    };

    dispatch(setSelectedEvent(updatedEvent));
    return updatedEvent;
  }, [eventData, selectedEvent, dispatch]);

  const handleClose = () => {
    if (selectedEvent?.meta?.isNew) {
      dispatch(setSelectedEvent(undefined));
    }
    dispatch(setAdminDrawerOpen(false));
  };

  return (
    <div
      className="fixed top-0 right-0 h-full bg-white shadow-lg z-50 transition-transform duration-300 w-full md:w-[var(--create-drawer-width)]"
      style={{
        willChange: "transform",
        transform: adminDrawerOpen ? "translateX(0)" : "translateX(100%)",
      }}
    >
      <div className="p-6 h-full">
        <div className="flex flex-col h-full w-full">
          <div className="ml-auto absolute right-2 z-1001">
            <IconButton onClick={handleClose}>
              <CloseIcon />
            </IconButton>
          </div>
          <Tabs
            value={value}
            onChange={handleChange}
            aria-label="Vertical tabs example"
          >
            <Tab label="Details" {...a11yProps(0)} />
            {event?.meta?.isNew ? null : (
              <Tab label="Occurrences" {...a11yProps(1)} />
            )}
          </Tabs>
          <TabPanel index={0} value={value}>
            <BasicEventDetails onClose={handleClose} />
          </TabPanel>
          <TabPanel index={1} value={value}>
            <EventOccurrences />
          </TabPanel>
        </div>
      </div>
    </div>
  );
};

export { EventDetails };
