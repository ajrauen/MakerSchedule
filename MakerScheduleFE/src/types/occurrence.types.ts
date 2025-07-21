interface OccurranceUsers {
  id: string;
  firstName: string;
  lastName: string;
}

interface Occurrence {
  eventId: string;
  id?: string;
  scheduleStart: string;
  duration?: number;
  attendees?: OccurranceUsers[];
  leaders?: OccurranceUsers[];
  status: OccurrenceStaus;
  meta?: Record<string | number, string | number | boolean>;
}

type OccurrenceStaus = "pending" | "complete" | "canceled";

type CreateOccurrence = Omit<Occurrence, "id" | "status" | "leaders"> & {
  leaders: string[];
};

export { type Occurrence, type CreateOccurrence };
