interface Occurrence {
  eventId: number;
  id: number;
  scheduleStart: number;
  duration?: number;
  attendees?: number[];
  leaders?: number[];
}

type CreateOccurence = Omit<Occurrence, "id"> & {};

export { type Occurrence, type CreateOccurence };
