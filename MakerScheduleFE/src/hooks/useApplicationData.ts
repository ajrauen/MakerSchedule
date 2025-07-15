import { getEvents } from "@ms/api/event.api";
import { getApplicaitonMetadata } from "@ms/api/metadata.api";
import type { ApplicaitonMetadata } from "@ms/types/application-metadata.types";
import { useQuery } from "@tanstack/react-query";

const useApplicationData = () => {
  const defaultAppMetaData: ApplicaitonMetadata = {
    durations: {},
    eventTypes: {},
  };

  const {
    data: eventsResponse,
    isError: eventError,
    isFetching: eventsLoading,
  } = useQuery({
    queryKey: ["events"],
    queryFn: getEvents,
    staleTime: Infinity,
  });

  const {
    data: applicaitonMetadataResponse,
    isError: appMetadataError,
    isFetching: metadataLoading,
  } = useQuery({
    queryKey: ["application-metadata"],
    queryFn: getApplicaitonMetadata,
    staleTime: Infinity,
  });

  if (appMetadataError || eventError) {
    throw Error("Required app data missing");
  }

  return {
    isLoading: eventsLoading || metadataLoading,
    events: eventsResponse?.data ?? [],
    appMetaData: applicaitonMetadataResponse?.data ?? defaultAppMetaData,
  };
};

export { useApplicationData };
