import { isOccurrenceInPast } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/EventDetails/EventOccurrences/Occurrence.utils";
import { useAppSelector } from "@ms/redux/hooks";
import { selectAdminState } from "@ms/redux/slices/adminSlice";
import { useMemo } from "react";

const OccurrenceCalendarDetails = () => {
  const { selectedEventOccurrence } = useAppSelector(selectAdminState);
  const isSelectedOccurrenceInPast = useMemo(() => {
    if (!selectedEventOccurrence) return false;

    return isOccurrenceInPast(selectedEventOccurrence);
  }, [selectedEventOccurrence]);

  return (
    <div className="flex flex-col h-full w-full">
      {selectedEventOccurrence && isSelectedOccurrenceInPast ? (
        <div>
          <h2>{selectedEventOccurrence?.eventName}</h2>
          <p>{selectedEventOccurrence?.status}</p>
        </div>
      ) : (
        <p>No event selected</p>
      )}
    </div>
  );
};

export { OccurrenceCalendarDetails };
