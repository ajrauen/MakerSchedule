import { isOccurrenceInPast } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/EventDetails/EventOccurrences/Occurrence.utils";
import { OccurrenceReadOnly } from "@ms/Pages/Admin/Events/Common/OccurrenceReadOnly/OccurrenceReadOnly";
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
        <OccurrenceReadOnly onBack={() => {}} />
      ) : (
        <p>No event selected</p>
      )}
    </div>
  );
};

export { OccurrenceCalendarDetails };
