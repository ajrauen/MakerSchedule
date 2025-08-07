import { OccurrenceCalendarContext } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/EventDetails/EventOccurrences/OccurrencesList/context/occurrence-context";
import { PickersDay, type PickersDayProps } from "@mui/x-date-pickers";
import { startOfDay } from "date-fns";
import { useContext, useState } from "react";

const OccurenceCalendarDate = (props: PickersDayProps) => {
  const { occurrences, today } = useContext(OccurrenceCalendarContext);

  const occurenceDates = occurrences.map((o) =>
    startOfDay(new Date(o.scheduleStart)).getTime()
  );

  const { day, className, selected = false, ...other } = props;
  const dayStart = startOfDay(day).getTime();
  const isOccurrence = occurenceDates.includes(dayStart);
  const [hover, setHover] = useState(false);
  const todayStart = startOfDay(today).getTime();

  let style = {};
  let tw = className ?? "";

  if (isOccurrence && !selected) {
    let bg = "";
    if (dayStart < todayStart) {
      bg = hover ? "#b91c1c" : "#ef4444"; // red for past
    } else if (dayStart === todayStart) {
      bg = hover ? "#6d28d9" : "#a21caf"; // purple for today
    } else {
      bg = hover ? "#15803d" : "#22c55e"; // green for future
    }
    style = {
      backgroundColor: bg,
      color: "white",
    };
    tw += " font-bold rounded-full ";
  }

  return (
    <PickersDay
      {...other}
      day={day}
      selected={selected}
      className={tw}
      style={style}
      onMouseEnter={() => setHover(true)}
      onMouseLeave={() => setHover(false)}
    />
  );
};

export { OccurenceCalendarDate };
