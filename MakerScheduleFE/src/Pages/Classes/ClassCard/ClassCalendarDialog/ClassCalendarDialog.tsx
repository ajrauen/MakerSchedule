import { ClassCalendar } from "@ms/Pages/Classes/ClassCard/ClassCalendarDialog/ClassCalendar/ClassCalendar";
import type { EventOffering } from "@ms/types/event.types";
import { Dialog, IconButton } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { ClassDescription } from "@ms/Pages/Classes/ClassCard/ClassDescription/ClassDescription";

interface ClassCalendarDialogProps {
  classEvent: EventOffering;
  isOpen: boolean;
  onClose: () => void;
}

const ClassCalendarDialog = ({
  classEvent,
  isOpen,
  onClose,
}: ClassCalendarDialogProps) => {
  return (
    <Dialog open={isOpen} maxWidth="md" className="p-4">
      <div className="ml-auto">
        <IconButton aria-label="close" onClick={onClose}>
          <CloseIcon />
        </IconButton>
      </div>

      <div className="flex p-4">
        <ClassCalendar event={classEvent} />
        <ClassDescription event={classEvent} />
      </div>
    </Dialog>
  );
};

export { ClassCalendarDialog };
