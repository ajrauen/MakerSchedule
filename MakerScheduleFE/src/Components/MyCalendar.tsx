import {
  Calendar,
  dateFnsLocalizer,
  Views,
  type Event,
  type View,
} from "react-big-calendar";
import { enUS } from "date-fns/locale";
import { format, parse, startOfWeek, getDay } from "date-fns";
import "react-big-calendar/lib/css/react-big-calendar.css";
import { useCallback, useMemo, useState } from "react";
import { CalendarEvent } from "@ms/Components/CalendarEvent/CalendarEvent";
import { CalendarEventWrapper } from "@ms/Components/CalendarEventWrapper/CalendarEventWrapper";
import { useQuery } from "@tanstack/react-query";
import { getEvents } from "@ms/api/event.api";

const locales = { "en-US": enUS };
const localizer = dateFnsLocalizer({
  format,
  parse,
  startOfWeek,
  getDay,
  locales,
});

export default function MyCalendar() {
  const [view, setView] = useState<View>("week");
  const [date, setDate] = useState<Date>(new Date());

  const { data: eventResponse } = useQuery({
    queryKey: ["events"],
    queryFn: getEvents,
    staleTime: 30000,
  });

  const onView = useCallback((view: View) => {
    setView(view);
  }, []);

  const onNavigate = useCallback((date: Date) => {
    setDate(date);
  }, []);

  const events = useMemo(() => {
    if (eventResponse?.data) {
      let calendarEvents: Event[] = [];

      eventResponse.data.forEach((eventOffering) => {
        if (eventOffering.occurences) {
          eventOffering.occurences.forEach((occurrence) => {
            const e: Event = {
              title: eventOffering.eventName,
              start: new Date(occurrence.scheduleStart),
              end: new Date(
                new Date(occurrence.scheduleStart).getTime() +
                  (occurrence.duration ?? 10000000)
              ),
              resource: {
                eventOffering: eventOffering,
                occurrence: occurrence,
              },
            };
            calendarEvents.push(e);
          });
        }
      });

      return calendarEvents;
    }
  }, [eventResponse]);

  return (
    <>
      <Calendar
        localizer={localizer}
        events={events}
        defaultView={Views.WEEK}
        views={[Views.MONTH, Views.WEEK, Views.DAY]}
        style={{ height: 500 }}
        onNavigate={onNavigate}
        onView={onView}
        view={view}
        date={date}
        components={{
          event: CalendarEvent,
          eventWrapper: CalendarEventWrapper,
        }}
      />
    </>
  );
}
