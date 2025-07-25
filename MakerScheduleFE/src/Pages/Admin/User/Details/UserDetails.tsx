import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import type { DomainUser } from "@ms/types/domain-user.types";
import { Button } from "@mui/material";

interface UserDetailsProps {
  selectedUser: DomainUser;
}

const UserDetails = ({ selectedUser }: UserDetailsProps) => {
  return (
    <div>
      Name: {selectedUser.firstName} {selectedUser.lastName}
      <FormSelect
        options={[
          { label: "Admin", value: "admin" },
          { label: "User", value: "user" },
        ]}
        value={selectedUser.role}
        onChange={(value) => console.log(value)}
      />
      <div>
        <Button variant="contained" type="submit">
          Save
        </Button>
      </div>
    </div>
  );
};

export { UserDetails };
