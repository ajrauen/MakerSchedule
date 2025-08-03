import { useAdminEventTypeData } from "@ms/hooks/useAdminEventTypesData";
import { EventTypeDetails } from "@ms/Pages/Admin/EventTypes/EventTypeDetails/EventTypeDetails";

import { EventTypesHeader } from "@ms/Pages/Admin/EventTypes/Header/Header";
import { AdminEventTypesTable } from "@ms/Pages/Admin/EventTypes/Table/Table";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import {
  selectAdminState,
  setAdminDrawerOpen,
  setSelectedEventType,
} from "@ms/redux/slices/adminSlice";
import type { EventType } from "@ms/types/event.types";

import { useEffect, useState, type TransitionEvent } from "react";

const AdminEventTypes = () => {
  const { adminDrawerOpen, selectedEventType } =
    useAppSelector(selectAdminState);

  const { eventTypes } = useAdminEventTypeData();
  const [filteredEventTypes, setFilteredEventTypes] = useState<EventType[]>([]);
  const dispatch = useAppDispatch();

  useEffect(() => {
    if (!eventTypes) return;
    setFilteredEventTypes(structuredClone(eventTypes));
  }, [eventTypes]);

  // const handleDrawerClose = (refreshData: boolean) => {
  //   setRefreshData(refreshData);
  //   setIsDrawerOpen(false);
  // };

  const handleEventTypeCreate = () => {
    const newEvent: EventType = {
      name: "",
      meta: {
        isNew: true,
      },
    };
    dispatch(setSelectedEventType(newEvent));
    dispatch(setAdminDrawerOpen(true));
  };
  // const handleDrawerOpen = () => {
  //   setRefreshData(false);
  //   setIsDrawerOpen(true);
  // };

  const handlePanelTransitionEnd = (event: TransitionEvent<HTMLDivElement>) => {
    // if (
    //   event.propertyName === "transform" &&
    //   event.target === event.currentTarget &&
    //   refreshData
    // ) {
    //   queryClient.invalidateQueries({
    //     queryKey: ["events"],
    //   });
    // }
  };

  const handleSearch = (searchValue: string | undefined) => {
    let filteredEvents = filteredEventTypes.filter((eventType) => {
      return eventType.name
        .toLowerCase()
        .includes(searchValue?.toLowerCase() || "");
    });
    setFilteredEventTypes(filteredEvents);
  };

  return (
    <div className="flex w-full h-full overflow-hidden pb-12">
      <div
        className={`flex-grow basis-0 transition-all duration-300  flex-col ${adminDrawerOpen ? "hidden md:flex" : ""}`}
        style={{
          marginRight: adminDrawerOpen ? "var(--create-drawer-width)" : "",
        }}
      >
        <EventTypesHeader
          onCreateEventType={handleEventTypeCreate}
          onSearch={handleSearch}
        />
        <AdminEventTypesTable eventTypes={filteredEventTypes} />
      </div>
      <div
        className="fixed top-0 right-0 h-full bg-white shadow-lg z-50 transition-transform duration-300 w-full md:w-[var(--create-drawer-width)]"
        style={{
          willChange: "transform",
          transform: adminDrawerOpen ? "translateX(0)" : "translateX(100%)",
        }}
        onTransitionEnd={handlePanelTransitionEnd}
      >
        <div className="p-6 h-full">
          {selectedEventType && (
            <EventTypeDetails selectedEventType={selectedEventType} />
          )}
        </div>
      </div>
    </div>
  );
};

export { AdminEventTypes };
