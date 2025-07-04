import { sendRequest } from "@ms/api/api.utils";
import type { RegisterCustomerRequest } from "@ms/types/customer.types";
import type { AxiosRequestConfig } from "axios";

const BASE_CUSTOMER_ENDPOINT = "api/customers";

const registerNewCustomerUser = async (user: RegisterCustomerRequest) => {
  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_CUSTOMER_ENDPOINT}`,
    withCredentials: true,
    data: user,
  };
  const response = await sendRequest<number>(req);
  console.log(response.data);

  return response;
};

export { registerNewCustomerUser };
