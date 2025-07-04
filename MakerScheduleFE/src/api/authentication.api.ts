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
  try {
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
  } catch (error) {
    throw error;
  }
};

const refreshToken = async () => {
  try {
    const req: AxiosRequestConfig = {
      method: "POST",
      url: `${BASE_AUTH_ENDPOINT}/refresh`,
    };
    return await sendRequest(req);
  } catch (error) {
    throw error;
  }
};

const logout = async () => {
  try {
    const req: AxiosRequestConfig = {
      method: "POST",
      url: `${BASE_AUTH_ENDPOINT}/logout`,
    };
    return await sendRequest(req);
  } catch (error) {
    throw error;
  }
};

const registerUser = async (user: RegisterUserProps) => {
  try {
    const req: AxiosRequestConfig = {
      method: "POST",
      url: `${BASE_AUTH_ENDPOINT}/register`,
      data: user,
      withCredentials: true,
    };
    return await sendRequest(req);
  } catch (error) {
    throw error;
  }
};

export { login, refreshToken, logout, registerUser };
