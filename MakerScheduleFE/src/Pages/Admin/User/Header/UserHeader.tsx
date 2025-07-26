import Select from "@ms/Components/FormComponents/FormSelect/Select/Select";
import { TextSearch } from "@ms/Components/TextSearch/TextSearch";
import AddIcon from "@mui/icons-material/Add";
import { Button } from "@mui/material";
import { useMemo } from "react";

interface UsersHeaderProps {
  onCreateEvent: () => void;
  onSearch: (value: string | undefined) => void;
  onFilterChange: (value: string) => void;
  roles: string[];
}

const UserHeader = ({
  onCreateEvent,
  onSearch,
  onFilterChange,
  roles,
}: UsersHeaderProps) => {
  const roleOptions = useMemo(() => {
    if (!roles) return [];
    let roleOptions = roles.map((role) => ({ value: role, label: role }));
    roleOptions.unshift({ value: "", label: "All Roles" });

    return roleOptions;
  }, [roles]);

  return (
    <div className="flex items-end gap-6">
      <TextSearch onSearch={onSearch} />
      <Select
        autoWidth={true}
        name="userType"
        label="User Type"
        variant="standard"
        className="w-64"
        options={roleOptions}
        onChange={(event) => {
          const value =
            event.target && typeof event.target.value === "string"
              ? event.target.value
              : "";
          onFilterChange(value);
        }}
      />

      <Button
        onClick={onCreateEvent}
        startIcon={<AddIcon />}
        variant="text"
        className="ml-auto "
      >
        User
      </Button>
    </div>
  );
};
export { UserHeader };
