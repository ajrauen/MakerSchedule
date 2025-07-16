import { OccurrenceTime } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/OccurrencesList/OccurrenceTime/OccurrenceTime";
import { IconButton } from "@mui/material";
import VisibilityIcon from "@mui/icons-material/Visibility";
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
    >
      <div className="flex grow">
        <OccurrenceTime
          start={occurrence.scheduleStart}
          end={occurrence.scheduleStart + (occurrence.duration ?? 0)}
        />
      </div>

      <IconButton onClick={() => onOccurenceSelect(occurrence)}>
        <VisibilityIcon />
      </IconButton>
    </li>
  );
};

export { OccurenceRow };
