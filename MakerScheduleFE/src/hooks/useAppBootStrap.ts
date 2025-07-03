import { getEvents } from "@ms/api/event.api";
import { useQuery } from "@tanstack/react-query";

const useAppBootStrap = () => {
  const { data } = useQuery({
    queryKey: ["events"],
    queryFn: getEvents,
  });
};

export { useAppBootStrap };
