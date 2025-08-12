import { sendRequest } from "@ms/api/api.utils";
import type { EventOffering } from "@ms/types/event.types";
import type { AxiosRequestConfig } from "axios";

const BASE_EVENT_ENDPOINT = "api/Events";

const createEvent = async (event: FormData): Promise<EventOffering> => {
  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_EVENT_ENDPOINT}`,
    withCredentials: true,
    data: event,
  };

  const response = await sendRequest<EventOffering>(req);
  return response.data;
};

const patchEvent = async (
  id: string,
  event: FormData
): Promise<EventOffering> => {
  const req: AxiosRequestConfig = {
    method: "PATCH",
    url: `${BASE_EVENT_ENDPOINT}/${id}`,
    withCredentials: true,
    data: event,
  };

  const response = await sendRequest<EventOffering>(req);
  return response.data;
};

const getEvents = async (): Promise<EventOffering[]> => {
  const req: AxiosRequestConfig = {
    method: "GET",
    url: `${BASE_EVENT_ENDPOINT}`,
    withCredentials: true,
  };

  const response = await sendRequest<EventOffering[]>(req);
  return response.data;
};

const getEvent = async (eventId: string): Promise<EventOffering> => {
  const req: AxiosRequestConfig = {
    method: "GET",
    url: `${BASE_EVENT_ENDPOINT}/${eventId}`,
    withCredentials: true,
  };

  const response = await sendRequest<EventOffering>(req);
  return response.data;
};

const deleteEvent = async (eventId: string): Promise<boolean> => {
  const req: AxiosRequestConfig = {
    method: "DELETE",
    url: `${BASE_EVENT_ENDPOINT}/${eventId}`,
    withCredentials: true,
  };

  const response = await sendRequest<boolean>(req);
  return response.data;
};

export { createEvent, getEvents, getEvent, deleteEvent, patchEvent };
