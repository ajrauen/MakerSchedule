import { sendRequest } from "@ms/api/api.utils";
import type {
  DomainUser,
  RegisterDomainUserRequest,
} from "@ms/types/domain-user.types";
import type { AxiosRequestConfig } from "axios";

const BASE_DOMAIN_USER_ENDPOINT = "api/domain-users";

const getDomainUserList = async () => {
  const req: AxiosRequestConfig = {
    method: "GET",
    url: BASE_DOMAIN_USER_ENDPOINT,
    withCredentials: true,
  };
  const response = await sendRequest<DomainUser[]>(req);

  return response;
};

const registerNewDomainUser = async (user: RegisterDomainUserRequest) => {
  const req: AxiosRequestConfig = {
    method: "POST",
    url: BASE_DOMAIN_USER_ENDPOINT,
    withCredentials: true,
    data: user,
  };
  const response = await sendRequest<number>(req);

  return response;
};

const getDomainUsers = async (role?: string) => {
  let url = BASE_DOMAIN_USER_ENDPOINT;
  if (role) {
    url += `?role=${role}`;
  }

  const req: AxiosRequestConfig = {
    method: "GET",
    url: url,
    withCredentials: true,
  };
  const response = await sendRequest<DomainUser[]>(req);

  return response;
};

const getAvailableDomainUserLeaders = async (
  startTime: string,
  duration: number,
  currentLeaderIds?: string[]
) => {
  const requestData = {
    startTime: startTime,
    duration: duration,
    currentLeaderIds: currentLeaderIds?.map((id) => id) || [],
  };

  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_DOMAIN_USER_ENDPOINT}/available-leaders`,
    withCredentials: true,
    data: requestData,
  };
  const response = await sendRequest<DomainUser[]>(req);

  return response;
};

export {
  getDomainUserList,
  registerNewDomainUser,
  getAvailableDomainUserLeaders,
  getDomainUsers,
};
