import { OccurenceDetails } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/OccurrenceDetails/OccurenceDetails";
import { OccurencesList } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/OccurrencesList/OccurrencesList";
import type { EventOffering } from "@ms/types/event.types";
import type { Occurrence } from "@ms/types/occurrence.types";
import { useEffect, useState } from "react";
import { OccurrenceView } from "./OccurrencView/OccurrencView";

interface EventOccurrencesProps {
  selectedEvent: EventOffering;
}

const EventOccurrences = ({ selectedEvent }: EventOccurrencesProps) => {
  const [selectedOccurrence, setSelectedOccurrence] = useState<
    Occurrence | undefined
  >();

  const [animating, setAnimating] = useState(false);
  const [showDetails, setShowDetails] = useState(false);

  const handleOccurrenceSelection = (occ: Occurrence) => {
    handleViewChange(setSelectedOccurrence(occ));
  };

  useEffect(() => {
    if (selectedEvent) {
      setShowDetails(false);
    }
  }, [selectedEvent]);

  const handleOccurrenceCreate = (selectedDate: Date) => {
    if (!selectedEvent?.id) return;

    const newOccurrence: Occurrence = {
      eventId: selectedEvent.id,
      scheduleStart: new Date(selectedDate).toISOString(),
      attendees: [],
      leaders: [],
      status: "pending",
      meta: {
        isNew: true,
      },
    };

    handleViewChange(setSelectedOccurrence(newOccurrence));
  };

  const handleViewChange = (callback: any) => {
    setAnimating(true);
    setTimeout(() => {
      callback;
      setShowDetails(true);
      setAnimating(false);
    }, 200);
  };

  const handleBack = () => {
    setAnimating(true);
    setTimeout(() => {
      setSelectedOccurrence(undefined);
      setShowDetails(false);
      setAnimating(false);
    }, 200);
  };

  const isSelectedOccurrenceInPast = selectedOccurrence
    ? new Date(selectedOccurrence.scheduleStart) < new Date()
    : false;

  return (
    <div className="relative w-full h-full overflow-hidden">
      <div
        className={`absolute w-full h-full transition-all duration-300 ease-in-out
          ${showDetails ? "opacity-0 translate-x-10 pointer-events-none" : "opacity-100 translate-x-0 z-10"}
        `}
      >
        <OccurencesList
          occurences={selectedEvent.occurences}
          onOccurenceSelect={handleOccurrenceSelection}
          onOccurenceCreate={handleOccurrenceCreate}
          selectedEvent={selectedEvent}
        />
      </div>
      <div
        className={`absolute w-full h-full transition-all duration-300 ease-in-out
          ${showDetails ? "opacity-100 translate-x-0 z-20" : "opacity-0 translate-x-10 pointer-events-none"}
        `}
      >
        {selectedOccurrence &&
          (isSelectedOccurrenceInPast ? (
            <OccurrenceView
              occurrence={selectedOccurrence}
              onBack={handleBack}
            />
          ) : (
            <OccurenceDetails
              occurrence={selectedOccurrence}
              selectedEvent={selectedEvent}
              onCancel={handleBack}
            />
          ))}
      </div>
    </div>
  );
};

export { EventOccurrences };
