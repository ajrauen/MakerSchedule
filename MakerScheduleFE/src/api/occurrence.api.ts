import { sendRequest } from "@ms/api/api.utils";
import type {
  CreateOccurrence,
  Occurrence,
  UpdateOccurrence,
} from "@ms/types/occurrence.types";
import type { AxiosRequestConfig } from "axios";

const BASE_OCCURRENCE_ENDPOINT = "api/Occurrences";

const createOccurrence = async (occurrence: CreateOccurrence) => {
  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_OCCURRENCE_ENDPOINT}`,
    withCredentials: true,
    data: occurrence,
  };
  const newOccurrence = await sendRequest<Occurrence>(req);

  return newOccurrence.data;
};

const updateOccurrence = async (occurrence: UpdateOccurrence) => {
  const req: AxiosRequestConfig = {
    method: "PUT",
    url: `${BASE_OCCURRENCE_ENDPOINT}`,
    withCredentials: true,
    data: occurrence,
  };

  const updateOcc = await sendRequest<Occurrence>(req);

  return updateOcc.data;
};

const deleteOccurrence = async (occurrenceId: string) => {
  const req: AxiosRequestConfig = {
    method: "DELETE",
    url: `${BASE_OCCURRENCE_ENDPOINT}/${occurrenceId}`,
    withCredentials: true,
  };

  await sendRequest<number>(req);

  return occurrenceId;
};

const getOccurrences = async (startDate: Date, endDate: Date) => {
  let url = `${BASE_OCCURRENCE_ENDPOINT}?`;
  if (startDate && endDate) {
    url += `startDate=${startDate.toISOString()}&endDate=${endDate.toISOString()}`;
  }

  const req: AxiosRequestConfig = {
    method: "GET",
    url: url,
    withCredentials: true,
  };

  const occurrences = await sendRequest<Occurrence[]>(req);

  return occurrences.data;
};

export { createOccurrence, updateOccurrence, deleteOccurrence, getOccurrences };
