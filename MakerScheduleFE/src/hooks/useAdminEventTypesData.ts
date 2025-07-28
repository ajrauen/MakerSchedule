import { getEventTypes } from "@ms/api/eventTypes.api";
import { useQuery } from "@tanstack/react-query";

const useAdminEventTypeData = () => {
  const {
    data: eventMetadataResponse,
    isError: eventMetadataError,
    isFetching: metadataLoading,
  } = useQuery({
    queryKey: ["eventTypes"],
    queryFn: getEventTypes,
    staleTime: Infinity,
  });

  if (eventMetadataError) {
    throw Error("Required app data missing");
  }

  return {
    isLoading: metadataLoading,
    eventTypes: eventMetadataResponse?.data ?? [],
  };
};

export { useAdminEventTypeData };
