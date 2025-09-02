interface OccurranceUsers {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
}

interface Occurrence {
  eventId: string;
  eventName?: string;
  id?: string;
  scheduleStart: string;
  attendees?: OccurranceUsers[];
  leaders?: OccurranceUsers[];
  status: OccurrenceStaus;
  meta?: {
    isNew?: boolean;
    componentOrigin?: "occurrenceList" | "occurrenceCalendar";
  };
}

type OccurrenceStaus = "pending" | "complete" | "canceled";

type CreateOccurrence = Omit<Occurrence, "id" | "status" | "leaders"> & {
  leaders: string[];
};

type UpdateOccurrence = Omit<Occurrence, "status" | "leaders"> & {
  leaders: string[];
};

export { type Occurrence, type CreateOccurrence, type UpdateOccurrence };
