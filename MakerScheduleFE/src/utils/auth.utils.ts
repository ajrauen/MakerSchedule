const ACCESS_TOKEN_KEY = "accessToken";

const setToken = (token: string) => {
  localStorage.setItem(ACCESS_TOKEN_KEY, token);
  window.dispatchEvent(new Event("accessTokenChanged"));
};

const removeToken = () => {
  localStorage.removeItem(ACCESS_TOKEN_KEY);
  window.dispatchEvent(new Event("accessTokenChanged"));
};

const getToken = () => {
  return localStorage.getItem(ACCESS_TOKEN_KEY);
};

const isTokenExpired = (token: string): boolean => {
  try {
    const parts = token.split(".");
    if (parts.length !== 3) return true;

    const payload = JSON.parse(atob(parts[1]));

    if (payload.exp && payload.exp * 1000 < Date.now()) {
      return true;
    }

    return false;
  } catch {
    return true;
  }
};

const isUserLoggedIn = () => {
  const token = localStorage.getItem(ACCESS_TOKEN_KEY);

  if (!token) return false;

  if (isTokenExpired(token)) {
    removeToken();
    return false;
  }

  return true;
};

export { setToken, removeToken, getToken, isUserLoggedIn };
