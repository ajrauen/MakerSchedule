import type { Occurrence } from "@ms/types/occurrence.types";

interface EventOffering {
  id?: string;
  eventName: string;
  description: string;
  attendees?: number[];
  leaders?: number[];
  duration?: number;
  price?: number;
  eventType?: string;
  thumbnailUrl?: string;
  occurences?: Occurrence[];
  meta?: Record<string, string | boolean | number>;
}

type CreateEventOffering = Omit<
  EventOffering,
  "id" | "thumbnailUrl | occurences"
> & {
  thumbnailFile: File;
};

type UpdateEventOffering = Partial<CreateEventOffering> & {
  id: string;
};

interface EventType {
  id?: string;
  name: string;
  meta?: Record<string, string | boolean | number>;
}

type CreateEventType = Omit<EventType, "id"> & {
  name: string;
};

type PatchEventType = Omit<EventType, "id"> & {
  name: string;
};

export {
  type EventOffering,
  type EventType,
  type CreateEventOffering,
  type UpdateEventOffering,
  type CreateEventType,
  type PatchEventType,
};
