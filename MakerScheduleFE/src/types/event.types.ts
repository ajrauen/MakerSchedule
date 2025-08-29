import type { Occurrence } from "@ms/types/occurrence.types";

interface EventOffering {
  id?: string;
  eventName: string;
  description: string;
  attendees?: number[];
  leaders?: number[];
  duration?: number;
  price: number;
  classSize: number;
  thumbnailUrl?: string;
  occurrences?: Occurrence[];
  eventTagIds?: string[];
  meta?: Record<string, string | boolean | number>;
}

type CreateEventOffering = Omit<
  EventOffering,
  "id" | "thumbnailUrl" | "occurrences" | "eventTagIds"
> & {
  thumbnailFile: File;
  eventTagIds: string[];
};

type UpdateEventOffering = Partial<CreateEventOffering> & {
  id: string;
};

export {
  type EventOffering,
  type CreateEventOffering,
  type UpdateEventOffering,
};
