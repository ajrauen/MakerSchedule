import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Button from "@mui/material/Button";
import type { EventType } from "@ms/types/event.types";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import {
  selectAdminState,
  setAdminDrawerOpen,
  setSelectedEventType,
} from "@ms/redux/slices/adminSlice";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import { deleteEventType } from "@ms/api/eventTypes.api";
import { ConfirmationDialog } from "@ms/Components/Dialogs/Confirmation";

interface AdminEventTableProps {
  eventTypes: EventType[];
}

const AdminEventTypesTable = ({ eventTypes }: AdminEventTableProps) => {
  const [eventTypeToDelete, setEventTypeToDelete] = useState<
    EventType | undefined
  >();

  const dispatch = useAppDispatch();
  const queryClient = useQueryClient();
  const { selectedEventType } = useAppSelector(selectAdminState);

  const { mutate: deleteEventMutation } = useMutation({
    mutationKey: ["deleteEvent"],
    mutationFn: deleteEventType,
    onSuccess: (res, deleteEventId) => {
      if (!eventTypeToDelete) return;

      queryClient.setQueryData(["eventTypes"], (oldEvents: EventType[]) => {
        if (!oldEvents) return oldEvents;
        return oldEvents.filter((evt) => evt.id !== deleteEventId);
      });

      if (selectedEventType?.id === deleteEventId) {
        setSelectedEventType(undefined);
        dispatch(setAdminDrawerOpen(false));
      }

      setEventTypeToDelete(undefined);
    },
  });

  const handleDeletClick = (evt: React.MouseEvent, row: EventType) => {
    evt.stopPropagation();
    setEventTypeToDelete(row);
  };

  const handleRowClick = (row: EventType) => {
    dispatch(setSelectedEventType(row));
    dispatch(setAdminDrawerOpen(true));
  };

  const handleCancelDeleteEvent = () => setEventTypeToDelete(undefined);
  const handleConfirmDeleteEvent = () => {
    if (!eventTypeToDelete?.id) return;
    deleteEventMutation(eventTypeToDelete.id);
  };

  return (
    <>
      <TableContainer className="overflow-auto h-full">
        <Table stickyHeader aria-label="sticky table">
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {eventTypes.map((row, idx) => (
              <TableRow
                key={idx}
                hover
                selected={selectedEventType?.id == row.id}
                onClick={() => {
                  handleRowClick(row);
                }}
              >
                <TableCell>{row.name}</TableCell>
                <TableCell>
                  <Button
                    variant="outlined"
                    color="secondary"
                    size="small"
                    className="m-2"
                    onClick={(evt) => handleDeletClick(evt, row)}
                  >
                    Delete
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <ConfirmationDialog
        open={!!eventTypeToDelete}
        onCancel={handleCancelDeleteEvent}
        onConfirm={handleConfirmDeleteEvent}
        title="Delete Event"
        details="You sure? Because you cant come back from this!"
      />
    </>
  );
};

export { AdminEventTypesTable };
