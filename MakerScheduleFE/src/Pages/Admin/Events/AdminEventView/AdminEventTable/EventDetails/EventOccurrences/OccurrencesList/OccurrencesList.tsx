import { DateCalendar } from "@mui/x-date-pickers";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import { startOfDay } from "date-fns";

import { useMemo, useState } from "react";
import { OccurrenceCalendarContext } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/EventDetails/EventOccurrences/OccurrencesList/context/occurrence-context";
import { OccurenceCalendarDate } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/EventDetails/EventOccurrences/OccurrencesList/OccurenceCalendarDate/OccurenceCalendarDate";
import { OccurenceRow } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/EventDetails/EventOccurrences/OccurrencesList/OccurrenceRow/OccurrenceRow";
import { IconButton } from "@mui/material";
import { AddCircle } from "@mui/icons-material";
import {
  selectAdminState,
  setSelectedEventOccurrence,
} from "@ms/redux/slices/adminSlice";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";

interface OccurrencesListProps {
  onOccurrenceCreate: (selectedDate: Date) => void;
  selectedDate: Date;
  onDateChange: (date: Date) => void;
}

const OccurrencesList = ({
  onOccurrenceCreate,
  selectedDate,
  onDateChange,
}: OccurrencesListProps) => {
  const { selectedEvent } = useAppSelector(selectAdminState);
  const dispatch = useAppDispatch();

  const occurrencesForDay = useMemo(() => {
    if (!selectedDate || !selectedEvent?.occurrences) return [];
    dispatch(setSelectedEventOccurrence(undefined));

    return selectedEvent.occurrences
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
  }, [dispatch, selectedDate, selectedEvent?.occurrences]);

  const handleAddOccurrenceClick = () => {
    let date = selectedDate;
    if (!date) {
      date = new Date();
      if (date.getHours() >= 12) {
        date.setDate(date.getDate() + 1);
      }
      date.setHours(12, 0, 0, 0);
    }
    onOccurrenceCreate(date);
  };

  return (
    <OccurrenceCalendarContext.Provider
      value={{ selectedDate, occurrences: selectedEvent?.occurrences ?? [] }}
    >
      <div className="flex flex-col items-center gap-4 overflow-y-auto h-full pb-8">
        <div className=" min-h-[255px]">
          <LocalizationProvider dateAdapter={AdapterDateFns}>
            <DateCalendar
              value={selectedDate}
              onChange={(value) => {
                onDateChange(value as Date);
              }}
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
                <OccurenceRow occurrence={occ} key={occ.id} />
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

export { OccurrencesList };
