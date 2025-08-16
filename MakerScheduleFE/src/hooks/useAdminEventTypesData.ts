import { getEventTypes } from "@ms/api/eventTypes.api";
import { useQuery } from "@tanstack/react-query";

const useAdminEventTypeData = () => {
  const {
    data: eventTypes,
    isError: eventTypesError,
    isFetching: eventTypesLoading,
  } = useQuery({
    queryKey: ["eventTypes"],
    queryFn: getEventTypes,
    staleTime: Infinity,
  });

  if (eventTypesError) {
    throw Error("Required app data missing");
  }

  return {
    isLoading: eventTypesLoading,
    eventTypes: eventTypes ?? [],
  };
};

export { useAdminEventTypeData };
