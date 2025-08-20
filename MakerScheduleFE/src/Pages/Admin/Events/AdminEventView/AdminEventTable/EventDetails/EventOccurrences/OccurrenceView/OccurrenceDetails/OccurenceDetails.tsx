import type { Occurrence } from "@ms/types/occurrence.types";
import { useQueryClient } from "@tanstack/react-query";

import { setSelectedEventOccurrence } from "@ms/redux/slices/adminSlice";
import { useAppDispatch } from "@ms/redux/hooks";
import type { EventOffering } from "@ms/types/event.types";
import { OccurrenceForm } from "@ms/Pages/Admin/Events/Common/OccurrenceForm/OccurrenceForm";

interface OccurrenceDetailsProps {
  onCancel: () => void;
}

const OccurrenceDetails = ({ onCancel }: OccurrenceDetailsProps) => {
  const dispatch = useAppDispatch();

  const queryClient = useQueryClient();

  const handleCreateSuccess = (occurrence: Occurrence) => {
    queryClient.setQueryData(
      ["event", occurrence.eventId],
      (oldData: EventOffering) => {
        if (!oldData) return undefined;

        return {
          ...oldData,
          occurrences: [...(oldData.occurrences || []), occurrence],
        };
      }
    );
    dispatch(setSelectedEventOccurrence(occurrence));
  };

  const handleUpdateSuccess = (updateOccurrence: Occurrence) => {
    queryClient.setQueryData(
      ["event", updateOccurrence.eventId],
      (oldData: EventOffering) => {
        if (!oldData || !oldData.occurrences) return undefined;
        const occurrenceIndex = oldData.occurrences.findIndex(
          (occurrence: Occurrence) => occurrence.id === updateOccurrence.id
        );
        if (occurrenceIndex >= 0) {
          return {
            ...oldData,
            occurrences: oldData.occurrences.map(
              (occurrence: Occurrence, idx) =>
                idx === occurrenceIndex ? updateOccurrence : occurrence
            ),
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
      handleCreateSuccess={handleCreateSuccess}
      handleUpdateSuccess={handleUpdateSuccess}
    />
  );
};

export { OccurrenceDetails };
