import type { Occurrence } from "@ms/types/occurrence.types";
import { useQueryClient } from "@tanstack/react-query";

import { setSelectedEventOccurrence } from "@ms/redux/slices/adminSlice";
import { useAppDispatch } from "@ms/redux/hooks";
import type { AxiosResponse } from "axios";
import type { EventOffering } from "@ms/types/event.types";
import { OccurrenceForm } from "@ms/Pages/Admin/Events/Common/OccurrenceForm/OccurrenceForm";
import { di } from "node_modules/@fullcalendar/core/internal-common";

interface OccurrenceDetailsProps {
  onCancel: () => void;
}

const OccurrenceDetails = ({ onCancel }: OccurrenceDetailsProps) => {
  const dispatch = useAppDispatch();

  const queryClient = useQueryClient();

  const handleSaveSuccess = (occurrence: Occurrence) => {
    queryClient.setQueryData(
      ["event", occurrence.eventId],
      (oldData: AxiosResponse<EventOffering>) => {
        if (!oldData) return undefined;

        return {
          ...oldData,
          data: {
            ...oldData.data,
            occurrences: [...(oldData.data.occurrences || []), occurrence],
          },
        };
      }
    );
    dispatch(setSelectedEventOccurrence(occurrence));
  };

  const handleUpdateSuccess = (updateOccurrence: Occurrence) => {
    queryClient.setQueryData(
      ["event", updateOccurrence.eventId],
      (oldData: AxiosResponse<EventOffering>) => {
        if (!oldData || !oldData.data || !oldData.data.occurrences)
          return undefined;
        const occurrenceIndex = oldData.data.occurrences.findIndex(
          (occurrence: Occurrence) => occurrence.id === updateOccurrence.id
        );
        if (occurrenceIndex >= 0) {
          return {
            ...oldData,
            data: {
              ...oldData.data,
              occurrences: oldData.data.occurrences.map(
                (occurrence: Occurrence, idx) =>
                  idx === occurrenceIndex ? updateOccurrence : occurrence
              ),
            },
          };
        }
        return oldData;
      }
    );
    dispatch(setSelectedEventOccurrence(updateOccurrence));
  };

  return (
    <OccurrenceForm
      onCancel={onCancel}
      handleSaveSuccess={handleSaveSuccess}
      handleUpdateSuccess={handleUpdateSuccess}
    />
  );
};

export { OccurrenceDetails };
