import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Button from "@mui/material/Button";
import type { DomainUser } from "@ms/types/domain-user.types";

interface AdminEventTableProps {
  users: DomainUser[];
  onEdit: (event: DomainUser) => void;
  selectedUser?: DomainUser;
  onEventDelete: (event: DomainUser) => void;
}

const AdminUsersTable = ({
  onEdit,
  selectedUser,
  onEventDelete,
  users = [],
}: AdminEventTableProps) => {
  const handleDeletClick = (evt: React.MouseEvent, row: DomainUser) => {
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
                {row.email ? (
                  <a
                    href={`mailto:${row.email}`}
                    className="line-clamp-2 break-words overflow-hidden text-blue-600 hover:text-blue-800 hover:underline cursor-pointer"
                    title={`Click to email ${row.email}`}
                    onClick={(e) => e.stopPropagation()}
                  >
                    {row.email}
                  </a>
                ) : (
                  <div className="line-clamp-2 break-words overflow-hidden text-gray-400">
                    No email
                  </div>
                )}
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
