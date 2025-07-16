import { OccurencesList } from "@ms/Pages/Admin/Events/EventDetails/EventOccurences/OccurencesList/OccurencesList";
import { OccurenceDetails } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/OccurrenceDetails/OccurenceDetails";
import type { EventOffering } from "@ms/types/event.types";
import type { Occurrence } from "@ms/types/occurrence.types";
import { useState } from "react";

const mockOccurences: Occurrence[] = [
  // Today
  {
    eventId: 1,
    id: 1,
    scheduleStart: new Date(new Date().setHours(8, 0, 0, 0)).getTime(), // today 8am
    duration: 60 * 60 * 1000, // 60 min
    attendees: [1, 2],
    leaders: [3],
  },
  {
    eventId: 1,
    id: 2,
    scheduleStart: new Date(new Date().setHours(15, 0, 0, 0)).getTime(), // today 3pm
    duration: 90 * 60 * 1000, // 90 min
    attendees: [2, 3],
    leaders: [1],
  },
  // Past 5 days
  {
    eventId: 2,
    id: 3,
    scheduleStart: new Date(
      new Date(Date.now() - 1 * 86400000).setHours(10, 0, 0, 0)
    ).getTime(), // yesterday 10am
    duration: 45 * 60 * 1000, // 45 min
    attendees: [4],
    leaders: [2],
  },
  {
    eventId: 2,
    id: 4,
    scheduleStart: new Date(
      new Date(Date.now() - 3 * 86400000).setHours(13, 0, 0, 0)
    ).getTime(), // 3 days ago 1pm
    duration: 30 * 60 * 1000, // 30 min
    attendees: [1, 4],
    leaders: [3],
  },
  {
    eventId: 3,
    id: 5,
    scheduleStart: new Date(
      new Date(Date.now() - 5 * 86400000).setHours(17, 0, 0, 0)
    ).getTime(), // 5 days ago 5pm
    duration: 120 * 60 * 1000, // 120 min
    attendees: [2, 5],
    leaders: [4],
  },
  // Next 7 days
  {
    eventId: 3,
    id: 6,
    scheduleStart: new Date(
      new Date(Date.now() + 1 * 86400000).setHours(8, 0, 0, 0)
    ).getTime(), // tomorrow 8am
    duration: 75 * 60 * 1000, // 75 min
    attendees: [3, 6],
    leaders: [5],
  },
  {
    eventId: 4,
    id: 7,
    scheduleStart: new Date(
      new Date(Date.now() + 2 * 86400000).setHours(10, 0, 0, 0)
    ).getTime(), // in 2 days 10am
    duration: 60 * 60 * 1000, // 60 min
    attendees: [4, 7],
    leaders: [6],
  },
  {
    eventId: 4,
    id: 8,
    scheduleStart: new Date(
      new Date(Date.now() + 4 * 86400000).setHours(13, 0, 0, 0)
    ).getTime(), // in 4 days 1pm
    duration: 90 * 60 * 1000, // 90 min
    attendees: [5, 8],
    leaders: [7],
  },
  {
    eventId: 5,
    id: 9,
    scheduleStart: new Date(
      new Date(Date.now() + 6 * 86400000).setHours(17, 0, 0, 0)
    ).getTime(), // in 6 days 5pm
    duration: 30 * 60 * 1000, // 30 min
    attendees: [6, 9],
    leaders: [8],
  },
  {
    eventId: 5,
    id: 10,
    scheduleStart: new Date(
      new Date(Date.now() + 7 * 86400000).setHours(19, 0, 0, 0)
    ).getTime(), // in 7 days 7pm
    duration: 60 * 60 * 1000, // 60 min
    attendees: [7, 10],
    leaders: [9],
  },
];

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
    setAnimating(true);
    setTimeout(() => {
      setSelectedOccurrence(occ);
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

  return (
    <div className="relative w-full h-full overflow-hidden">
      <div
        className={`absolute w-full h-full transition-all duration-300 ease-in-out
          ${showDetails ? "opacity-0 translate-x-10 pointer-events-none" : "opacity-100 translate-x-0 z-10"}
        `}
      >
        <OccurencesList
          occurences={mockOccurences}
          onOccurenceSelect={handleOccurrenceSelection}
          selectedEvent={selectedEvent}
        />
      </div>
      <div
        className={`absolute w-full h-full transition-all duration-300 ease-in-out
          ${showDetails ? "opacity-100 translate-x-0 z-20" : "opacity-0 translate-x-10 pointer-events-none"}
        `}
      >
        <OccurenceDetails occurrence={selectedOccurrence} />
        {showDetails && (
          <button
            className="mt-4 ml-4 px-4 py-2 bg-gray-200 rounded hover:bg-gray-300"
            onClick={handleBack}
          >
            Back
          </button>
        )}
      </div>
    </div>
  );
};

export { EventOccurrences };
