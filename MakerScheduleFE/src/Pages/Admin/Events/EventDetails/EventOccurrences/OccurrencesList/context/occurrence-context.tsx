import { createContext } from "react";

const OccurrenceCalendarContext = createContext<{
  today: Date;
  occurences: any[];
}>({ today: new Date(), occurences: [] });

export { OccurrenceCalendarContext };
