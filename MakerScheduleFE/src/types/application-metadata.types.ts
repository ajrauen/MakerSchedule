import type { EventType } from "@ms/types/event.types";

type Duration = Record<number, string>;

interface AdminEventsMetaData {
  durations: Duration;
  eventTypes: EventType[];
}

interface AdminEventTypesMetaData {
  eventTypes: EventType[];
}

export { type AdminEventsMetaData, type AdminEventTypesMetaData };
