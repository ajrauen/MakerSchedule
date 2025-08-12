import { sendAxiosRequest } from "@ms/api/api";
import type { CreateEventType, EventType } from "@ms/types/event.types";

const BASE_METADATA_ENDPOINT = "api/EventTypes";

const getEventTypes = async () => {
  const url = `${BASE_METADATA_ENDPOINT}`;
  const req = {
    method: "GET",
    url: url,
  };
  const types = await sendAxiosRequest<EventType[]>(req);

  return types;
};

const createEventType = async (eventTypeData: CreateEventType) => {
  const url = `${BASE_METADATA_ENDPOINT}`;
  const req = {
    method: "Post",
    url: url,
    data: eventTypeData,
  };

  const type = await sendAxiosRequest<string>(req);

  return type;
};

const patchEventType = async (eventTypeData: EventType) => {
  const url = `${BASE_METADATA_ENDPOINT}/${eventTypeData.id}`;
  const req = {
    method: "Patch",
    url: url,
    data: eventTypeData,
  };
  const type = await sendAxiosRequest<string>(req);

  return type;
};

const deleteEventType = async (eventTypeId: string) => {
  const url = `${BASE_METADATA_ENDPOINT}/${eventTypeId}`;
  const req = {
    method: "Delete",
    url: url,
  };
  await sendAxiosRequest<boolean>(req);

  return eventTypeId;
};

export { createEventType, deleteEventType, getEventTypes, patchEventType };
