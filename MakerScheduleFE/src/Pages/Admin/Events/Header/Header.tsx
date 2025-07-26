import { TextSearch } from "@ms/Components/TextSearch/TextSearch";
import AddIcon from "@mui/icons-material/Add";
import { Button } from "@mui/material";

interface EventsHeaderProps {
  onCreateEvent: () => void;
  onSearch: (value: string | undefined) => void;
}

const EventsHeader = ({ onCreateEvent, onSearch }: EventsHeaderProps) => {
  return (
    <div className="flex items-end gap-6">
      <div className="grow">
        <TextSearch onSearch={onSearch} />
      </div>
      <Button onClick={onCreateEvent} startIcon={<AddIcon />} variant="text">
        Event
      </Button>
    </div>
  );
};

export { EventsHeader };
