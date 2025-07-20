import { useEffect, useState } from "react";

const useHasAccessToken = () => {
  const [isLoggedIn, setHasAccessToken] = useState(false);

  useEffect(() => {
    setHasAccessToken(getHasAccessToken());
  }, []);

  const getHasAccessToken = () => {
    return !!localStorage.getItem("accessToken");
  };

  useEffect(() => {
    const onStorage = (e: StorageEvent) => {
      if (e.key === "accessToken") {
        setHasAccessToken(getHasAccessToken());
      }
    };
    window.addEventListener("storage", onStorage);

    const origSetItem = localStorage.setItem;
    const origRemoveItem = localStorage.removeItem;
    localStorage.setItem = function (...args) {
      origSetItem.apply(this, args);
      if (args[0] === "accessToken") {
        setHasAccessToken(getHasAccessToken());
      }
    };
    localStorage.removeItem = function (...args) {
      origRemoveItem.apply(this, args);
      if (args[0] === "accessToken") {
        setHasAccessToken(getHasAccessToken());
      }
    };

    return () => {
      window.removeEventListener("storage", onStorage);
      localStorage.setItem = origSetItem;
      localStorage.removeItem = origRemoveItem;
    };
  }, []);

  return isLoggedIn;
};

export { useHasAccessToken };
