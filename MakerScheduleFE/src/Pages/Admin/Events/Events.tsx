import { AdminCalendarView } from "@ms/Pages/Admin/Events/AdminCalendarView/AdminCalendarView";
import { AdminEventView } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventView";
import { useAppDispatch } from "@ms/redux/hooks";
import {
  setAdminDrawerOpen,
  setSelectedEvent,
  setSelectedEventOccurrence,
} from "@ms/redux/slices/adminSlice";
import type { ViewState } from "@ms/types/admin.types";

import { useState } from "react";

const AdminEvents = () => {
  const [viewState, setViewState] = useState<ViewState>("table");
  const dispatch = useAppDispatch();

  const handleViewStateChange = (value: ViewState) => {
    dispatch(setSelectedEventOccurrence(undefined));
    dispatch(setSelectedEvent(undefined));
    dispatch(setAdminDrawerOpen(false));
    setViewState(value);
  };

  return (
    <div className="flex w-full h-full overflow-hidden pb-12">
      <div className="flex-grow flex-col w-full">
        {viewState === "calendar" ? (
          <AdminCalendarView
            onViewStateChange={handleViewStateChange}
            viewState={viewState}
          />
        ) : (
          <AdminEventView
            onViewStateChange={handleViewStateChange}
            viewState={viewState}
          />
        )}
      </div>
    </div>
  );
};

export { AdminEvents };
