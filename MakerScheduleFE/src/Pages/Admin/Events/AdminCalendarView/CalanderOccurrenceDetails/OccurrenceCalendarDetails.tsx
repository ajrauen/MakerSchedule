import { isOccurrenceInPast } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/EventDetails/EventOccurrences/Occurrence.utils";
import { OccurrenceForm } from "@ms/Pages/Admin/Events/Common/OccurrenceForm/OccurrenceForm";
import { OccurrenceReadOnly } from "@ms/Pages/Admin/Events/Common/OccurrenceReadOnly/OccurrenceReadOnly";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import {
  selectAdminState,
  setAdminDrawerOpen,
  setSelectedEventOccurrence,
} from "@ms/redux/slices/adminSlice";
import type { Occurrence } from "@ms/types/occurrence.types";
import { useQueryClient } from "@tanstack/react-query";
import type { AxiosResponse } from "axios";
import { useMemo } from "react";

interface OccurrenceCalendarDetailsProps {
  calendarStartDate: Date | null;
  calendarEndDate: Date | null;
  selectedEventType?: string;
}

const OccurrenceCalendarDetails = ({
  calendarStartDate,
  calendarEndDate,
  selectedEventType,
}: OccurrenceCalendarDetailsProps) => {
  const { selectedEventOccurrence } = useAppSelector(selectAdminState);
  const isSelectedOccurrenceInPast = useMemo(() => {
    if (!selectedEventOccurrence) return false;

    return isOccurrenceInPast(selectedEventOccurrence);
  }, [selectedEventOccurrence]);

  const queryClient = useQueryClient();
  const dispatch = useAppDispatch();

  const handleSaveSuccess = (createOccurrence: Occurrence) => {
    queryClient.setQueryData(
      ["occurrences", calendarStartDate, calendarEndDate, selectedEventType],
      (oldData: AxiosResponse<Occurrence[]>) => {
        if (!oldData) return undefined;

        return {
          ...oldData,
          data: [...(oldData.data || []), createOccurrence],
        };
      }
    );
    dispatch(setSelectedEventOccurrence(createOccurrence));
  };

  const handleUpdateSuccess = (updateOccurrence: Occurrence) => {
    queryClient.setQueryData(
      ["occurrences", calendarStartDate, calendarEndDate, selectedEventType],
      (oldData: AxiosResponse<Occurrence[]>) => {
        if (!oldData || !oldData.data) return undefined;
        const occurrenceIndex = oldData.data.findIndex(
          (occurrence: Occurrence) => occurrence.id === updateOccurrence.id
        );

        if (occurrenceIndex >= 0) {
          return {
            ...oldData,
            data: oldData.data.map((occurrence: Occurrence, idx) =>
              idx === occurrenceIndex ? updateOccurrence : occurrence
            ),
          };
        }
        return oldData;
      }
    );
  };

  const handleDeleteSuccess = (deleteOccurrence: Occurrence) => {
    queryClient.setQueryData(
      ["occurrences", calendarStartDate, calendarEndDate, selectedEventType],
      (oldData: AxiosResponse<Occurrence[]>) => {
        if (!oldData || !oldData.data) return undefined;

        return {
          ...oldData,
          data: oldData.data.filter(
            (occurrence: Occurrence) => occurrence.id !== deleteOccurrence.id
          ),
        };
      }
    );
    dispatch(setSelectedEventOccurrence(undefined));
  };

  const handleCloseDrawer = () => {
    dispatch(setSelectedEventOccurrence(undefined));
    dispatch(setAdminDrawerOpen(false));
  };

  return (
    <div className="flex flex-col h-full w-full pt-12">
      {selectedEventOccurrence && isSelectedOccurrenceInPast ? (
        <OccurrenceReadOnly onBack={handleCloseDrawer} />
      ) : (
        <OccurrenceForm
          onCancel={handleCloseDrawer}
          handleSaveSuccess={handleSaveSuccess}
          handleUpdateSuccess={handleUpdateSuccess}
          handleDeleteSuccess={handleDeleteSuccess}
        />
      )}
    </div>
  );
};

export { OccurrenceCalendarDetails };
