import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import type { EventTag } from "@ms/types/event-tags.types";

interface AdminEventTagsTableProps {
  eventTags: EventTag[];
  onEdit: (eventTag: EventTag) => void;
  selectedEventTag?: EventTag;
}

const AdminEventTagsTable = ({
  onEdit,
  selectedEventTag,
  eventTags = [],
}: AdminEventTagsTableProps) => {
  return (
    <TableContainer className="overflow-auto h-full">
      <Table stickyHeader aria-label="sticky table">
        <TableHead>
          <TableRow>
            <TableCell>Tag Name</TableCell>
            <TableCell>Events Count</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {eventTags.map((tag, idx) => (
            <TableRow
              key={idx}
              hover
              selected={selectedEventTag?.id == tag.id}
              onClick={() => {
                onEdit(tag);
              }}
            >
              <TableCell>{tag.name}</TableCell>

              <TableCell>{tag.eventIds?.length ?? 0} events</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export { AdminEventTagsTable };
