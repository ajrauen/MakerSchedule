import { sendRequest } from "@ms/api/api.utils";
import type {
  CreateOccurrence,
  Occurrence,
  UpdateOccurrence,
} from "@ms/types/occurrence.types";
import type { AxiosRequestConfig } from "axios";
import { start } from "repl";

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

const updateOccurrence = async (occurrence: UpdateOccurrence) => {
  const req: AxiosRequestConfig = {
    method: "PUT",
    url: `${BASE_OCCURRENCE_ENDPOINT}`,
    withCredentials: true,
    data: occurrence,
  };

  return await sendRequest<number>(req);
};

const deleteOccurrence = async (occurrenceId: string) => {
  const req: AxiosRequestConfig = {
    method: "DELETE",
    url: `${BASE_OCCURRENCE_ENDPOINT}/${occurrenceId}`,
    withCredentials: true,
  };

  return await sendRequest<number>(req);
};

const getOccurrences = async (
  startDate: Date,
  endDate: Date,
  eventType: string | undefined
) => {
  let url = `${BASE_OCCURRENCE_ENDPOINT}?`;
  if (startDate && endDate) {
    url += `startDate=${startDate.toISOString()}&endDate=${endDate.toISOString()}`;
  }

  if (eventType) {
    url += `&eventType=${eventType}`;
  }

  const req: AxiosRequestConfig = {
    method: "GET",
    url: url,
    withCredentials: true,
  };

  return await sendRequest<Occurrence[]>(req);
};

export { createOccurrence, updateOccurrence, deleteOccurrence, getOccurrences };
