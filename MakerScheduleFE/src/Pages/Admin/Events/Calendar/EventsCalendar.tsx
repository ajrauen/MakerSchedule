import FullCalendar from "@fullcalendar/react";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import interactionPlugin from "@fullcalendar/interaction";
import type { Occurrence } from "@ms/types/occurrence.types";
import { getOccurrences } from "@ms/api/occurrence.api";
import { useEffect, useRef, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { useAdminEventsData } from "@ms/hooks/useAdminEventsData";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import {
  selectAdminState,
  setAdminDrawerOpen,
  setSelectedEvent,
  setSelectedEventOccurrence,
} from "@ms/redux/slices/adminSlice";

interface EventCalendarProps {
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

const EventCalendar = ({ selectedEventType }: EventCalendarProps) => {
  const { events } = useAdminEventsData();

  const calendarRef = useRef<any>(null);
  const containerRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!containerRef.current || !calendarRef.current) return;
    const calendarApi = calendarRef.current.getApi();
    const observer = new window.ResizeObserver(() => {
      calendarApi.updateSize();
    });
    observer.observe(containerRef.current);
    return () => {
      observer.disconnect();
    };
  }, []);

  const handleDatesSet = (arg: any) => {
    setCalendarStartDate(arg.start);
    setCalendarEndDate(arg.end);
  };
  const [calendarStartDate, setCalendarStartDate] = useState<Date | null>(null);
  const [calendarEndDate, setCalendarEndDate] = useState<Date | null>(null);
  const dispath = useAppDispatch();

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

  const handleEventClick = (info: any) => {
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

    dispath(setSelectedEventOccurrence(updateOccurrence));
    dispath(setAdminDrawerOpen(true));
  };

  return (
    <div ref={containerRef} style={{ height: "100%" }}>
      <FullCalendar
        plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin]}
        ref={calendarRef}
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

export { EventCalendar };
