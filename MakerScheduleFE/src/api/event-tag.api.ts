import { sendRequest } from "@ms/api/api.utils";
import type { CreateEventTag, EventTag } from "@ms/types/event-tags.types";
import type { AxiosRequestConfig } from "axios";

const BASE_EVENT_TAG_ENDPOINT = "api/EventTags";

const createEventTag = async (eventTag: CreateEventTag): Promise<EventTag> => {
  const req: AxiosRequestConfig = {
    method: "POST",
    url: `${BASE_EVENT_TAG_ENDPOINT}`,
    withCredentials: true,
    data: eventTag,
    headers: {
      "Content-Type": "application/json",
    },
  };

  const response = await sendRequest<EventTag>(req);
  return response.data;
};

const patchEventTag = async (
  id: string,
  event: Partial<EventTag>
): Promise<EventTag> => {
  const req: AxiosRequestConfig = {
    method: "PATCH",
    url: `${BASE_EVENT_TAG_ENDPOINT}/${id}`,
    withCredentials: true,
    data: event,
  };

  const response = await sendRequest<EventTag>(req);
  return response.data;
};

const getEventTags = async (): Promise<EventTag[]> => {
  const req: AxiosRequestConfig = {
    method: "GET",
    url: `${BASE_EVENT_TAG_ENDPOINT}`,
    withCredentials: true,
  };

  const response = await sendRequest<EventTag[]>(req);
  return response.data;
};

const getEventTag = async (eventTagId: string): Promise<EventTag> => {
  const req: AxiosRequestConfig = {
    method: "GET",
    url: `${BASE_EVENT_TAG_ENDPOINT}/${eventTagId}`,
    withCredentials: true,
  };

  const response = await sendRequest<EventTag>(req);
  return response.data;
};

const deleteEventTag = async (eventTagId: string): Promise<boolean> => {
  const req: AxiosRequestConfig = {
    method: "DELETE",
    url: `${BASE_EVENT_TAG_ENDPOINT}/${eventTagId}`,
    withCredentials: true,
  };

  const response = await sendRequest<boolean>(req);
  return response.data;
};

export {
  createEventTag,
  patchEventTag,
  getEventTags,
  getEventTag,
  deleteEventTag,
};
