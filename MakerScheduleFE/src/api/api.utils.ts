import { type AxiosRequestConfig } from "axios";

import { sendAxiosRequest } from "@ms/api/api";

const sendRequest = <T>(req: AxiosRequestConfig) => {
  const url = `${req.url}`;
  req.url = url;

  return sendAxiosRequest<T>(req);
};

export { sendRequest };
