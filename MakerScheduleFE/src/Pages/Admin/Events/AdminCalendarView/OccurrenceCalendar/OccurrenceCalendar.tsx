import FullCalendar from "@fullcalendar/react";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import interactionPlugin from "@fullcalendar/interaction";
import type { Occurrence } from "@ms/types/occurrence.types";

import { useAdminEventsData } from "@ms/hooks/useAdminEventsData";
import { useAppDispatch } from "@ms/redux/hooks";
import {
  setAdminDrawerOpen,
  setSelectedEvent,
  setSelectedEventOccurrence,
} from "@ms/redux/slices/adminSlice";
import type { DatesSetArg, EventClickArg } from "@fullcalendar/core/index.js";

interface OccurrenceCalendarProps {
  selectedEventType?: string;
  onDateSet: (dates: DatesSetArg) => void;

  occurrences: Occurrence[];
}

function calculateEndTime(
  occurrence: Occurrence | undefined
): string | undefined {
  if (!occurrence?.scheduleStart || !occurrence?.duration) return undefined;
  const start = new Date(occurrence.scheduleStart);
  start.setMilliseconds(start.getMilliseconds() + occurrence.duration);
  return start.toISOString();
}

const OccurrenceCalendar = ({
  onDateSet,

  occurrences,
}: OccurrenceCalendarProps) => {
  const { events } = useAdminEventsData();

  const dispatch = useAppDispatch();

  function getEventTypeColor(): string {
    return "red";
  }

  const convertToCalendarEvents = (occurrences: Occurrence[]) => {
    return occurrences.map((item) => ({
      id: `${item.eventId}_${item.id}`,
      title: item.eventName,
      start: item.scheduleStart,
      end: calculateEndTime(item),
      backgroundColor: getEventTypeColor(),
      extendedProps: {
        eventId: item.eventId,
        occurrenceId: item.id,
        capacity: 20,
        enrolledCount: item.attendees?.length || 0,
      },
    }));
  };

  const handleEventClick = (info: EventClickArg) => {
    const occurrence = occurrences.find(
      (occ) => occ.id === info.event.extendedProps.occurrenceId
    );
    if (!occurrence) return;

    const updateOccurrence = structuredClone(occurrence);
    updateOccurrence.meta = {
      componentOrigin: "occurrenceCalendar",
    };

    const event = events.find((evt) => evt.id === updateOccurrence.eventId);
    setSelectedEvent(event);

    dispatch(setSelectedEventOccurrence(updateOccurrence));
    dispatch(setAdminDrawerOpen(true));
  };

  return (
    <div style={{ width: "100%", position: "relative" }}>
      <FullCalendar
        plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin]}
        initialView="dayGridMonth"
        headerToolbar={{
          left: "prev,next today",
          center: "title",
          right: "dayGridMonth,timeGridWeek,timeGridDay",
        }}
        events={occurrences ? convertToCalendarEvents(occurrences) : []}
        eventClick={handleEventClick}
        editable={true}
        selectable={true}
        selectMirror={true}
        dayMaxEvents={true}
        height="auto"
        datesSet={onDateSet}
      />
    </div>
  );
};

export { OccurrenceCalendar };
