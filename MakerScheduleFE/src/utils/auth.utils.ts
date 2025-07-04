import { useEffect, useState } from "react";

const ACCESS_TOKEN_KEY = "accessToken";

const setToken = (token: string) => {
  localStorage.setItem(ACCESS_TOKEN_KEY, token);
  window.dispatchEvent(new Event("accessTokenChanged"));
};

const removeToken = () => {
  localStorage.removeItem(ACCESS_TOKEN_KEY);
  window.dispatchEvent(new Event("accessTokenChanged"));
};

const isUserLoggedIn = () => {
  return !!localStorage.getItem(ACCESS_TOKEN_KEY);
};

function useIsLoggedIn() {
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
}

export { setToken, removeToken, isUserLoggedIn, useIsLoggedIn };
