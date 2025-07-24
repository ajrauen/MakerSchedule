import type { EventOffering, EventType } from "@ms/types/event.types";

import { BasicEventDetails } from "@ms/Pages/Admin/Events/EventDetails/BasicDetails/BasicEventDetails";
import { IconButton, Tab, Tabs } from "@mui/material";
import { useEffect, useMemo, useState } from "react";
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

  const handleChange = (_event: React.SyntheticEvent, newValue: number) => {
    setValue(newValue);
  };

  useEffect(() => {
    if (selectedEvent.meta?.isNew) {
      setValue(0);
    } else {
      setValue(1);
    }
  }, [selectedEvent.meta?.isNew]);

  const { data: eventResponse } = useQuery({
    queryKey: ["event", selectedEvent?.id],
    queryFn: async () => {
      if (!selectedEvent.id) {
        throw new Error("Query should not run without userId");
      }

      return getEvent(selectedEvent.id);
    },
    enabled: !!selectedEvent?.id || !!selectedEvent.meta?.isNew,
  });

  function a11yProps(index: number) {
    return {
      id: `vertical-tab-${index}`,
      "aria-controls": `vertical-tabpanel-${index}`,
    };
  }

  const event = useMemo(() => {
    return {
      ...selectedEvent,
      ...eventResponse?.data,
    };
  }, [eventResponse?.data, selectedEvent]);

  return (
    <div className="flex flex-col h-full w-full">
      <div className="ml-auto absolute right-2 z-1001">
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
        {event.meta?.isNew ? null : (
          <Tab label="Occurrences" {...a11yProps(1)} />
        )}
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
