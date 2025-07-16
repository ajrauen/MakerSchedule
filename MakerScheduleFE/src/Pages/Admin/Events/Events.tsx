import { useApplicationData } from "@ms/hooks/useApplicationData";
import { EventDetails } from "@ms/Pages/Admin/Events/EventDetails/EventDetails";
import { EventsHeader } from "@ms/Pages/Admin/Events/Header/Header";
import { AdminEventsTable } from "@ms/Pages/Admin/Events/Table/Table";
import type { EventOffering } from "@ms/types/event.types";
import { useQueryClient } from "@tanstack/react-query";
import { useState, type TransitionEvent } from "react";

const AdminEvents = () => {
  const [isDrawerOpen, setIsDrawerOpen] = useState(false);
  const [refreshData, setRefreshData] = useState(false);
  const [selectedEvent, setSelectedEvent] = useState<EventOffering | undefined>(
    undefined
  );

  const { events, appMetaData } = useApplicationData();
  const queryClient = useQueryClient();

  const handleDrawerClose = (refreshData: boolean) => {
    setRefreshData(refreshData);
    setIsDrawerOpen(false);
  };

  const handleEventEdit = (event: EventOffering) => {
    setSelectedEvent(event);
    handleDrawerOpen();
  };

  const handleDrawerOpen = () => {
    setRefreshData(false);
    setIsDrawerOpen(true);
  };

  const handlePanelTransitionEnd = (event: TransitionEvent<HTMLDivElement>) => {
    if (
      event.propertyName === "transform" &&
      event.target === event.currentTarget &&
      refreshData
    ) {
      queryClient.invalidateQueries({
        queryKey: ["events"],
      });
    }
  };

  return (
    <div className="flex w-full">
      <div
        className={`flex-grow basis-0 transition-all duration-300  flex-col ${isDrawerOpen ? "hidden md:flex" : ""}`}
        style={{
          marginRight: isDrawerOpen ? "var(--create-drawer-width)" : "",
        }}
      >
        <EventsHeader onOpenDrawer={handleDrawerOpen} />
        <AdminEventsTable
          events={events}
          onEdit={handleEventEdit}
          eventTypes={appMetaData.eventTypes}
          selectedEvent={selectedEvent}
        />
      </div>
      <div
        className="fixed top-0 right-0 h-full bg-white shadow-lg z-50 transition-transform duration-300 w-full md:w-[var(--create-drawer-width)]"
        style={{
          willChange: "transform",
          transform: isDrawerOpen ? "translateX(0)" : "translateX(100%)",
        }}
        onTransitionEnd={handlePanelTransitionEnd}
      >
        <div className="p-6 h-full">
          <EventDetails
            onClose={handleDrawerClose}
            selectedEvent={selectedEvent}
            eventTypes={appMetaData.eventTypes}
          />
        </div>
      </div>
    </div>
  );
};

export { AdminEvents };
