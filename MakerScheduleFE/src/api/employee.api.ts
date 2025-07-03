import { sendRequest } from "@ms/api/api.utils";
import type { Employee } from "@ms/types/employee.types";
import type { AxiosRequestConfig } from "axios";

const BASE_EMPLOYEE_ENDPOINT = "api/customers";

const getEmployeeList = async () => {
  const req: AxiosRequestConfig = {
    method: "GET",
    url: `${BASE_EMPLOYEE_ENDPOINT}`,
    withCredentials: true,
  };
  const response = await sendRequest<Employee[]>(req);
  console.log(response.data);

  return response;
};

export { getEmployeeList };
