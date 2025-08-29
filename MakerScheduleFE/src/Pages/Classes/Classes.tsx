import { getEvents } from "@ms/api/event.api";
import { ClassCard } from "@ms/Pages/Classes/ClassCard/ClassCard";
import { useQuery } from "@tanstack/react-query";

const Classes = () => {
  const { data: events } = useQuery({
    queryKey: ["events"],
    queryFn: getEvents,
    staleTime: 30000,
  });

  return (
    <div className="flex flex-row gap-4 bg-[#F2F4EF] justify-center w-full min-h-screen">
      <div className="flex flex-col gap-4 p-2 lg:max-w-[880px] w-full mx-auto lg:p-6">
        {events?.map((event, index) => (
          <ClassCard key={index} event={event} />
        ))}
      </div>
    </div>
  );
};

export { Classes };
