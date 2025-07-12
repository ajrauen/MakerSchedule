import type { CreateEventOffering } from "@ms/types/event.types";

const createSaveForm = (event: CreateEventOffering) => {
  const form = new FormData();
  form.append("eventName", event.eventName);
  form.append("description", event.description);

  if (event.duration) form.append("duration", event.duration.toString());
  form.append("eventType", event.eventType.toString());

  form.append("FormFile", event.imageFile);

  return form;
};

export { createSaveForm };
