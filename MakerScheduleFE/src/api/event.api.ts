import { sendRequest } from "@ms/api/api.utils";
import type { EventOffering } from "@ms/types/event.types";
import type { AxiosRequestConfig } from "axios";

const BASE_EVENT_ENDPOINT = "api/Events";

const createEvent = async (event: FormData) => {
  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_EVENT_ENDPOINT}`,
    withCredentials: true,
    data: event,
  };

  return await sendRequest<number>(req);
};

const getEvents = async () => {
  const req: AxiosRequestConfig = {
    method: "GET",
    url: `${BASE_EVENT_ENDPOINT}`,
    withCredentials: true,
  };

  return await sendRequest<EventOffering[]>(req);
};

const getEvent = async (eventId: string) => {
  const req: AxiosRequestConfig = {
    method: "GET",
    url: `${BASE_EVENT_ENDPOINT}/${eventId}`,
    withCredentials: true,
  };

  return await sendRequest<EventOffering>(req);
};

const deleteEvent = async (eventId: string) => {
  const req: AxiosRequestConfig = {
    method: "DELETE",
    url: `${BASE_EVENT_ENDPOINT}/${eventId}`,
    withCredentials: true,
  };

  return await sendRequest<boolean>(req);
};

export { createEvent, getEvents, getEvent, deleteEvent };
