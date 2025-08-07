import { IconButton, Drawer } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { OccurrenceCalendar } from "./OccurrenceCalendar/OccurrenceCalendar";
import { useState, useEffect } from "react";
import { useAppSelector } from "@ms/redux/hooks";
import { selectAdminState } from "@ms/redux/slices/adminSlice";
import { OccurrenceCalendarDetails } from "@ms/Pages/Admin/Events/AdminCalendarView/CalanderOccurrenceDetails/OccurrenceCalendarDetails";

interface AdminCalendarViewProps {
  selectedEventType?: string;
}

const AdminCalendarView = ({ selectedEventType }: AdminCalendarViewProps) => {
  const [drawerOpen, setDrawerOpen] = useState(false);
  const { selectedEventOccurrence } = useAppSelector(selectAdminState);

  useEffect(() => {
    if (
      selectedEventOccurrence?.meta?.componentOrigin === "occurrenceCalendar"
    ) {
      setDrawerOpen(true);
    } else {
      setDrawerOpen(false);
    }
  }, [selectedEventOccurrence]);

  const handleCloseDrawer = () => {
    setDrawerOpen(false);
  };

  const handleDrawerClose = () => {
    handleCloseDrawer();
  };

  const handleIconClose = () => {
    handleCloseDrawer();
  };

  return (
    <>
      <OccurrenceCalendar selectedEventType={selectedEventType} />

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
