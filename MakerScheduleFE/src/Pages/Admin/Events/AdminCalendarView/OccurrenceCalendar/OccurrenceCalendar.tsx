import FullCalendar from "@fullcalendar/react";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import interactionPlugin, {
  type DateClickArg,
} from "@fullcalendar/interaction";
import type { Occurrence } from "@ms/types/occurrence.types";

import { useAdminEventsData } from "@ms/hooks/useAdminEventsData";
import { useAppDispatch } from "@ms/redux/hooks";
import {
  setAdminDrawerOpen,
  setSelectedEvent,
  setSelectedEventOccurrence,
} from "@ms/redux/slices/adminSlice";
import type { DatesSetArg, EventClickArg } from "@fullcalendar/core/index.js";
import type { EventOffering } from "@ms/types/event.types";

interface OccurrenceCalendarProps {
  onDateSet: (dates: DatesSetArg) => void;
  onDateClick: (date: DateClickArg) => void;
  occurrences: Occurrence[];
}

function calculateEndTime(
  occurrence: Occurrence | undefined,
  event: EventOffering | undefined
): string | undefined {
  if (!occurrence?.scheduleStart || !event?.duration) return undefined;
  const start = new Date(occurrence.scheduleStart);
  start.setMinutes(start.getMinutes() + event.duration);
  return start.toISOString();
}

const OccurrenceCalendar = ({
  onDateSet,
  onDateClick,
  occurrences,
}: OccurrenceCalendarProps) => {
  const { events } = useAdminEventsData();

  const dispatch = useAppDispatch();

  const convertToCalendarEvents = (occurrences: Occurrence[]) => {
    return occurrences.map((item) => ({
      id: `${item.eventId}_${item.id}`,
      title: item.eventName,
      start: item.scheduleStart,
      end: calculateEndTime(
        item,
        events.find((evt) => evt.id === item.eventId)
      ),
      backgroundColor: "red",
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
    dispatch(setSelectedEvent(event));

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
        dateClick={onDateClick}
      />
    </div>
  );
};

export { OccurrenceCalendar };
