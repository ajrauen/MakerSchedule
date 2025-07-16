import { sendRequest } from "@ms/api/api.utils";
import type { EventOffering } from "@ms/types/event.types";
import type { AxiosRequestConfig } from "axios";

const BASE_EVENT_ENDPOINT = "api/events";

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

const getEvent = async (eventId: number) => {
  const req: AxiosRequestConfig = {
    method: "GET",
    url: `${BASE_EVENT_ENDPOINT}/${eventId}`,
    withCredentials: true,
  };

  return await sendRequest<EventOffering>(req);
};

export { createEvent, getEvents, getEvent };
