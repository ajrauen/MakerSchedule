interface Occurrence {
  eventId: string;
  id?: string;
  scheduleStart: string;
  duration?: number;
  attendees?: number[];
  leaders?: number[];
  meta?: Record<string | number, string | number | boolean>; //store UI only data
}

type CreateOccurrence = Omit<Occurrence, "id"> & {};

export { type Occurrence, type CreateOccurrence };
