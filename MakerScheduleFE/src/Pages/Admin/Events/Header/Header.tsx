import AddIcon from "@mui/icons-material/Add";
import { Button } from "@mui/material";

interface EventsHeaderProps {
  onOpenDrawer: () => void;
}

const EventsHeader = ({ onOpenDrawer }: EventsHeaderProps) => {
  return (
    <div>
      <Button onClick={onOpenDrawer} startIcon={<AddIcon />} variant="text">
        Create Event
      </Button>
    </div>
  );
};

export { EventsHeader };
