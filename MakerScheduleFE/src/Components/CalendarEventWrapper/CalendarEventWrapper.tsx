const CalendarEventWrapper = ({ children }: any) => {
  return (
    <div className="[&_.rbc-event-label]:hidden! [&_.rbc-event]:p-0!  [&_.rbc-event]:border-hidden!">
      {children}
    </div>
  );
};

export { CalendarEventWrapper };
