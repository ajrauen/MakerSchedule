import { CreateEvent } from "@ms/Components/CreateEvent/CreateEvent";
import AddIcon from "@mui/icons-material/Add";
import { Button } from "@mui/material";
import { useState } from "react";

const EventsHeader = () => {
  const [isCreateEventFormOpen, setIsCreateEventFormOpen] = useState(false);

  const handleCloseCraeteEventForm = () => setIsCreateEventFormOpen(false);

  return (
    <div>
      <Button
        onClick={() => setIsCreateEventFormOpen(true)}
        startIcon={<AddIcon />}
      >
        Create Event
      </Button>
      <CreateEvent
        isOpen={isCreateEventFormOpen}
        onClose={handleCloseCraeteEventForm}
      />
    </div>
  );
};

export { EventsHeader };
