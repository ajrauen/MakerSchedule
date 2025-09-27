import { sendRequest } from "@ms/api/api.utils";
import { setToken } from "@ms/utils/auth.utils";
import type { UserLogin } from "@ms/types/auth.types";
import type { AxiosRequestConfig } from "axios";

interface LoginApiProps {
  creds: UserLogin;
}

interface RegisterUserProps {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  address: string;
  preferredContactMethod: string;
  notes?: string;
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

  const data = response.data as { accessToken: string };
  setToken(data.accessToken);
  return response;
};

const refreshToken = async () => {
  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_AUTH_ENDPOINT}/refresh`,
  };
  return await sendRequest(req);
};

const logout = async () => {
  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_AUTH_ENDPOINT}/logout`,
  };
  return await sendRequest(req);
};

const registerUser = async (user: RegisterUserProps) => {
  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_AUTH_ENDPOINT}/register`,
    data: user,
    withCredentials: true,
  };
  return await sendRequest(req);
};

export { login, refreshToken, logout, registerUser };
