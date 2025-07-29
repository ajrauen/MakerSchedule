import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import { TextSearch } from "@ms/Components/TextSearch/TextSearch";
import type { EventType } from "@ms/types/event.types";
import AddIcon from "@mui/icons-material/Add";
import { Button } from "@mui/material";
import { useMemo } from "react";

interface EventsHeaderProps {
  onCreateEvent: () => void;
  onSearch: (value: string | undefined) => void;
  onFilterChange: (value: string) => void;
  eventTypes: EventType[];
}

const EventsHeader = ({
  onCreateEvent,
  onSearch,
  onFilterChange,
  eventTypes,
}: EventsHeaderProps) => {
  const eventTypeOptions = useMemo(() => {
    if (!eventTypes) return [];
    let eventTypeOptions = eventTypes.map((eventType) => ({
      value: eventType.name,
      label: eventType.name,
    }));
    eventTypeOptions.unshift({ value: "", label: "All Event Types" });

    return eventTypeOptions;
  }, [eventTypes]);

  return (
    <div className="flex items-end mt-2">
      <div className="grow flex items-end gap-6">
        <TextSearch onSearch={onSearch} />
        <FormSelect
          autoWidth={true}
          label="Event Type"
          variant="standard"
          className="w-64"
          options={eventTypeOptions}
          onChange={(event) => {
            const value =
              event.target && typeof event.target.value === "string"
                ? event.target.value
                : "";
            onFilterChange(value);
          }}
        />
      </div>
      <Button onClick={onCreateEvent} startIcon={<AddIcon />} variant="text">
        Event
      </Button>
    </div>
  );
};

export { EventsHeader };
