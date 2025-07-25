import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Button from "@mui/material/Button";
import type { User } from "@ms/types/users.types";

interface AdminEventTableProps {
  users: User[];
  onEdit: (event: User) => void;
  selectedUser?: User;
  onEventDelete: (event: User) => void;
}

const AdminUsersTable = ({
  onEdit,
  selectedUser,
  onEventDelete,
  users = [],
}: AdminEventTableProps) => {
  const handleDeletClick = (evt: React.MouseEvent, row: User) => {
    evt.stopPropagation();
    onEventDelete(row);
  };

  return (
    <TableContainer className="overflow-auto h-full">
      <Table stickyHeader aria-label="sticky table">
        <TableHead>
          <TableRow>
            <TableCell>User Name</TableCell>
            <TableCell>Email</TableCell>
            <TableCell>Roles</TableCell>
            <TableCell>Action</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {users.map((row, idx) => (
            <TableRow
              key={idx}
              hover
              selected={selectedUser?.id == row.id}
              onClick={() => {
                onEdit(row);
              }}
            >
              <TableCell>{`${row.firstName} ${row.lastName}`}</TableCell>
              <TableCell className="w-2/5">
                <div
                  className="line-clamp-2 break-words overflow-hidden"
                  title={row.email}
                >
                  {row.email}
                </div>
              </TableCell>
              <TableCell>
                {row.roles?.length ? row.roles.join(", ") : "No roles assigned"}
              </TableCell>
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

export { AdminUsersTable };
