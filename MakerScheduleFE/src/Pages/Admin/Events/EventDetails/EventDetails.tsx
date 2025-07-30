import type { EventOffering, EventType } from "@ms/types/event.types";

import { BasicEventDetails } from "@ms/Pages/Admin/Events/EventDetails/BasicDetails/BasicEventDetails";
import { IconButton, Tab, Tabs } from "@mui/material";
import { useEffect, useMemo, useState } from "react";
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
import { EventOccurrences } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/EventOccurrences";

interface CreateEventProps {
  eventTypes: EventType[];
}

const EventDetails = ({ eventTypes }: CreateEventProps) => {
  const [value, setValue] = useState(0);

  const handleChange = (_event: React.SyntheticEvent, newValue: number) => {
    setValue(newValue);
  };

  const { selectedEvent, selectedEventOccurrence } =
    useAppSelector(selectAdminState);
  const dispatch = useAppDispatch();

  useEffect(() => {
    setValue(0);
  }, [selectedEvent]);

  useEffect(() => {
    if (selectedEventOccurrence) {
      setValue(1);
    }
  }, [selectedEventOccurrence]);

  const { data: eventResponse } = useQuery({
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
  }, [eventResponse?.data || selectedEvent]);

  const handleClose = () => {
    if (selectedEvent?.meta?.isNew) {
      dispatch(setSelectedEvent(undefined));
    }
    dispatch(setAdminDrawerOpen(false));
  };

  return (
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
        <BasicEventDetails onClose={handleClose} eventTypes={eventTypes} />
      </TabPanel>
      <TabPanel index={1} value={value}>
        <EventOccurrences />
      </TabPanel>
    </div>
  );
};

export { EventDetails };
