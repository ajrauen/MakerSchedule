import { getDomainUserList } from "@ms/api/domain-user.api";

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

  if (userError) {
    throw Error("Required app data missing");
  }

  return {
    isLoading: usersLoading,
    users: usersResponse?.data ?? [],
  };
};

export { useAdminUsersData };
