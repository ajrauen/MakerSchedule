import { type AxiosRequestConfig } from "axios";

import { SERVER_URI } from "@ms/common/env-constants";
import { sendAxiosRequest } from "@ms/api/api";

const sendRequest = <T>(req: AxiosRequestConfig) => {
  const url = `${SERVER_URI}/${req.url}`;
  req.url = url;

  return sendAxiosRequest<T>(req);
};

export { sendRequest };
