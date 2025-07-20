interface OccurrenceTimeProps {
  start: Date | string;
  end: Date | string;
}

const OccurrenceTime = ({ start, end }: OccurrenceTimeProps) => {
  const formatTime = (date: Date) => {
    const hours = date.getHours() % 12 || 12;
    const minutes = date.getMinutes();
    const ampm = date.getHours() < 12 ? "AM" : "PM";
    return minutes !== 0
      ? `${hours}:${minutes.toString().padStart(2, "0")} ${ampm}`
      : `${hours} ${ampm}`;
  };

  const startDate = typeof start === "string" ? new Date(start) : start;
  const endDate = typeof end === "string" ? new Date(end) : end;
  return (
    <span className="text-gray-500 text-sm relative ">
      {formatTime(startDate)}
      <span className="px-2">-</span>
      {formatTime(endDate)}
    </span>
  );
};

export { OccurrenceTime };
