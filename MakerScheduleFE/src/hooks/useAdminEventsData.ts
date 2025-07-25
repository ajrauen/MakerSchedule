import { getDomainUsers } from "@ms/api/domain-user.api";
import { getEvents } from "@ms/api/event.api";
import { getApplicaitonMetadata } from "@ms/api/metadata.api";
import type { AdminEventsMetaData } from "@ms/types/application-metadata.types";
import { useQuery } from "@tanstack/react-query";

const useAdminEventsData = () => {
  const defaultAppMetaData: AdminEventsMetaData = {
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

  const { isError: domainLeaderError, isFetching: domainLeaderLoading } =
    useQuery({
      queryKey: ["domainUserLeaders"],
      queryFn: () => getDomainUsers("leader"),
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

  if (appMetadataError || eventError || domainLeaderError) {
    throw Error("Required app data missing");
  }

  return {
    isLoading: eventsLoading || metadataLoading || domainLeaderLoading,
    events: eventsResponse?.data ?? [],
    appMetaData: applicaitonMetadataResponse?.data ?? defaultAppMetaData,
  };
};

export { useAdminEventsData };
