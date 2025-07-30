import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import { TextSearch } from "@ms/Components/TextSearch/TextSearch";
import ToggleButton from "@mui/material/ToggleButton";
import ToggleButtonGroup from "@mui/material/ToggleButtonGroup";
import TableViewIcon from "@mui/icons-material/TableView";
import CalendarMonthIcon from "@mui/icons-material/CalendarMonth";
import type { EventOffering, EventType } from "@ms/types/event.types";
import AddIcon from "@mui/icons-material/Add";
import { Button } from "@mui/material";
import { useMemo } from "react";
import { useAppDispatch } from "@ms/redux/hooks";
import {
  setAdminDrawerOpen,
  setSelectedEvent,
} from "@ms/redux/slices/adminSlice";

interface EventsHeaderProps {
  onSearch: (value: string | undefined) => void;
  onFilterChange: (value: string) => void;
  eventTypes: EventType[];
  onSetViewState: (value: string) => void;
  viewState: string;
}

const EventsHeader = ({
  onSearch,
  onFilterChange,
  eventTypes,
  onSetViewState,
  viewState,
}: EventsHeaderProps) => {
  const dispatch = useAppDispatch();

  const eventTypeOptions = useMemo(() => {
    if (!eventTypes) return [];
    let eventTypeOptions = eventTypes.map((eventType) => ({
      value: eventType.id,
      label: eventType.name,
    }));
    eventTypeOptions.unshift({ value: "", label: "All Event Types" });

    return eventTypeOptions;
  }, [eventTypes]);

  const handleCreateEvent = () => {
    const newEvent: EventOffering = {
      description: "",
      eventName: "",

      meta: {
        isNew: true,
      },
    };
    dispatch(setAdminDrawerOpen(true));
    dispatch(setSelectedEvent(newEvent));
  };

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
      <div>
        <ToggleButtonGroup
          value={viewState}
          exclusive
          onChange={(_event, value) => {
            if (value) onSetViewState(value);
          }}
          size="small"
        >
          <ToggleButton value="table" aria-label="Table View">
            <TableViewIcon />
          </ToggleButton>
          <ToggleButton value="calendar" aria-label="Calendar View">
            <CalendarMonthIcon />
          </ToggleButton>
        </ToggleButtonGroup>
      </div>
      <Button
        onClick={handleCreateEvent}
        startIcon={<AddIcon />}
        variant="text"
      >
        Event
      </Button>
    </div>
  );
};

export { EventsHeader };
