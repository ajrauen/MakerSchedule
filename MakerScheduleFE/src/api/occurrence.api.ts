import { sendRequest } from "@ms/api/api.utils";
import type { CreateOccurrence } from "@ms/types/occurrence.types";
import type { AxiosRequestConfig } from "axios";

const BASE_OCCURRENCE_ENDPOINT = "api/Occurrences";

const createOccurrence = async (occurrence: CreateOccurrence) => {
  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_OCCURRENCE_ENDPOINT}`,
    withCredentials: true,
    data: occurrence,
  };

  return await sendRequest<number>(req);
};

export { createOccurrence };
