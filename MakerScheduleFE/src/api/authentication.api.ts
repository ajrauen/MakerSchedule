import { sendRequest } from "@ms/api/api.utils";
import type { UserLogin } from "@ms/types/auth.types";
import type { AxiosRequestConfig } from "axios";

interface LoginApiProps {
  creds: UserLogin;
}

const BASE_AUTH_ENDPOINT = "api/Auth";

const login = async ({ creds }: LoginApiProps) => {
  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_AUTH_ENDPOINT}/login`,
    data: creds,
    withCredentials: true,
  };
  const response = await sendRequest(req);
  localStorage.setItem("accessToken", response.data.accessToken);
  console.log(response.data.accessToken);

  return response;
};

const refreshToken = () => {
  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_AUTH_ENDPOINT}/refresh`,
  };

  return sendRequest(req);
};

const logout = () => {
  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_AUTH_ENDPOINT}/logout`,
  };

  return sendRequest(req);
};

export { login, refreshToken, logout };
