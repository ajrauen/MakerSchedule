import { isUserLoggedIn } from "@ms/utils/auth.utils";
import { useEffect, useState } from "react";

const useIsLoggedIn = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(isUserLoggedIn());

  useEffect(() => {
    const onChange = () => setIsLoggedIn(isUserLoggedIn());
    window.addEventListener("storage", onChange);
    window.addEventListener("accessTokenChanged", onChange);
    return () => {
      window.removeEventListener("storage", onChange);
      window.removeEventListener("accessTokenChanged", onChange);
    };
  }, []);

  return isLoggedIn;
};

export { useIsLoggedIn };
