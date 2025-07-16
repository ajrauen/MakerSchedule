import type { EventOffering, EventType } from "@ms/types/event.types";

import { BasicEventDetails } from "@ms/Pages/Admin/Events/EventDetails/BasicDetails/BasicEventDetails";
import { IconButton, Tab, Tabs } from "@mui/material";
import { useEffect, useState } from "react";
import { TabPanel } from "@ms/Components/LayoutComponents/TabPanel/TabPanel";
import CloseIcon from "@mui/icons-material/Close";
import { useQuery } from "@tanstack/react-query";
import { getEvent } from "@ms/api/event.api";
import { EventOccurrences } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/EventOccurrences";

interface CreateEventProps {
  onClose: (refreshData: boolean) => void;
  selectedEvent: EventOffering;
  eventTypes: EventType;
}

const EventDetails = ({
  onClose,
  selectedEvent,
  eventTypes,
}: CreateEventProps) => {
  const [value, setValue] = useState(0);
  const [event, setEvent] = useState<EventOffering>(selectedEvent);

  const handleChange = (_event: React.SyntheticEvent, newValue: number) => {
    setValue(newValue);
  };

  const { data: eventResponse } = useQuery({
    queryKey: [selectedEvent?.id, "event"],
    queryFn: () => {
      if (!selectedEvent?.id) return;
      return getEvent(selectedEvent.id);
    },
    enabled: !!selectedEvent?.id,
  });

  useEffect(() => {
    if (eventResponse?.data) {
      setEvent(eventResponse.data);
      return;
    }
  }, [eventResponse]);

  function a11yProps(index: number) {
    return {
      id: `vertical-tab-${index}`,
      "aria-controls": `vertical-tabpanel-${index}`,
    };
  }

  return (
    <div className="flex flex-col h-full w-full">
      <div className="ml-auto absolute right-2 Z-100">
        <IconButton onClick={() => onClose(false)}>
          <CloseIcon />
        </IconButton>
      </div>
      <Tabs
        value={value}
        onChange={handleChange}
        aria-label="Vertical tabs example"
      >
        <Tab label="Details" {...a11yProps(0)} />
        <Tab label="Occurences" {...a11yProps(1)} />
      </Tabs>
      <TabPanel index={0} value={value}>
        <BasicEventDetails
          onClose={onClose}
          eventTypes={eventTypes}
          selectedEvent={event}
        />
      </TabPanel>
      <TabPanel index={1} value={value}>
        <EventOccurrences selectedEvent={event} />
      </TabPanel>
    </div>
  );
};

export { EventDetails };
