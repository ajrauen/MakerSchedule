import type { EventType } from "@ms/types/event.types";

type Duration = Record<number, string>;

interface AdminEventsMetaData {
  durations: Duration;
  eventTypes: EventType;
}

export { type AdminEventsMetaData };
