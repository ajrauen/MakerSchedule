import type { Occurrence } from "@ms/types/Occurrence.types";

type EventType = Record<number, string>;

interface EventOffering {
  id: number;
  eventName: string;
  description: string;
  attendees?: number[];
  leaders?: number[];
  scheduleStart?: number;
  duration?: number;
  price?: number;
  eventType: number;
  fileUrl?: string;
  occurences?: Occurrence[];
}

type CreateEventOffering = Omit<
  EventOffering,
  "id" | "fileUrl | occurences"
> & {
  imageFile: File;
};

export { type EventOffering, type CreateEventOffering, type EventType };
