import type { Occurrence } from "@ms/types/occurrence.types";
import { createContext } from "react";

const OccurrenceCalendarContext = createContext<{
  today: Date;
  occurrences: Occurrence[];
}>({ today: new Date(), occurrences: [] });

export { OccurrenceCalendarContext };
