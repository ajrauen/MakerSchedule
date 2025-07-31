import { OccurenceDetails } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/OccurrenceDetails/OccurenceDetails";
import type { Occurrence } from "@ms/types/occurrence.types";
import { useEffect, useState } from "react";
import { OccurrenceView } from "./OccurrencView/OccurrencView";
import { OccurrencesList } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/OccurrencesList/OccurrencesList";
import {
  selectAdminState,
  setSelectedEventOccurrence,
} from "@ms/redux/slices/adminSlice";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";

interface EventOccurrencesProps {}

const EventOccurrences = ({}: EventOccurrencesProps) => {
  const [animating, setAnimating] = useState(false);
  const [showDetails, setShowDetails] = useState(false);
  const { selectedEvent, selectedEventOccurrence } =
    useAppSelector(selectAdminState);
  const dispatch = useAppDispatch();

  useEffect(() => {
    if (selectedEventOccurrence) {
      setShowDetails(true);
    } else {
      setShowDetails(false);
    }
  }, [selectedEventOccurrence]);

  useEffect(() => {
    if (selectedEvent && !selectedEventOccurrence) {
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

    handleViewChange(dispatch(setSelectedEventOccurrence(newOccurrence)));
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
      dispatch(setSelectedEventOccurrence(undefined));
      setShowDetails(false);
      setAnimating(false);
    }, 200);
  };

  const isSelectedOccurrenceInPast = selectedEventOccurrence
    ? new Date(selectedEventOccurrence.scheduleStart) < new Date()
    : false;

  return (
    <div className="relative w-full h-full overflow-hidden">
      <div
        className={`absolute w-full h-full transition-all duration-300 ease-in-out
          ${showDetails ? "opacity-0 translate-x-10 pointer-events-none" : "opacity-100 translate-x-0 z-10"}
        `}
      >
        <OccurrencesList onOccurrenceCreate={handleOccurrenceCreate} />
      </div>
      <div
        className={`absolute w-full h-full transition-all duration-300 ease-in-out
          ${showDetails ? "opacity-100 translate-x-0 z-20" : "opacity-0 translate-x-10 pointer-events-none"}
        `}
      >
        {selectedEventOccurrence &&
          (isSelectedOccurrenceInPast ? (
            <OccurrenceView
              occurrence={selectedEventOccurrence}
              onBack={handleBack}
            />
          ) : (
            <OccurenceDetails onCancel={handleBack} />
          ))}
      </div>
    </div>
  );
};

export { EventOccurrences };
