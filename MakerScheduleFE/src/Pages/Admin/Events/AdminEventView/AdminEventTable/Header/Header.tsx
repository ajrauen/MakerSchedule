import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import { TextSearch } from "@ms/Components/TextSearch/TextSearch";
import ToggleButton from "@mui/material/ToggleButton";
import ToggleButtonGroup from "@mui/material/ToggleButtonGroup";
import TableViewIcon from "@mui/icons-material/TableView";
import CalendarMonthIcon from "@mui/icons-material/CalendarMonth";
import type { EventOffering } from "@ms/types/event.types";
import AddIcon from "@mui/icons-material/Add";
import { Button } from "@mui/material";
import { useAppDispatch } from "@ms/redux/hooks";
import {
  setAdminDrawerOpen,
  setSelectedEvent,
  setSelectedEventOccurrence,
} from "@ms/redux/slices/adminSlice";
import type { Occurrence } from "@ms/types/occurrence.types";
import type { ViewState } from "@ms/types/admin.types";

interface EventsHeaderProps {
  onSearch: (value: string | undefined) => void;
  onFilterChange: (value: string) => void;
  onSetViewState: (value: ViewState) => void;
  viewState: ViewState;
}

const EventsHeader = ({
  onSearch,
  onFilterChange,
  onSetViewState,
  viewState,
}: EventsHeaderProps) => {
  const dispatch = useAppDispatch();

  const handleCreateEvent = () => {
    const newEvent: EventOffering = {
      description: "",
      eventName: "",

      meta: {
        isNew: true,
        componentOrigin: "occurrenceCalendar",
      },
    };
    dispatch(setAdminDrawerOpen(true));
    dispatch(setSelectedEvent(newEvent));
  };

  const handleCreateOccurrence = () => {
    const today = new Date();
    if (today.getHours() >= 12) {
      today.setDate(today.getDate() + 1);
    }
    today.setHours(12, 0, 0, 0);

    const newOccurrence: Occurrence = {
      eventId: "",
      scheduleStart: today.toISOString(),
      attendees: [],
      leaders: [],
      status: "pending",
      meta: {
        isNew: true,
        componentOrigin: "occurrenceCalendar",
      },
    };

    dispatch(setSelectedEventOccurrence(newOccurrence));
    dispatch(setAdminDrawerOpen(true));
  };

  const handleCreateClick = () => {
    if (viewState === "calendar") {
      handleCreateOccurrence();
    } else {
      handleCreateEvent();
    }
  };

  return (
    <div className="flex items-end mt-2">
      <div className="flex items-end gap-6">
        <TextSearch onSearch={onSearch} />
        <FormSelect
          autoWidth={true}
          label="Event Type"
          variant="standard"
          className="w-64"
          options={[]}
          onChange={(event) => {
            const value =
              event.target && typeof event.target.value === "string"
                ? event.target.value
                : "";
            onFilterChange(value);
          }}
        />
      </div>
      <div className="mx-auto pr-[25rem] ">
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
        onClick={handleCreateClick}
        startIcon={<AddIcon />}
        variant="text"
      >
        {viewState === "calendar" ? "Occurrence" : "Event"}
      </Button>
    </div>
  );
};

export { EventsHeader };
