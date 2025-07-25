import { zodResolver } from "@hookform/resolvers/zod";
import { UserForm } from "@ms/Components/UserForm/UserForm";
import type {
  DomainUser,
  RegisterDomainUserRequest,
} from "@ms/types/domain-user.types";
import { Button, Select } from "@mui/material";
import { useEffect } from "react";
import { useForm } from "react-hook-form";

interface UserDetailsProps {
  selectedUser: DomainUser;
}

const UserDetails = ({ selectedUser }: UserDetailsProps) => {
  return (
    <div>
      Name: {selectedUser.firstName} {selectedUser.lastName}
      <Select
        value={selectedUser.role}
        onChange={(e) => console.log(e.target.value)}
      >
        <option value="admin">Admin</option>
        <option value="user">User</option>
      </Select>
      <div>
        <Button variant="contained" type="submit">
          Save
        </Button>
      </div>
    </div>
  );
};

export { UserDetails };
