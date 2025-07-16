import { sendRequest } from "@ms/api/api.utils";
import type { EventOffering } from "@ms/types/event.types";
import type { Occurrence } from "@ms/types/Occurrence.types";
import type { AxiosRequestConfig } from "axios";

const BASE_Occurence_ENDPOINT = "api/ccurrences";

const createOccurrence = async (event: FormData) => {
  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_Occurence_ENDPOINT}`,
    withCredentials: true,
    data: event,
  };

  return await sendRequest<number>(req);
};

const getOccurences = async () => {
  const req: AxiosRequestConfig = {
    method: "GET",
    url: `${BASE_Occurence_ENDPOINT}`,
    withCredentials: true,
  };

  return await sendRequest<Occurrence[]>(req);
};

const getOccurence = async (occurrenceId: number) => {
  const req: AxiosRequestConfig = {
    method: "GET",
    url: `${BASE_Occurence_ENDPOINT}/${occurrenceId}`,
    withCredentials: true,
  };

  return await sendRequest<Occurrence>(req);
};

export { createOccurrence, getOccurences, getOccurence };
