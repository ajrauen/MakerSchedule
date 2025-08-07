import type { Occurrence } from "@ms/types/occurrence.types";

const isOccurrenceInPast = (occurrence: Occurrence) => {
  return new Date(occurrence.scheduleStart) < new Date();
};

export { isOccurrenceInPast };
