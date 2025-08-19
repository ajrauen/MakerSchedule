import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import ToggleButton from "@mui/material/ToggleButton";
import ToggleButtonGroup from "@mui/material/ToggleButtonGroup";
import TableViewIcon from "@mui/icons-material/TableView";
import CalendarMonthIcon from "@mui/icons-material/CalendarMonth";
import AddIcon from "@mui/icons-material/Add";
import { Button } from "@mui/material";
import { useAppDispatch } from "@ms/redux/hooks";
import {
  setAdminDrawerOpen,
  setSelectedEventOccurrence,
} from "@ms/redux/slices/adminSlice";
import type { Occurrence } from "@ms/types/occurrence.types";
import type { ViewState } from "@ms/types/admin.types";

interface OccurrenceCalendarHeaderProps {
  onFilterChange: (value: string) => void;
  onSetViewState: (value: ViewState) => void;
  viewState: ViewState;
  selectedDate: Date | null;
}

const OccurrenceCalendarHeader = ({
  onFilterChange,
  onSetViewState,
  viewState,
  selectedDate,
}: OccurrenceCalendarHeaderProps) => {
  const dispatch = useAppDispatch();

  const handleCreateOccurrence = () => {
    let today: Date;
    if (selectedDate) {
      today = new Date(selectedDate);
    } else {
      today = new Date();
    }
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

  return (
    <div className="flex items-end mt-2">
      <div className=" flex items-end gap-6">
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
      <div className="mx-auto pr-24 ">
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
        onClick={handleCreateOccurrence}
        startIcon={<AddIcon />}
        variant="text"
      >
        {viewState === "calendar" ? "Occurrence" : "Event"}
      </Button>
    </div>
  );
};

export { OccurrenceCalendarHeader };
