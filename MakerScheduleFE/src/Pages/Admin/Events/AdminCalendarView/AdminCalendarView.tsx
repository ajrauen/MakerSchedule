import { IconButton, Drawer } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { OccurrenceCalendar } from "./OccurrenceCalendar/OccurrenceCalendar";
import { useState, useEffect } from "react";
import { useAppSelector } from "@ms/redux/hooks";
import { selectAdminState } from "@ms/redux/slices/adminSlice";
import { OccurrenceCalendarDetails } from "@ms/Pages/Admin/Events/AdminCalendarView/CalanderOccurrenceDetails/OccurrenceCalendarDetails";
import { OccurrenceCalendarHeader } from "@ms/Pages/Admin/Events/AdminCalendarView/Header/Header";
import type { ViewState } from "@ms/types/admin.types";
import { useAdminEventsData } from "@ms/hooks/useAdminEventsData";
import { useQuery } from "@tanstack/react-query";
import type { DatesSetArg } from "@fullcalendar/core/index.js";
import { getOccurrences } from "@ms/api/occurrence.api";
import type { Occurrence } from "@ms/types/occurrence.types";

interface AdminCalendarViewProps {
  selectedEventType?: string;
  onViewStateChange: (value: ViewState) => void;
  viewState: ViewState;
}

const AdminCalendarView = ({
  selectedEventType,
  onViewStateChange,
  viewState,
}: AdminCalendarViewProps) => {
  const [drawerOpen, setDrawerOpen] = useState(false);
  const { selectedEventOccurrence } = useAppSelector(selectAdminState);
  const { appMetaData } = useAdminEventsData();
  const [filteredOccurrences, setFilteredOccurrences] = useState<Occurrence[]>(
    []
  );

  const [calendarStartDate, setCalendarStartDate] = useState<Date | null>(null);
  const [calendarEndDate, setCalendarEndDate] = useState<Date | null>(null);

  const handleDatesSet = (arg: DatesSetArg) => {
    setCalendarStartDate(arg.start);
    setCalendarEndDate(arg.end);
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
    queryKey: [
      "occurrences",
      calendarStartDate,
      calendarEndDate,
      selectedEventType,
    ],
    queryFn: () =>
      getOccurrences(calendarStartDate!, calendarEndDate!, selectedEventType),
    enabled: !!calendarStartDate && !!calendarEndDate,
  });

  useEffect(() => {
    if (occurrences && occurrences.data) {
      setFilteredOccurrences(occurrences.data);
    }
  }, [occurrences]);

  const handleCloseDrawer = () => {
    setDrawerOpen(false);
  };

  const handleDrawerClose = () => {
    handleCloseDrawer();
  };

  const handleIconClose = () => {
    handleCloseDrawer();
  };

  const handleFilterChange = (value: string) => {
    if (!occurrences) return;

    if (!value) {
      setFilteredOccurrences(occurrences.data ?? []);
    } else {
      setFilteredOccurrences(
        occurrences.data.filter((occurrence) => occurrence.eventType === value)
      );
    }
  };

  return (
    <>
      <OccurrenceCalendarHeader
        eventTypes={appMetaData.eventTypes || []}
        onFilterChange={handleFilterChange}
        onSetViewState={onViewStateChange}
        viewState={viewState}
      />

      <OccurrenceCalendar
        selectedEventType={selectedEventType}
        occurrences={filteredOccurrences}
        onDateSet={handleDatesSet}
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
            <IconButton onClick={handleIconClose}>
              <CloseIcon />
            </IconButton>
          </div>

          <OccurrenceCalendarDetails />
        </div>
      </Drawer>
    </>
  );
};

export { AdminCalendarView };
