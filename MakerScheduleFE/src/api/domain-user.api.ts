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

export { getDomainUserList, registerNewDomainUser };
