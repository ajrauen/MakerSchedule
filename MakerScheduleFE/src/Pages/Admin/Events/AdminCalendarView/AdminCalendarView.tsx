import { IconButton, Drawer } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { OccurrenceCalendar } from "./OccurrenceCalendar/OccurrenceCalendar";
import { useState, useEffect } from "react";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import {
  selectAdminState,
  setSelectedEvent,
} from "@ms/redux/slices/adminSlice";
import { OccurrenceCalendarDetails } from "@ms/Pages/Admin/Events/AdminCalendarView/CalanderOccurrenceDetails/OccurrenceCalendarDetails";
import { OccurrenceCalendarHeader } from "@ms/Pages/Admin/Events/AdminCalendarView/Header/Header";
import type { ViewState } from "@ms/types/admin.types";
import { useAdminEventsData } from "@ms/hooks/useAdminEventsData";
import { useQuery } from "@tanstack/react-query";
import type { DatesSetArg } from "@fullcalendar/core/index.js";
import { getOccurrences } from "@ms/api/occurrence.api";
import type { Occurrence } from "@ms/types/occurrence.types";
import type { DateClickArg } from "@fullcalendar/interaction/index.js";

interface AdminCalendarViewProps {
  onViewStateChange: (value: ViewState) => void;
  viewState: ViewState;
}

const AdminCalendarView = ({
  onViewStateChange,
  viewState,
}: AdminCalendarViewProps) => {
  const [drawerOpen, setDrawerOpen] = useState(false);
  const { selectedEventOccurrence } = useAppSelector(selectAdminState);
  const [filteredOccurrences, setFilteredOccurrences] = useState<Occurrence[]>(
    []
  );

  const [calendarStartDate, setCalendarStartDate] = useState<Date | null>(null);
  const [calendarEndDate, setCalendarEndDate] = useState<Date | null>(null);
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);

  const dispatch = useAppDispatch();

  const handleDatesSet = (arg: DatesSetArg) => {
    setCalendarStartDate(arg.start);
    setCalendarEndDate(arg.end);
  };

  const handleDateClick = (arg: DateClickArg) => {
    setSelectedDate(arg.date);
  };

  useEffect(() => {
    if (
      selectedEventOccurrence?.meta?.componentOrigin === "occurrenceCalendar"
    ) {
      setDrawerOpen(true);
    } else {
      setDrawerOpen(false);
    }
  }, [selectedEventOccurrence]);

  const { data: occurrences } = useQuery({
    queryKey: ["occurrences", calendarStartDate, calendarEndDate],
    queryFn: () => getOccurrences(calendarStartDate!, calendarEndDate!),
    enabled: !!calendarStartDate && !!calendarEndDate,
  });

  useEffect(() => {
    if (occurrences) {
      setFilteredOccurrences(occurrences);
    }
  }, [occurrences]);

  const handleDrawerClose = () => {
    setDrawerOpen(false);
    dispatch(setSelectedEvent(undefined));
  };

  const handleFilterChange = (value: string) => {
    if (!occurrences) return;

    if (!value) {
      setFilteredOccurrences(occurrences ?? []);
    } else {
      setFilteredOccurrences(
        []
        // occurrences.filter((occurrence) => occurrence.eventType === value)
      );
    }
  };

  return (
    <>
      <OccurrenceCalendarHeader
        onFilterChange={handleFilterChange}
        onSetViewState={onViewStateChange}
        selectedDate={selectedDate}
        viewState={viewState}
      />

      <OccurrenceCalendar
        occurrences={filteredOccurrences}
        onDateSet={handleDatesSet}
        onDateClick={handleDateClick}
      />

      <Drawer
        anchor="right"
        open={drawerOpen}
        onClose={handleDrawerClose}
        variant="temporary"
        ModalProps={{
          keepMounted: false,
          disableScrollLock: true,
        }}
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
            <IconButton onClick={handleDrawerClose}>
              <CloseIcon />
            </IconButton>
          </div>

          <OccurrenceCalendarDetails
            calendarEndDate={calendarEndDate}
            calendarStartDate={calendarStartDate}
            onDrawerClose={handleDrawerClose}
          />
        </div>
      </Drawer>
    </>
  );
};

export { AdminCalendarView };
