import { getDomainUsers } from "@ms/api/domain-user.api";
import { getEvents } from "@ms/api/event.api";
import { getEventMetadata } from "@ms/api/metadata.api";
import type { AdminEventsMetaData } from "@ms/types/application-metadata.types";
import { useQuery } from "@tanstack/react-query";

const useAdminEventsData = () => {
  const defaultAppMetaData: AdminEventsMetaData = {
    durations: {},
    eventTypes: [],
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

  const { isError: domainLeaderError, isFetching: domainLeaderLoading } =
    useQuery({
      queryKey: ["domainUserLeaders"],
      queryFn: () => getDomainUsers("leader"),
      staleTime: Infinity,
    });

  const {
    data: eventMetadataResponse,
    isError: eventMetadataError,
    isFetching: metadataLoading,
  } = useQuery({
    queryKey: ["events-metadata"],
    queryFn: getEventMetadata,
    staleTime: Infinity,
  });

  if (eventMetadataError || eventError || domainLeaderError) {
    throw Error("Required app data missing");
  }

  return {
    isLoading: eventsLoading || metadataLoading || domainLeaderLoading,
    events: eventsResponse?.data ?? [],
    appMetaData: eventMetadataResponse?.data ?? defaultAppMetaData,
  };
};

export { useAdminEventsData };
