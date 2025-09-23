import { getDomainUserList } from "@ms/api/domain-user.api";
import { getUserMetadata } from "@ms/api/metadata.api";

import { useQuery } from "@tanstack/react-query";

const useAdminUsersData = () => {
  const {
    data: usersResponse,
    isError: userError,
    isFetching: usersLoading,
  } = useQuery({
    queryKey: ["domainUsers"],
    queryFn: getDomainUserList,
    staleTime: Infinity,
  });

  const {
    data: userMetadataResponse,
    isError: userMetadataError,
    isFetching: metadataLoading,
  } = useQuery({
    queryKey: ["users-metadata"],
    queryFn: getUserMetadata,
    staleTime: Infinity,
  });

  if (userError || userMetadataError) {
    throw Error("Required app data missing");
  }

  return {
    isLoading: usersLoading || metadataLoading,
    users: usersResponse ?? [],
    userMetaData: userMetadataResponse ?? {
      roles: [],
    },
  };
};

export { useAdminUsersData };
