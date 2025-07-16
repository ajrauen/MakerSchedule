interface Occurrence {
  eventId: number;
  id: number;
  scheduleStart: number;
  duration?: number;
  attendees?: number[];
  leaders?: number[];
  meta?: Record<string | number, string | number | boolean>; //store UI only data
}

type CreateOccurrence = Omit<Occurrence, "id"> & {};

export { type Occurrence, type CreateOccurrence };
