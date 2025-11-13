import { useState } from "react";
import {
  LocalizationProvider,
  DateCalendar,
  PickersDay,
  type PickersDayProps,
} from "@mui/x-date-pickers";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import { isSameDay } from "date-fns";
import type { EventOffering } from "@ms/types/event.types";

interface ClassCalendarProps {
  onDateSelect?: (date: Date) => void;
  event?: EventOffering;
}

export const ClassCalendar = ({ onDateSelect, event }: ClassCalendarProps) => {
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);

  const isEventDay = (date: Date) => {
    if (!event?.occurrences || event.occurrences.length === 0) return false;

    const now = new Date();
    const threeHoursFromNow = new Date(now.getTime() + 3 * 60 * 60 * 1000);

    return event.occurrences.some((occurrence) => {
      const occurrenceDate = new Date(occurrence.scheduleStart);
      return (
        isSameDay(date, occurrenceDate) && occurrenceDate > threeHoursFromNow
      );
    });
  };

  const CustomDay = (props: PickersDayProps) => {
    const { day, selected, outsideCurrentMonth, ...other } = props;

    let cssClass = `border-2 font-bold rounded-full `;
    if (selected) {
      cssClass += "border-blue-700 bg-blue-700 text-white hover:!bg-blue-400";
    } else {
      cssClass +=
        "border-blue-700 text-blue-700 bg-transparent hover:!bg-blue-400";

      if (isEventDay(day)) {
        cssClass += " !bg-blue-200";
      }
    }

    return (
      <PickersDay
        {...other}
        day={day}
        outsideCurrentMonth={outsideCurrentMonth}
        selected={selected}
        className={cssClass}
      />
    );
  };

  const handleDateChange = (date: Date | null) => {
    if (date && isEventDay(date)) {
      setSelectedDate(date);
      if (onDateSelect) {
        onDateSelect(date);
      }
    }
  };

  return (
    <div className="p-4">
      <LocalizationProvider dateAdapter={AdapterDateFns}>
        <DateCalendar
          value={selectedDate}
          onChange={handleDateChange}
          slots={{ day: CustomDay }}
          shouldDisableDate={(date) => !isEventDay(date)}
          className="rounded-lg "
          sx={{ transform: "scale(1.3)" }}
        />
      </LocalizationProvider>
    </div>
  );
};
