import type { Occurrence } from "@ms/types/occurrence.types";

interface OccurenceDetailsProps {
  occurrence?: Occurrence;
}

const OccurenceDetails = ({ occurrence }: OccurenceDetailsProps) => {
  if (!occurrence) {
    return <div className="p-4 text-gray-500">No occurrence selected.</div>;
  }

  return (
    <div className="p-4 space-y-2 bg-white rounded shadow">
      <div>
        <strong>ID:</strong> {occurrence.id}
      </div>
      <div>
        <strong>Event ID:</strong> {occurrence.eventId}
      </div>
      <div>
        <strong>Start:</strong>{" "}
        {new Date(occurrence.scheduleStart).toLocaleString()}
      </div>
      {occurrence.duration !== undefined && (
        <div>
          <strong>Duration:</strong> {occurrence.duration} min
        </div>
      )}
      {occurrence.attendees && (
        <div>
          <strong>Attendees:</strong> {occurrence.attendees.join(", ")}
        </div>
      )}
      {occurrence.leaders && (
        <div>
          <strong>Leaders:</strong> {occurrence.leaders.join(", ")}
        </div>
      )}
    </div>
  );
};

export { OccurenceDetails };
