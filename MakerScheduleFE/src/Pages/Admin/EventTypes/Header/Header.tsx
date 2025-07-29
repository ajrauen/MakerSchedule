import { TextSearch } from "@ms/Components/TextSearch/TextSearch";
import AddIcon from "@mui/icons-material/Add";
import { Button } from "@mui/material";

interface EventTypesHeaderProps {
  onCreateEventType: () => void;
  onSearch: (value: string | undefined) => void;
}

const EventTypesHeader = ({
  onCreateEventType,
  onSearch,
}: EventTypesHeaderProps) => {
  return (
    <div className="flex items-end gap-6  mt-3">
      <div className="grow">
        <TextSearch onSearch={onSearch} />
      </div>
      <Button
        onClick={onCreateEventType}
        startIcon={<AddIcon />}
        variant="text"
      >
        Event Type
      </Button>
    </div>
  );
};

export { EventTypesHeader };
