import { refreshToken } from "@ms/api/authentication.api";
import { useEffect } from "react";
import { removeToken } from "@ms/utils/auth.utils";
import { useQuery } from "@tanstack/react-query";
import { useIsLoggedIn } from "@ms/hooks/useIsLoggedIn";

const useRefreshTokenOnLoad = () => {
  const isLoggedIn = useIsLoggedIn();

  const { error: isError } = useQuery({
    queryKey: ["refreshTokenOnLoad"],
    queryFn: refreshToken,
    enabled: !isLoggedIn,
    retry: false,
    throwOnError: false,
  });

  useEffect(() => {
    if (isError) {
      removeToken();
    }
  }, [isError]);
};

export { useRefreshTokenOnLoad };
