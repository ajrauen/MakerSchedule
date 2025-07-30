import { createContext } from "react";

const OccurrenceCalendarContext = createContext<{
  today: Date;
  occurrences: any[];
}>({ today: new Date(), occurrences: [] });

export { OccurrenceCalendarContext };
