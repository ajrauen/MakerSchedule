import type { Occurrence } from "@ms/types/occurrence.types";
import { createContext } from "react";

const OccurrenceCalendarContext = createContext<{
  selectedDate: Date;
  occurrences: Occurrence[];
}>({ selectedDate: new Date(), occurrences: [] });

export { OccurrenceCalendarContext };
