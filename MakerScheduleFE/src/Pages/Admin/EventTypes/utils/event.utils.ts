import type { CreateEventOffering } from "@ms/types/event.types";

const createSaveForm = (event: CreateEventOffering) => {
  if (!event.eventType) {
    throw new Error("Missing event type");
  }

  const form = new FormData();
  form.append("eventName", event.eventName);
  form.append("description", event.description);

  if (event.duration) form.append("duration", event.duration.toString());
  form.append("eventType", event.eventType.toString());
  form.append("FormFile", event.thumbnailFile);

  return form;
};

const createUpdateForm = (event: Partial<CreateEventOffering>) => {
  if (!event.eventType) {
    throw new Error("Missing event type");
  }

  const form = new FormData();
  if (event.eventName) form.append("eventName", event.eventName);
  if (event.description) form.append("description", event.description);
  if (event.duration) form.append("duration", event.duration.toString());

  if (event.eventType) form.append("eventType", event.eventType.toString());
  if (event.thumbnailFile) form.append("FormFile", event.thumbnailFile);

  return form;
};

const durationMap: Record<number, string> = {
  [15 * 60]: "15 minutes",
  [30 * 60]: "30 minutes",
  [45 * 60]: "45 minutes",
  [60 * 60]: "1 hour",
  [90 * 60]: "1 hour 30 minutes",
  [120 * 60]: "2 hours",
  [150 * 60]: "2 hours 30 minutes",
  [180 * 60]: "3 hours",
  [210 * 60]: "3 hours 30 minutes",
  [240 * 60]: "4 hours",
};

const durationOptions = Object.entries(durationMap).map(([value, label]) => ({
  label,
  value: parseInt(value),
}));

export { durationOptions, createSaveForm, createUpdateForm, durationMap };
