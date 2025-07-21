import { getAvailableDomainUserLeaders } from "@ms/api/domain-user.api";
import { queryClient } from "@ms/common/query-client";
import { OccurrenceTime } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/OccurrencesList/OccurrenceTime/OccurrenceTime";
import type { EventOffering } from "@ms/types/event.types";
import type { Occurrence } from "@ms/types/occurrence.types";
import { useQuery } from "@tanstack/react-query";

interface OccurenceRowProps {
  occurrence: Occurrence;
  onOccurenceSelect: (occurrence: Occurrence) => void;
  selectedEvent: EventOffering;
}

const OccurenceRow = ({
  occurrence,
  onOccurenceSelect,
  selectedEvent,
}: OccurenceRowProps) => {
  const query = queryClient
    .getQueryCache()
    .find({ queryKey: ["available-leaders", occurrence.id] });

  const { refetch: fetchAvailAbleLeaders } = useQuery({
    queryKey: ["available-leaders", occurrence.id],
    queryFn: () => {
      const isoString = occurrence.scheduleStart;

      const apiDuration = occurrence.duration ?? selectedEvent.duration;

      if (!apiDuration) return;

      return getAvailableDomainUserLeaders(
        isoString,
        apiDuration,
        occurrence.leaders
      );
    },
    staleTime: 10000,
    enabled: false,
  });

  const handleRowHover = () => {
    // only fetch if date isnt stale and the occurrence isnt in the past
    if (
      (query?.state.status === "pending" || query?.isStale()) &&
      occurrence.status.toLowerCase() === "pending"
    ) {
      fetchAvailAbleLeaders();
    }
  };

  return (
    <li
      key={occurrence.id}
      className="flex items-center px-4 py-3 hover:bg-gray-50 cursor-pointer"
      onClick={() => onOccurenceSelect(occurrence)}
    >
      <div className="flex grow" onMouseOver={handleRowHover}>
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
