import { ClassCalendar } from "@ms/Pages/Classes/ClassCard/ClassCalendarDialog/ClassCalendar/ClassCalendar";
import type { EventOffering } from "@ms/types/event.types";
import {
  Dialog,
  DialogTitle,
  IconButton,
  useMediaQuery,
  useTheme,
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { ClassDescription } from "@ms/Pages/Classes/ClassCard/ClassDescription/ClassDescription";
import { useQuery } from "@tanstack/react-query";
import { getEvent } from "@ms/api/event.api";
import { isSameDay } from "date-fns";
import { useMemo, useState } from "react";
import { RegisterForClass } from "@ms/Pages/Classes/ClassCard/ClassCalendarDialog/RegisterForClass/RegisterForClass";
import type { Occurrence } from "@ms/types/occurrence.types";
import { Login } from "@ms/common/Login/Login";

interface ClassCalendarDialogProps {
  classEvent: EventOffering;
  isOpen: boolean;
  onClose: () => void;
  setSelectedEventOccurrence: (occurrence: Occurrence | undefined) => void;
  selectedEventOccurrence: Occurrence | undefined;
  showClassRegistration: boolean;
  setShowClassRegistration: (show: boolean) => void;
}

const ClassCalendarDialog = ({
  classEvent,
  isOpen,
  onClose,
  selectedEventOccurrence,
  setSelectedEventOccurrence,
  showClassRegistration,
  setShowClassRegistration,
}: ClassCalendarDialogProps) => {
  const [showLoginForm, setShowLoginForm] = useState(false);

  const { data: eventData, isLoading } = useQuery({
    queryKey: ["event", classEvent.id],
    queryFn: async () => {
      return getEvent(classEvent.id!);
    },
    enabled: !!classEvent.id && isOpen,
  });

  const handleDateSelect = (date: Date) => {
    const event = eventData?.occurrences?.find((occurrence) =>
      isSameDay(date, new Date(occurrence.scheduleStart))
    );
    setSelectedEventOccurrence(event);
  };

  const handleOnClose = () => {
    setSelectedEventOccurrence(undefined);
    setShowClassRegistration(false);
    onClose();
  };

  const title = useMemo(() => {
    if (selectedEventOccurrence && showClassRegistration) {
      return `Register for ${classEvent.eventName}`;
    }

    return classEvent.eventName;
  }, [selectedEventOccurrence, showClassRegistration, classEvent.eventName]);

  const theme = useTheme();
  const fullScreen = useMediaQuery(theme.breakpoints.down("md"));

  return (
    <Dialog
      open={isOpen}
      maxWidth="lg"
      title={title}
      fullWidth
      fullScreen={fullScreen}
    >
      <DialogTitle className="flex">
        <span>{title}</span>
        {!showClassRegistration && (
          <div className="ml-auto">
            <IconButton aria-label="close" onClick={handleOnClose}>
              <CloseIcon />
            </IconButton>
          </div>
        )}
      </DialogTitle>

      {isLoading ? (
        <div>Loading...</div>
      ) : (
        <>
          {showLoginForm ? (
            <Login
              onClose={() => setShowLoginForm(false)}
              showWelcomeMessage={false}
            />
          ) : showClassRegistration && selectedEventOccurrence ? (
            <RegisterForClass
              event={classEvent}
              occurrence={selectedEventOccurrence}
              closeRegistration={() => setShowClassRegistration(false)}
            />
          ) : (
            <div className="flex flex-col gap-16 px-4 pb-4 lg:h-[450px] lg:flex-row lg:gap-0 pt-6">
              <div className="lg:min-w-[430px] lg:mr-8 h-full">
                <ClassCalendar
                  event={eventData}
                  onDateSelect={handleDateSelect}
                />
              </div>

              <ClassDescription
                event={classEvent}
                occurrence={selectedEventOccurrence}
                onShowRegistration={setShowClassRegistration}
                setShowLoginView={setShowLoginForm}
              />
            </div>
          )}
        </>
      )}
    </Dialog>
  );
};

export { ClassCalendarDialog };
