import { useAppSelector } from "@ms/redux/hooks";
import { selectAdminState } from "@ms/redux/slices/adminSlice";
import { Button } from "@mui/material";

interface OccurrencReadOnlyProps {
  onBack: () => void;
}

const OccurrenceReadOnly = ({ onBack }: OccurrencReadOnlyProps) => {
  const { selectedEventOccurrence: occurrence } =
    useAppSelector(selectAdminState);

  return (
    <div className="p-3">
      {!occurrence ? (
        <div className="text-red-600">No occurrence selected.</div>
      ) : (
        <>
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
