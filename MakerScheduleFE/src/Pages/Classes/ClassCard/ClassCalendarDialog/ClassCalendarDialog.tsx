import { ClassCalendar } from "@ms/Pages/Classes/ClassCard/ClassCalendarDialog/ClassCalendar/ClassCalendar";
import type { EventOffering } from "@ms/types/event.types";
import { Dialog, IconButton } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { ClassDescription } from "@ms/Pages/Classes/ClassCard/ClassDescription/ClassDescription";
import { useQuery } from "@tanstack/react-query";
import { getEvent } from "@ms/api/event.api";

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
  console.log(classEvent, isOpen);

  const { data: eventData, isLoading } = useQuery({
    queryKey: ["event", classEvent.id],
    queryFn: async () => {
      return getEvent(classEvent.id!);
    },
    enabled: !!classEvent.id && isOpen,
  });

  return (
    <Dialog open={isOpen} maxWidth="md" className="p-4">
      <div className="ml-auto">
        <IconButton aria-label="close" onClick={onClose}>
          <CloseIcon />
        </IconButton>
      </div>

      <div className="flex p-4">
        {isLoading ? (
          <div>Loading...</div>
        ) : (
          <>
            <ClassCalendar event={eventData} />
            <ClassDescription event={classEvent} />
          </>
        )}
      </div>
    </Dialog>
  );
};

export { ClassCalendarDialog };
