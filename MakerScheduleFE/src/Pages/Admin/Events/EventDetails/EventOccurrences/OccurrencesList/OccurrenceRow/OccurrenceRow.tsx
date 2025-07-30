import { getAvailableDomainUserLeaders } from "@ms/api/domain-user.api";
import { deleteOccurrence } from "@ms/api/occurrence.api";
import { queryClient } from "@ms/common/query-client";
import { OccurrenceTime } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/OccurrencesList/OccurrenceTime/OccurrenceTime";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import {
  selectAdminState,
  setSelectedEventOccurrence,
} from "@ms/redux/slices/adminSlice";
import type { Occurrence } from "@ms/types/occurrence.types";
import DeleteIcon from "@mui/icons-material/Delete";
import { IconButton } from "@mui/material";
import { useMutation, useQuery } from "@tanstack/react-query";
import { toast } from "react-toastify";

interface OccurenceRowProps {
  occurrence: Occurrence;
}

const OccurenceRow = ({ occurrence }: OccurenceRowProps) => {
  const query = queryClient
    .getQueryCache()
    .find({ queryKey: ["available-leaders", occurrence.id] });

  const { selectedEvent } = useAppSelector(selectAdminState);
  const dispatch = useAppDispatch();

  const { refetch: fetchAvailAbleLeaders } = useQuery({
    queryKey: ["available-leaders", occurrence.id],
    queryFn: () => {
      const isoString = occurrence.scheduleStart;

      const apiDuration = occurrence.duration ?? selectedEvent?.duration ?? 0;

      if (!apiDuration) return;

      return getAvailableDomainUserLeaders(
        isoString,
        apiDuration,
        occurrence.id ?? "",
        occurrence.leaders?.map((leader) => leader.id) ?? []
      );
    },
    staleTime: 4000,
    enabled: false,
  });

  const { mutate: deleteOccurrenceMutation } = useMutation({
    mutationKey: ["updateMutation"],
    mutationFn: ({ occurrenceId }: { occurrenceId: string }) =>
      deleteOccurrence(occurrenceId),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["event", selectedEvent?.id],
      });
      toast("Occurrence Deleted Successfully");
    },
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

  const handleRowDelete = (evt: React.MouseEvent<HTMLButtonElement>) => {
    evt.stopPropagation();
    if (!occurrence.id) return;
    deleteOccurrenceMutation({
      occurrenceId: occurrence.id,
    });
  };

  const handleRowSelect = (occ: Occurrence) => {
    dispatch(setSelectedEventOccurrence(occ));
  };

  const isPastOccurrence = new Date(occurrence.scheduleStart) < new Date();

  return (
    <li
      key={occurrence.id}
      className="flex items-center px-4 py-3 hover:bg-gray-50 cursor-pointer"
      onClick={() => handleRowSelect(occurrence)}
    >
      <div className="flex grow" onMouseOver={handleRowHover}>
        <div className="flex-1 flex  items-center">
          <OccurrenceTime
            start={occurrence.scheduleStart}
            end={
              new Date(
                new Date(occurrence.scheduleStart).getTime() +
                  (occurrence.duration ?? 0)
              )
            }
          />
          {!occurrence.leaders || occurrence.leaders.length === 0 ? (
            <span className="ml-2 text-red-800">No leaders assigned</span>
          ) : (
            occurrence.leaders.map((leader, idx) => (
              <span key={leader.id} className="ml-2">
                {leader.lastName}
                {idx === (occurrence.leaders?.length ?? 0) - 1 ? "" : ", "}
              </span>
            ))
          )}
        </div>
        {!isPastOccurrence && (
          <IconButton onClick={handleRowDelete} className="!ml-auto">
            <DeleteIcon />
          </IconButton>
        )}
      </div>
    </li>
  );
};

export { OccurenceRow };
