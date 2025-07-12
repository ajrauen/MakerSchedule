import { getEvents } from "@ms/api/event.api";
import { EventsHeader } from "@ms/Pages/Admin/Events/Header/Header";
import { AdminEventsTable } from "@ms/Pages/Admin/Events/Table/Table";
import { useQuery } from "@tanstack/react-query";

const AdminEvents = () => {
  const { data: eventResponse } = useQuery({
    queryKey: ["events"],
    queryFn: getEvents,
    staleTime: 30000,
  });

  return (
    <div>
      <EventsHeader />
      <AdminEventsTable events={eventResponse?.data} />
    </div>
  );
};

export { AdminEvents };
