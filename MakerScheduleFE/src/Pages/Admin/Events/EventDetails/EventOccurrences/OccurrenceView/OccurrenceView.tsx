import { isOccurrenceInPast } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/Occurrence.utils";
import { OccurrenceDetails } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/OccurrenceView/OccurrenceDetails/OccurenceDetails";
import { OccurrenceReadOnly } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/OccurrenceView/OccurrenceReadOnly/OccurrenceReadOnly";
import { useAppSelector } from "@ms/redux/hooks";
import { selectAdminState } from "@ms/redux/slices/adminSlice";

interface OccurrenceViewProps {
  onBack: () => void;
}

const OccurrenceView = ({ onBack }: OccurrenceViewProps) => {
  const { selectedEventOccurrence } = useAppSelector(selectAdminState);

  return (
    <div>
      {selectedEventOccurrence &&
        (isOccurrenceInPast(selectedEventOccurrence) ? (
          <OccurrenceReadOnly onBack={onBack} />
        ) : (
          <OccurrenceDetails onCancel={onBack} />
        ))}
    </div>
  );
};

export { OccurrenceView };
