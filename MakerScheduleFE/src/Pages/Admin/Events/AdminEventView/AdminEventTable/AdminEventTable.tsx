import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import type { EventOffering } from "@ms/types/event.types";
import { durationMap } from "@ms/Pages/Admin/Events/AdminEventView/utils/event.utils";

interface AdminEventTableProps {
  events: EventOffering[];
  onEventSelect: (event: EventOffering) => void;
  selectedEvent?: EventOffering;
}

const AdminEventTable = ({
  events = [],
  onEventSelect,
  selectedEvent,
}: AdminEventTableProps) => {
  return (
    <TableContainer className="overflow-auto h-full">
      <Table stickyHeader aria-label="sticky table">
        <TableHead>
          <TableRow>
            <TableCell>Event Name</TableCell>
            <TableCell>Description</TableCell>
            <TableCell>Event Type</TableCell>
            <TableCell>Duration</TableCell>
            <TableCell>Image</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {events.map((row, idx) => (
            <TableRow
              key={idx}
              hover
              selected={selectedEvent?.id === row.id}
              onClick={() => onEventSelect(row)}
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
              <TableCell>{row.eventType?.name}</TableCell>
              <TableCell>{row.duration && durationMap[row.duration]}</TableCell>
              <TableCell>
                <img
                  src={
                    row.thumbnailUrl ??
                    "https://t3.ftcdn.net/jpg/03/37/46/98/360_F_337469861_iFRwd4Ia15Ihuwfa8fEDA9OKPhIVsgZR.jpg"
                  }
                  alt={row.eventName}
                  width={80}
                  height={60}
                  style={{ objectFit: "cover" }}
                />
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export { AdminEventTable };
