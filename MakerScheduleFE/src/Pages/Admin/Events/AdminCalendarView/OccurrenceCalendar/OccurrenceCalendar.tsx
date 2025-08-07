import FullCalendar from "@fullcalendar/react";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import interactionPlugin from "@fullcalendar/interaction";
import type { Occurrence } from "@ms/types/occurrence.types";
import { getOccurrences } from "@ms/api/occurrence.api";
import { useState, useRef } from "react";
import { useQuery } from "@tanstack/react-query";
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
}

function calculateEndTime(
  occurrence: Occurrence | undefined
): string | undefined {
  if (!occurrence?.scheduleStart || !occurrence?.duration) return undefined;
  const start = new Date(occurrence.scheduleStart);
  start.setMilliseconds(start.getMilliseconds() + occurrence.duration);
  return start.toISOString();
}

const OccurrenceCalendar = ({ selectedEventType }: OccurrenceCalendarProps) => {
  const { events } = useAdminEventsData();
  const calendarRef = useRef<FullCalendar>(null);

  const handleDatesSet = (arg: DatesSetArg) => {
    setCalendarStartDate(arg.start);
    setCalendarEndDate(arg.end);
  };
  const [calendarStartDate, setCalendarStartDate] = useState<Date | null>(null);
  const [calendarEndDate, setCalendarEndDate] = useState<Date | null>(null);
  const dispatch = useAppDispatch();

  const { data: occurrences } = useQuery({
    queryKey: [
      "occurrences",
      calendarStartDate,
      calendarEndDate,
      selectedEventType,
    ],
    queryFn: () =>
      getOccurrences(calendarStartDate!, calendarEndDate!, selectedEventType),
    enabled: !!calendarStartDate && !!calendarEndDate,
  });

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
    const occurrence = occurrences?.data.find(
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
        events={
          occurrences?.data ? convertToCalendarEvents(occurrences.data) : []
        }
        eventClick={handleEventClick}
        editable={true}
        selectable={true}
        selectMirror={true}
        dayMaxEvents={true}
        height="auto"
        datesSet={handleDatesSet}
      />
    </div>
  );
};

export { OccurrenceCalendar };
