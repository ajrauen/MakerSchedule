import { sendAxiosRequest } from "@ms/api/api";
import type { CreateEventType, EventType } from "@ms/types/event.types";

const BASE_METADATA_ENDPOINT = "api/EventTypes";

const getEventTypes = async () => {
  const url = `${BASE_METADATA_ENDPOINT}`;
  const req = {
    method: "GET",
    url: url,
  };

  return await sendAxiosRequest<EventType[]>(req);
};

const createEventType = async (eventTypeData: CreateEventType) => {
  const url = `${BASE_METADATA_ENDPOINT}`;
  const req = {
    method: "Post",
    url: url,
    data: eventTypeData,
  };

  return await sendAxiosRequest<string>(req);
};

const patchEventType = async (eventTypeData: EventType) => {
  const url = `${BASE_METADATA_ENDPOINT}/${eventTypeData.id}`;
  const req = {
    method: "Patch",
    url: url,
    data: eventTypeData,
  };

  return await sendAxiosRequest<string>(req);
};

const deleteEventType = async (eventTypeId: string) => {
  const url = `${BASE_METADATA_ENDPOINT}/${eventTypeId}`;
  const req = {
    method: "Delete",
    url: url,
  };

  return await sendAxiosRequest<boolean>(req);
};

export { createEventType, deleteEventType, getEventTypes, patchEventType };
