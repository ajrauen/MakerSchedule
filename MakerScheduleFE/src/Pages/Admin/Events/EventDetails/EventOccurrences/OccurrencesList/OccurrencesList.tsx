import { DateCalendar } from "@mui/x-date-pickers";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import { startOfDay } from "date-fns";

import { useMemo, useState } from "react";
import { OccurrenceCalendarContext } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/OccurrencesList/context/occurrence-context";
import { OccurenceCalendarDate } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/OccurrencesList/OccurenceCalendarDate/OccurenceCalendarDate";
import { OccurenceRow } from "@ms/Pages/Admin/Events/EventDetails/EventOccurrences/OccurrencesList/OccurrenceRow/OccurrenceRow";
import type { Occurrence } from "@ms/types/occurrence.types";
import { IconButton } from "@mui/material";
import { AddCircle } from "@mui/icons-material";
import type { EventOffering } from "@ms/types/event.types";

interface OccurencesListProps {
  occurences?: Occurrence[];
  onOccurenceSelect: (occurrence: Occurrence) => void;
  onOccurenceCreate: () => void;
}

const OccurencesList = ({
  occurences = [],
  onOccurenceSelect,
  onOccurenceCreate,
}: OccurencesListProps) => {
  const today = startOfDay(new Date());

  const [selectedDate, setSelectedDate] = useState<Date | null>(today);

  const occurrencesForDay = useMemo(() => {
    if (!selectedDate) return [];
    return occurences
      .filter(
        (o) =>
          startOfDay(selectedDate).getTime() ===
          startOfDay(new Date(o.scheduleStart)).getTime()
      )
      .sort(
        (a, b) =>
          new Date(a.scheduleStart).getTime() -
          new Date(b.scheduleStart).getTime()
      );
  }, [selectedDate]);

  const handleAddOccurrenceClick = () => {
    onOccurenceCreate();
  };

  console.log(occurrencesForDay);

  return (
    <OccurrenceCalendarContext.Provider
      value={{ today, occurences: occurences }}
    >
      <div className="flex flex-col items-center gap-4">
        <LocalizationProvider dateAdapter={AdapterDateFns}>
          <DateCalendar
            value={selectedDate}
            onChange={setSelectedDate}
            slots={{ day: OccurenceCalendarDate }}
          />
        </LocalizationProvider>
        <div className="flex w-full">
          <div className="ml-auto relative right-7">
            <IconButton onClick={handleAddOccurrenceClick}>
              <AddCircle />
            </IconButton>
          </div>
        </div>
        <div className="w-full max-w-md">
          {occurrencesForDay.length > 0 ? (
            <ul className="divide-y divide-gray-200 bg-white rounded shadow">
              {occurrencesForDay.map((occ) => (
                <OccurenceRow
                  occurrence={occ}
                  onOccurenceSelect={onOccurenceSelect}
                  key={occ.id}
                />
              ))}
            </ul>
          ) : (
            <div className="text-gray-400 text-center py-8">
              No occurrences for this day.
            </div>
          )}
        </div>
      </div>
    </OccurrenceCalendarContext.Provider>
  );
};

export { OccurencesList };
