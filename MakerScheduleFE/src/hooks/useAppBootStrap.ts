import { getEvents } from "@ms/api/event.api";
import { useQuery } from "@tanstack/react-query";

const useAppBootStrap = () => {
  useQuery({
    queryKey: ["events"],
    queryFn: getEvents,
  });
};

export { useAppBootStrap };
