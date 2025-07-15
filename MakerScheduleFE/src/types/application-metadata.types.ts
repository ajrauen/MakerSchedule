import type { EventType } from "@ms/types/event.types";

type Duration = Record<number, string>;

interface ApplicaitonMetadata {
  durations: Duration;
  eventTypes: EventType;
}

export { type ApplicaitonMetadata };
