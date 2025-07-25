import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "@mui/material";
import { useForm } from "react-hook-form";
import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import { useEffect, useMemo, useState } from "react";
import type {
  CreateEventOffering,
  EventOffering,
  EventType,
} from "@ms/types/event.types";
import {
  createSaveForm,
  durationOptions,
} from "@ms/Pages/Admin/Events/utils/event.utils";
import { ImageUpload } from "@ms/Pages/Admin/Events/EventDetails/ImageUpload/ImageUpload";
import { useMutation } from "@tanstack/react-query";
import { createEvent, patchEvent } from "@ms/api/event.api";
import { z } from "zod";

const createEventvalidationSchema = z.object({
  eventName: z
    .string()
    .min(3, { error: "Event name must be at least 3 characters" }),
  description: z
    .string()
    .min(3, { error: "Description must be at least 3 characters" }),

  duration: z.number().optional(),
  imageFile: z.instanceof(File, { error: "Event Image is required" }),
  eventType: z.string(),
});

type CreateEventFormData = z.infer<typeof createEventvalidationSchema>;

const createEventInitialFormData = {
  eventName: "",
  eventType: "",
  description: "",
  duration: undefined,
};

interface BasicEventDetailsProps {
  onClose: (refreshData: boolean) => void;
  selectedEvent: EventOffering;
  eventTypes: EventType;
}

const BasicEventDetails = ({
  onClose,
  selectedEvent,
  eventTypes,
}: BasicEventDetailsProps) => {
  const [newEventFileUrl, setNewEventFileUrl] = useState<string | null>(null);

  const {
    getValues,
    control,
    handleSubmit,
    formState: { errors },
    reset,
    watch,
  } = useForm<CreateEventFormData>({
    resolver: zodResolver(createEventvalidationSchema),
    defaultValues: createEventInitialFormData,
  });

  useEffect(() => {
    clearNewEventFileUrl();

    if (selectedEvent.meta?.isNew) {
      reset(createEventInitialFormData);
      return;
    }

    if (selectedEvent) {
      const editEvent: CreateEventFormData = {
        description: selectedEvent.description,
        eventName: selectedEvent.eventName,
        duration: selectedEvent.duration,
        eventType: selectedEvent.eventType?.toString() ?? "",
        imageFile: new File([], "tmp"),
      };

      reset(editEvent);
    }
  }, [selectedEvent]);

  const handleOnClose = (refreshData = false) => {
    clearNewEventFileUrl();

    onClose(refreshData);
    reset(createEventInitialFormData);
  };

  const clearNewEventFileUrl = () => {
    if (newEventFileUrl) {
      URL.revokeObjectURL(newEventFileUrl);
      setNewEventFileUrl(null);
    }
  };

  const { mutate: saveEventQuery, isPending: isSavePending } = useMutation({
    mutationKey: ["createEvent"],
    mutationFn: createEvent,
    onSuccess: () => handleOnClose(true),
  });

  const { mutate: patchEventQuery, isPending: isPatchPending } = useMutation({
    mutationKey: ["patchEvent"],
    mutationFn: ({ id, event }: { id: string; event: FormData }) =>
      patchEvent(id, event),
    onSuccess: () => handleOnClose(true),
  });

  const handleSave = () => {
    const { description, eventName, duration, imageFile, eventType } =
      getValues();

    const eventOffering: CreateEventOffering = {
      description,
      eventName,
      duration: duration,
      eventType: parseInt(eventType),
      imageFile,
    };
    const formEvent = createSaveForm(eventOffering);

    if (selectedEvent.id) {
      patchEventQuery({
        event: formEvent,
        id: selectedEvent.id,
      });
    } else {
      saveEventQuery(formEvent);
    }
  };

  const eventTypeOptions = useMemo(() => {
    const options = [];
    for (const key in eventTypes) {
      options.push({
        value: key,
        label: eventTypes[key],
      });
    }
    return options;
  }, [eventTypes]);

  const fileUrlWatch = watch("imageFile");

  useEffect(() => {
    if (fileUrlWatch) {
      const img = new Image();
      img.src = URL.createObjectURL(fileUrlWatch);
      setNewEventFileUrl(img.src);
    }
  }, [fileUrlWatch]);

  return (
    <form onSubmit={handleSubmit(handleSave)} className="flex flex-col h-full">
      <div className="flex flex-col gap-3">
        <div className="flex flex-row gap-3 ">
          <div className="flex flex-col gap-3 w-1/2">
            <FormTextField name="eventName" label="Name" control={control} />
          </div>
          <div className="flex flex-col gap-3 w-1/2">
            <FormSelect
              name="duration"
              control={control}
              options={durationOptions}
            />
          </div>
        </div>
        <div className="flex flex-row gap-3 ">
          <div className="flex flex-col gap-3 w-1/2">
            <FormSelect
              name="eventType"
              control={control}
              options={eventTypeOptions}
            />
          </div>
        </div>
        <FormTextField
          name="description"
          label="Description"
          control={control}
          multiline
          rows={7}
        />
        {selectedEvent.fileUrl ? (
          <div>
            <img
              className="h-auto  object-fit w-full lg:w-1/3 lg-flex-shrink-0 lg:object-contain aspect-4/3 object-cover"
              src={newEventFileUrl ?? selectedEvent.fileUrl}
              alt="filter"
            />
          </div>
        ) : (
          <div>
            <ImageUpload
              control={control}
              name="imageFile"
              error={errors.imageFile?.message}
            />
          </div>
        )}
      </div>
      <div className="pt-4 ml-auto gap-3 flex">
        <Button
          type="button"
          onClick={() => handleOnClose(false)}
          disabled={isSavePending}
        >
          Cancel
        </Button>
        <Button
          type="submit"
          variant="outlined"
          disabled={isSavePending || isPatchPending}
          loading={isSavePending || isPatchPending}
        >
          Save
        </Button>
      </div>
    </form>
  );
};

export { BasicEventDetails };
