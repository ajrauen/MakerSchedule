import type { CreateEventOffering } from "@ms/types/event.types";

const createSaveForm = (event: CreateEventOffering) => {
  const form = new FormData();
  form.append("eventName", event.eventName);
  form.append("description", event.description);
  form.append("Price", event.price.toString());
  form.append("ClassSize", event.classSize.toString());

  if (event.duration) form.append("duration", event.duration.toString());
  form.append("FormFile", event.thumbnailFile);

  if (event.eventTagIds && event.eventTagIds.length > 0) {
    event.eventTagIds.forEach((tag) => {
      form.append("EventTagIds", tag);
    });
  }

  return form;
};

const createUpdateForm = (event: Partial<CreateEventOffering>) => {
  const form = new FormData();
  if (event.eventName) form.append("eventName", event.eventName);
  if (event.description) form.append("description", event.description);
  if (event.duration) form.append("duration", event.duration.toString());
  if (event.eventTagIds && event.eventTagIds.length > 0) {
    event.eventTagIds.forEach((tag) => {
      form.append("EventTagIds", tag);
    });
  }
  // Don't append anything for EventTagIds if array is empty - this will result in null/empty array on backend
  if (event.thumbnailFile) form.append("FormFile", event.thumbnailFile);
  if (event.price) form.append("Price", event.price.toString());
  if (event.classSize) form.append("ClassSize", event.classSize.toString());

  return form;
};

const durationMap: Record<number, string> = {
  [15]: "15 minutes",
  [30]: "30 minutes",
  [45]: "45 minutes",
  [60]: "1 hour",
  [90]: "1 hour 30 minutes",
  [120]: "2 hours",
  [150]: "2 hours 30 minutes",
  [180]: "3 hours",
  [210]: "3 hours 30 minutes",
  [240]: "4 hours",
};

const durationOptions = Object.entries(durationMap).map(([value, label]) => ({
  label,
  value: parseInt(value),
}));

export { durationOptions, createSaveForm, createUpdateForm, durationMap };
