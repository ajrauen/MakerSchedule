import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Button from "@mui/material/Button";
import type { EventType } from "@ms/types/event.types";

interface AdminEventTableProps {
  eventTypes: EventType[];
  onEdit: (event: EventType) => void;
  selectedEventType?: EventType;
  onEventTypeDelete: (event: EventType) => void;
}

const AdminEventTypesTable = ({
  onEdit,
  selectedEventType,
  onEventTypeDelete,
  eventTypes,
}: AdminEventTableProps) => {
  const handleDeletClick = (evt: React.MouseEvent, row: EventType) => {
    evt.stopPropagation();
    onEventTypeDelete(row);
  };

  return (
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
                onEdit(row);
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
  );
};

export { AdminEventTypesTable };
