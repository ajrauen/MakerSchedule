import { type Event } from "react-big-calendar";

interface CalendarEventProps {
  event: Event;
  title: string;
}

const CalendarEvent = ({ event, title }: CalendarEventProps) => {
  return (
    <div
      style={{
        backgroundColor: "red",
        color: "white",
        padding: "2px 4px",
        borderRadius: "3px",
        fontSize: "12px",
        height: "100%",
        display: "flex",
        alignItems: "center",
        border: "none",
        boxShadow: "none",
      }}
    >
      {event.title}
    </div>
  );
};

export { CalendarEvent };
