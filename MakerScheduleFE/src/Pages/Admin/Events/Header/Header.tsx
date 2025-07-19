import AddIcon from "@mui/icons-material/Add";
import { Button } from "@mui/material";

interface EventsHeaderProps {
  onCreateEvent: () => void;
}

const EventsHeader = ({ onCreateEvent }: EventsHeaderProps) => {
  return (
    <div>
      <Button onClick={onCreateEvent} startIcon={<AddIcon />} variant="text">
        Create Event
      </Button>
    </div>
  );
};

export { EventsHeader };
