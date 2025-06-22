import { type AxiosRequestConfig } from "axios";

import { AUTH_URI } from "@ms/common/env-constants";
import { sendAxiosRequest } from "@ms/api/api";

const sendRequest = (req: AxiosRequestConfig) => {
  const url = `${AUTH_URI}/${req.url}`;
  req.url = url;

  return sendAxiosRequest(req);
};

export { sendRequest };
