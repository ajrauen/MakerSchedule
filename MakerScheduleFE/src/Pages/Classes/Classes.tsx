import { getEvents } from "@ms/api/event.api";
import { ClassCard } from "@ms/Pages/Classes/ClassCard/ClassCard";
import { type EventOffering } from "@ms/types/event.types";
import { useQuery } from "@tanstack/react-query";
import { useMemo } from "react";

const Classes = () => {
  const { data: eventResponse } = useQuery({
    queryKey: ["events"],
    queryFn: getEvents,
    staleTime: 30000,
  });

  const eventData = useMemo(() => {
    if (!eventResponse?.data) return [];

    return eventResponse.data.map((event: EventOffering) => ({
      eventName: event.eventName || "",
      description: event.description || "",
      attendees: event.attendees || [],
      leaders: event.leaders || [],
      scheduleStart: event.scheduleStart,
      duration: event.duration,
      price: event.price || 0,
      eventType: event.eventType,
      fileUrl: event.fileUrl
        ? event.fileUrl
        : "https://www.akc.org/wp-content/uploads/2017/11/Pembroke-Welsh-Corgi-standing-outdoors-in-the-fall.jpg",
    })) as EventOffering[];
  }, [eventResponse]);

  return (
    <div className="flex flex-row gap-4 bg-[#F2F4EF] justify-center w-full min-h-screen">
      <div className="w-1/6">filter</div>
      <div className="flex flex-col gap-4  lg:max-w-[880px] w-full mx-auto lg:p-6">
        {eventData.map((event, index) => (
          <ClassCard key={index} event={event} />
        ))}
      </div>
    </div>
  );
};

export { Classes };
