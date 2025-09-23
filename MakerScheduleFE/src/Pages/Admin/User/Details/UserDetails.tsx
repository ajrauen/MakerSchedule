import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import type { DomainUser } from "@ms/types/domain-user.types";
import { Button } from "@mui/material";
import EmailIcon from "@mui/icons-material/Email";

interface UserDetailsProps {
  selectedUser: DomainUser;
}

const UserDetails = ({ selectedUser }: UserDetailsProps) => {
  const handleEmailClick = () => {
    if (selectedUser.email) {
      window.open(`mailto:${selectedUser.email}`);
    }
  };

  return (
    <div className="space-y-4">
      <h2 className="text-xl font-semibold mb-4">
        {selectedUser.firstName} {selectedUser.lastName}
      </h2>

      {selectedUser.email && (
        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Email
          </label>
          <div className="flex items-center space-x-2">
            <span className="text-sm text-gray-600 flex-1">
              {selectedUser.email}
            </span>
            <Button
              variant="outlined"
              size="small"
              startIcon={<EmailIcon />}
              onClick={handleEmailClick}
            >
              Email User
            </Button>
          </div>
        </div>
      )}

      <div className="mb-4">
        <label className="block text-sm font-medium text-gray-700 mb-2">
          Roles
        </label>
        <FormSelect
          options={[
            { label: "Admin", value: "admin" },
            { label: "User", value: "user" },
          ]}
          value={selectedUser.roles?.[0] || ""}
          onChange={(value) => console.log(value)}
        />
      </div>

      <div className="pt-4 border-t">
        <Button variant="contained" type="submit">
          Save Changes
        </Button>
      </div>
    </div>
  );
};

export { UserDetails };
