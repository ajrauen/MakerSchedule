import { isOccurrenceInPast } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/EventDetails/EventOccurrences/Occurrence.utils";
import { OccurrenceForm } from "@ms/Pages/Admin/Events/Common/OccurrenceForm/OccurrenceForm";
import { OccurrenceReadOnly } from "@ms/Pages/Admin/Events/Common/OccurrenceReadOnly/OccurrenceReadOnly";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import {
  selectAdminState,
  setSelectedEventOccurrence,
} from "@ms/redux/slices/adminSlice";
import type { Occurrence } from "@ms/types/occurrence.types";
import { useQueryClient } from "@tanstack/react-query";
import { useMemo } from "react";

interface OccurrenceCalendarDetailsProps {
  calendarStartDate: Date | null;
  calendarEndDate: Date | null;
  onDrawerClose: () => void;
}

const OccurrenceCalendarDetails = ({
  calendarStartDate,
  calendarEndDate,
  onDrawerClose,
}: OccurrenceCalendarDetailsProps) => {
  const { selectedEventOccurrence } = useAppSelector(selectAdminState);
  const isSelectedOccurrenceInPast = useMemo(() => {
    if (!selectedEventOccurrence) return false;

    return isOccurrenceInPast(selectedEventOccurrence);
  }, [selectedEventOccurrence]);

  const queryClient = useQueryClient();
  const dispatch = useAppDispatch();

  const handleCreateSuccess = (createOccurrence: Occurrence) => {
    queryClient.setQueryData(
      ["occurrences", calendarStartDate, calendarEndDate],
      (oldData: Occurrence[]) => {
        if (!oldData) return undefined;

        return [...oldData, createOccurrence];
      }
    );
    dispatch(setSelectedEventOccurrence(createOccurrence));
  };

  const handleUpdateSuccess = (updateOccurrence: Occurrence) => {
    queryClient.setQueryData(
      ["occurrences", calendarStartDate, calendarEndDate],
      (oldData: Occurrence[]) => {
        if (!oldData) return undefined;
        const occurrenceIndex = oldData.findIndex(
          (occurrence: Occurrence) => occurrence.id === updateOccurrence.id
        );

        if (occurrenceIndex >= 0) {
          return [
            ...oldData.slice(0, occurrenceIndex),
            updateOccurrence,
            ...oldData.slice(occurrenceIndex + 1),
          ];
        }
        return oldData;
      }
    );
  };

  const handleDeleteSuccess = (deleteOccurrence: Occurrence) => {
    queryClient.setQueryData(
      ["occurrences", calendarStartDate, calendarEndDate],
      (oldData: Occurrence[]) => {
        if (!oldData) return undefined;

        return oldData.filter(
          (occurrence: Occurrence) => occurrence.id !== deleteOccurrence.id
        );
      }
    );
    dispatch(setSelectedEventOccurrence(undefined));
  };

  return (
    <div className="flex flex-col h-full w-full pt-12">
      {selectedEventOccurrence && isSelectedOccurrenceInPast ? (
        <OccurrenceReadOnly onBack={onDrawerClose} />
      ) : (
        <OccurrenceForm
          onCancel={onDrawerClose}
          handleCreateSuccess={handleCreateSuccess}
          handleUpdateSuccess={handleUpdateSuccess}
          handleDeleteSuccess={handleDeleteSuccess}
        />
      )}
    </div>
  );
};

export { OccurrenceCalendarDetails };
