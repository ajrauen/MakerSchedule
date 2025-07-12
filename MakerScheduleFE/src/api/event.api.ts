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
  const response = await sendRequest<number>(req);

  return response;
};

const getEvents = async () => {
  const req: AxiosRequestConfig = {
    method: "GET",
    url: `${BASE_EVENT_ENDPOINT}`,
    withCredentials: true,
  };
  const response = await sendRequest<EventOffering[]>(req);
  console.log(response.data);

  return response;
};

const getEvent = async (eventId: string) => {
  const req: AxiosRequestConfig = {
    method: "GET",
    url: `${BASE_EVENT_ENDPOINT}/${eventId}`,
    withCredentials: true,
  };
  const response = await sendRequest<EventOffering>(req);
  console.log(response.data);

  return response;
};

export { createEvent, getEvents, getEvent };
