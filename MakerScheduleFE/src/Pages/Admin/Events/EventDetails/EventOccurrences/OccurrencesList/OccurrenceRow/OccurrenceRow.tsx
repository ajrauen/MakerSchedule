import { OccurrenceTime } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/OccurrencesList/OccurrenceTime/OccurrenceTime";
import type { Occurrence } from "@ms/types/occurrence.types";

interface OccurenceRowProps {
  occurrence: Occurrence;
  onOccurenceSelect: (occurrence: Occurrence) => void;
}

const OccurenceRow = ({ occurrence, onOccurenceSelect }: OccurenceRowProps) => {
  return (
    <li
      key={occurrence.id}
      className="flex items-center px-4 py-3 hover:bg-gray-50 cursor-pointer"
      onClick={() => onOccurenceSelect(occurrence)}
    >
      <div className="flex grow">
        <OccurrenceTime
          start={occurrence.scheduleStart}
          end={
            new Date(
              new Date(occurrence.scheduleStart).getTime() +
                (occurrence.duration ?? 0)
            )
          }
        />
      </div>
    </li>
  );
};

export { OccurenceRow };
