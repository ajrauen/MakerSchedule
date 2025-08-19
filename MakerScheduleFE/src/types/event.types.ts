import type { Occurrence } from "@ms/types/occurrence.types";

interface EventOffering {
  id?: string;
  eventName: string;
  description: string;
  attendees?: number[];
  leaders?: number[];
  duration?: number;
  price?: number;
  thumbnailUrl?: string;
  occurrences?: Occurrence[];
  meta?: Record<string, string | boolean | number>;
}

type CreateEventOffering = Omit<
  EventOffering,
  "id" | "thumbnailUrl" | "occurrences"
> & {
  thumbnailFile: File;
};

type UpdateEventOffering = Partial<CreateEventOffering> & {
  id: string;
};

export {
  type EventOffering,
  type CreateEventOffering,
  type UpdateEventOffering,
};
