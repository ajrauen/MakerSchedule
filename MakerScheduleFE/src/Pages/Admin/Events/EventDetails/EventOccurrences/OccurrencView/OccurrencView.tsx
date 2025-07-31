import type { Occurrence } from "@ms/types/occurrence.types";
import { Button } from "@mui/material";
import { format } from "date-fns";

interface OccurrenceViewProps {
  occurrence: Occurrence;
  onBack: () => void;
}

const OccurrenceView = ({ occurrence, onBack }: OccurrenceViewProps) => {
  // Calculate end time if duration is present
  const startDate = new Date(occurrence.scheduleStart);
  const endDate = occurrence.duration
    ? new Date(startDate.getTime() + occurrence.duration * 60000)
    : undefined;

  return (
    <div className="p-3">
      <div className="text-sm text-gray-500 mb-2">
        {format(startDate, "PPpp")} {endDate && `- ${format(endDate, "PPpp")}`}
      </div>
      <div className="mb-2">
        <span className="font-semibold">Status:</span>{" "}
        {occurrence.status || "N/A"}
      </div>
      {occurrence.attendees && occurrence.attendees.length > 0 && (
        <div className="mb-2">
          <span className="font-semibold">Attendees:</span>{" "}
          {occurrence.attendees
            .map((a) => `${a.firstName} ${a.lastName}`)
            .join(", ")}
        </div>
      )}
      {occurrence.leaders && occurrence.leaders.length > 0 && (
        <div className="mb-2">
          <span className="font-semibold">Leaders:</span>{" "}
          {occurrence.leaders
            .map((l) => `${l.firstName} ${l.lastName}`)
            .join(", ")}
        </div>
      )}
      {occurrence.meta && (
        <div className="text-xs text-gray-400">
          <span className="font-semibold">Meta:</span>{" "}
          {Object.entries(occurrence.meta)
            .map(([k, v]) => `${k}: ${v}`)
            .join(", ")}
        </div>
      )}

      <Button
        variant="outlined"
        color="primary"
        onClick={onBack}
        className="mt-4"
      >
        Back
      </Button>
    </div>
  );
};

export { OccurrenceView };
