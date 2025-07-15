import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import Button from "@mui/material/Button";
import type { EventOffering, EventType } from "@ms/types/event.types";
import { durationMap } from "@ms/Pages/Admin/Events/utils/event.utils";

interface AdminEventTableProps {
  events: EventOffering[];
  eventTypes: EventType;
  onEdit: (event: EventOffering) => void;
  selectedEvent?: EventOffering;
}

const AdminEventsTable = ({
  eventTypes,
  onEdit,
  selectedEvent,
  events = [],
}: AdminEventTableProps) => {
  return (
    <TableContainer component={Paper}>
      <Table stickyHeader aria-label="sticky table">
        <TableHead>
          <TableRow>
            <TableCell>Event Name</TableCell>
            <TableCell>Description</TableCell>
            <TableCell>Event Type</TableCell>
            <TableCell>Duration</TableCell>
            <TableCell>Image</TableCell>
            <TableCell>Action</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {events.map((row, idx) => (
            <TableRow
              key={idx}
              hover
              selected={selectedEvent?.id == row.id}
              onClick={() => {
                onEdit(row);
              }}
            >
              <TableCell>{row.eventName}</TableCell>
              <TableCell className="w-2/5">
                <div
                  className="line-clamp-2 break-words overflow-hidden"
                  title={row.description}
                >
                  {row.description}
                </div>
              </TableCell>
              <TableCell>{eventTypes[row.eventType]}</TableCell>
              <TableCell>{row.duration && durationMap[row.duration]}</TableCell>
              <TableCell>
                <img
                  src={
                    row.fileUrl ??
                    "https://t3.ftcdn.net/jpg/03/37/46/98/360_F_337469861_iFRwd4Ia15Ihuwfa8fEDA9OKPhIVsgZR.jpg"
                  }
                  alt={row.eventName}
                  width={80}
                  height={60}
                  style={{ objectFit: "cover" }}
                />
              </TableCell>
              <TableCell>
                <Button
                  variant="outlined"
                  color="secondary"
                  size="small"
                  style={{ marginLeft: 8 }}
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

export { AdminEventsTable };
