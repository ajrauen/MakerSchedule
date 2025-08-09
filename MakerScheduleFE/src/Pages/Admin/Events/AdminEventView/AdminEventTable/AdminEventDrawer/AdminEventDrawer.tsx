import { useAdminEventsData } from "@ms/hooks/useAdminEventsData";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import {
  selectAdminState,
  setSelectedEvent,
} from "@ms/redux/slices/adminSlice";
import type { EventOffering } from "@ms/types/event.types";
import { useQuery } from "@tanstack/react-query";
import { useMemo } from "react";
import { IconButton, Tab, Tabs, Drawer } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { TabPanel } from "@ms/Components/LayoutComponents/TabPanel/TabPanel";
import { EventOccurrences } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/EventDetails/EventOccurrences/EventOccurrences";
import { BasicEventDetails } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/EventDetails/BasicDetails/BasicEventDetails";
import { getEvent } from "@ms/api/event.api";

interface AdminEventDrawerProps {
  onDrawerClose: () => void;
  selectedTab: number;
  setSelectedTab: (value: number) => void;
}

const AdminEventDrawer = ({
  onDrawerClose,
  selectedTab,
  setSelectedTab,
}: AdminEventDrawerProps) => {
  const { appMetaData } = useAdminEventsData();
  const { selectedEvent, adminDrawerOpen } = useAppSelector(selectAdminState);

  const dispatch = useAppDispatch();

  const handleTabChange = (_event: React.SyntheticEvent, newValue: number) => {
    setSelectedTab(newValue);
  };

  const { data: eventResponse } = useQuery({
    queryKey: ["event", selectedEvent?.id],
    queryFn: async () => {
      return getEvent(selectedEvent!.id!);
    },
    enabled: !!selectedEvent?.id && adminDrawerOpen,
  });

  const detailedEvent = useMemo(() => {
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
  }, [eventResponse?.data, selectedEvent, dispatch]);

  const handleIconClose = () => {
    onDrawerClose();
  };

  function a11yProps(index: number) {
    return {
      id: `vertical-tab-${index}`,
      "aria-controls": `vertical-tabpanel-${index}`,
    };
  }

  return (
    <Drawer
      anchor="right"
      open={adminDrawerOpen}
      onClose={onDrawerClose}
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

        {detailedEvent && (
          <>
            <Tabs
              value={selectedTab}
              onChange={handleTabChange}
              aria-label="Event details tabs"
            >
              <Tab label="Details" {...a11yProps(0)} />
              {!detailedEvent.meta?.isNew && (
                <Tab label="Occurrences" {...a11yProps(1)} />
              )}
            </Tabs>

            <TabPanel index={0} value={selectedTab}>
              <BasicEventDetails
                onClose={onDrawerClose}
                eventTypes={appMetaData.eventTypes || []}
              />
            </TabPanel>

            <TabPanel index={1} value={selectedTab}>
              <EventOccurrences />
            </TabPanel>
          </>
        )}
      </div>
    </Drawer>
  );
};

export { AdminEventDrawer };
