import { sendRequest } from "@ms/api/api.utils";
import type {
  DomainUser,
  RegisterDomainUserRequest,
  UpdateDomainUserRequest,
} from "@ms/types/domain-user.types";
import type { AxiosRequestConfig } from "axios";

const BASE_DOMAIN_USER_ENDPOINT = "api/domain-users";

const getActiveUser = async () => {
  const req: AxiosRequestConfig = {
    method: "GET",
    url: `${BASE_DOMAIN_USER_ENDPOINT}`,
    withCredentials: true,
  };
  const response = await sendRequest<DomainUser>(req);

  return response.data;
};

const getDomainUserList = async () => {
  const req: AxiosRequestConfig = {
    method: "GET",
    url: `${BASE_DOMAIN_USER_ENDPOINT}/all`,
    withCredentials: true,
  };
  const response = await sendRequest<DomainUser[]>(req);

  return response.data;
};

const registerNewDomainUser = async (user: RegisterDomainUserRequest) => {
  const req: AxiosRequestConfig = {
    method: "POST",
    url: BASE_DOMAIN_USER_ENDPOINT,
    withCredentials: true,
    data: user,
  };
  const response = await sendRequest<number>(req);

  return response.data;
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

  return response.data;
};

const getAvailableDomainUserLeaders = async (
  startTime: string,
  duration: number,
  occurrenceId: string,
  currentLeaderIds?: string[]
) => {
  const requestData = {
    startTime: startTime,
    duration: duration,
    occurrenceId: occurrenceId === "" ? null : occurrenceId,
    currentLeaderIds: currentLeaderIds?.map((id) => id) || [],
  };

  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_DOMAIN_USER_ENDPOINT}/available-leaders`,
    withCredentials: true,
    data: requestData,
  };
  const response = await sendRequest<DomainUser[]>(req);

  return response.data;
};

const sendPasswordResetRequest = async (email: string) => {
  const requestData = {
    email: email,
  };

  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_DOMAIN_USER_ENDPOINT}/request-password-reset`,
    withCredentials: true,
    data: requestData,
  };
  const response = await sendRequest(req);

  return response.data;
};

const validateResetPasswordToken = async (email: string, token: string) => {
  const requestData = {
    email: email,
    token: token,
  };

  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_DOMAIN_USER_ENDPOINT}/validate-reset-password-token`,
    withCredentials: true,
    data: requestData,
  };
  const response = await sendRequest(req);

  return response.data;
};

interface ResetPasswordRequest {
  email: string;
  token: string;
  newPassword: string;
}

const resetPassword = async ({
  email,
  token,
  newPassword,
}: ResetPasswordRequest) => {
  const requestData = {
    email,
    token,
    newPassword,
  };

  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_DOMAIN_USER_ENDPOINT}/reset-password`,
    withCredentials: true,
    data: requestData,
  };
  const response = await sendRequest(req);

  return response.data;
};

const updateUserProfile = async ({
  userId,
  data,
}: {
  userId: string;
  data: UpdateDomainUserRequest;
}) => {
  const req: AxiosRequestConfig = {
    method: "PUT",
    url: `${BASE_DOMAIN_USER_ENDPOINT}/${userId}`,
    withCredentials: true,
    data: data,
  };
  const response = await sendRequest(req);

  return response.data;
};

const updateUserPassword = async ({
  userId,
  currentPassword,
  newPassword,
}: {
  userId: string;
  currentPassword: string;
  newPassword: string;
}) => {
  const req: AxiosRequestConfig = {
    method: "PUT",
    url: `${BASE_DOMAIN_USER_ENDPOINT}/${userId}/change-password`,
    withCredentials: true,
    data: {
      currentPassword,
      newPassword,
    },
  };
  const response = await sendRequest(req);

  return response.data;
};

export {
  getDomainUserList,
  registerNewDomainUser,
  getAvailableDomainUserLeaders,
  getDomainUsers,
  sendPasswordResetRequest,
  validateResetPasswordToken,
  getActiveUser,
  resetPassword,
  updateUserPassword,
  updateUserProfile,
};
