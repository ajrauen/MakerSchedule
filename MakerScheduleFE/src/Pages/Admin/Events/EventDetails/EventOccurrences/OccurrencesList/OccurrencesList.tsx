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
  onOccurenceCreate: (selectedDate: Date) => void;
  selectedEvent: EventOffering;
}

const OccurencesList = ({
  occurences = [],
  onOccurenceSelect,
  onOccurenceCreate,
  selectedEvent,
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
  }, [selectedDate, selectedEvent]);

  const handleAddOccurrenceClick = () => {
    let date = selectedDate;
    if (!date) {
      date = new Date();
    }
    onOccurenceCreate(date);
  };

  return (
    <OccurrenceCalendarContext.Provider
      value={{ today, occurences: occurences }}
    >
      <div className="flex flex-col items-center gap-4 overflow-y-auto h-full pb-8">
        <div className=" min-h-[255px]">
          <LocalizationProvider dateAdapter={AdapterDateFns}>
            <DateCalendar
              value={selectedDate}
              onChange={setSelectedDate}
              slots={{ day: OccurenceCalendarDate }}
            />
          </LocalizationProvider>
        </div>

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
                  selectedEvent={selectedEvent}
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
