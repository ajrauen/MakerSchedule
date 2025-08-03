import { useAppSelector } from "@ms/redux/hooks";
import { selectAdminState } from "@ms/redux/slices/adminSlice";
import { Button } from "@mui/material";
import { format } from "date-fns";

interface OccurrencReadOnlyProps {
  onBack: () => void;
}

const OccurrenceReadOnly = ({ onBack }: OccurrencReadOnlyProps) => {
  const { selectedEventOccurrence: occurrence } =
    useAppSelector(selectAdminState);

  // Defensive: handle missing or invalid scheduleStart
  let startDate: Date | null = null;
  let endDate: Date | null = null;
  if (occurrence?.scheduleStart) {
    startDate = new Date(occurrence.scheduleStart);
    if (occurrence.duration) {
      endDate = new Date(startDate.getTime() + occurrence.duration * 60000);
    }
  }

  return (
    <div className="p-3">
      {!occurrence ? (
        <div className="text-red-600">No occurrence selected.</div>
      ) : (
        <>
          <div className="text-sm text-gray-500 mb-2">
            {startDate ? format(startDate, "PPpp") : "No start time"}
            {endDate && ` - ${format(endDate, "PPpp")}`}
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
        </>
      )}
    </div>
  );
};

export { OccurrenceReadOnly };
