import type { Occurrence } from "@ms/types/occurrence.types";

type EventType = Record<number, string>;

interface EventOffering {
  id?: string;
  eventName: string;
  description: string;
  attendees?: number[];
  leaders?: number[];
  duration?: number;
  price?: number;
  eventType?: number;
  fileUrl?: string;
  occurences?: Occurrence[];
  meta?: Record<string, string | boolean | number>;
}

type CreateEventOffering = Omit<
  EventOffering,
  "id" | "fileUrl | occurences"
> & {
  imageFile: File;
};

export { type EventOffering, type CreateEventOffering, type EventType };
