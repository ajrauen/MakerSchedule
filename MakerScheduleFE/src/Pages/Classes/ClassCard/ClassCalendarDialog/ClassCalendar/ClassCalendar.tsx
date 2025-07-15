import { useState, useMemo } from "react";
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
  // Only allow the event's day to be selected
  const eventDate = useMemo(() => {
    if (!event?.scheduleStart) return undefined;
    const d = new Date(event.scheduleStart);
    d.setHours(0, 0, 0, 0);
    return d;
  }, [event]);

  const [selectedDate, setSelectedDate] = useState<Date | null>(
    eventDate ?? null
  );

  const isEventDay = (date: Date) => eventDate && isSameDay(date, eventDate);

  const CustomDay = (props: PickersDayProps) => {
    const { day, selected, outsideCurrentMonth, ...other } = props;
    const eventDay = isEventDay(day);

    // Disable all days except the event day
    if (!eventDay) {
      return (
        <PickersDay
          {...other}
          day={day}
          outsideCurrentMonth={outsideCurrentMonth}
          disabled
        />
      );
    }

    // Style for the event day
    return (
      <PickersDay
        {...other}
        day={day}
        outsideCurrentMonth={outsideCurrentMonth}
        selected={selected}
        className={
          `border-2 font-bold rounded-full ` +
          (selected
            ? "border-blue-700 bg-blue-700 text-white hover:bg-blue-800"
            : "border-blue-700 text-blue-700 bg-transparent hover:bg-blue-100")
        }
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
          className="rounded-lg bg-gray-50"
        />
      </LocalizationProvider>
      {eventDate && (
        <div className="text-sm text-gray-500 mt-2">
          Event runs on {eventDate.toLocaleDateString()}
        </div>
      )}
    </div>
  );
};
