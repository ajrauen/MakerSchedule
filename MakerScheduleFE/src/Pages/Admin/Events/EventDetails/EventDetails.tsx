import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import { createEvent } from "@ms/api/event.api";
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
import { Button, IconButton } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { useEffect, useMemo } from "react";

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

interface CreateEventProps {
  onClose: (refreshData: boolean) => void;
  selectedEvent?: EventOffering;
  eventTypes: EventType;
}

const EventDetails = ({
  onClose,
  selectedEvent,
  eventTypes,
}: CreateEventProps) => {
  const {
    getValues,
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<CreateEventFormData>({
    resolver: zodResolver(createEventvalidationSchema),
    defaultValues: createEventInitialFormData,
  });

  useEffect(() => {
    if (selectedEvent) {
      const editEvent: CreateEventFormData = {
        description: selectedEvent.description,
        eventName: selectedEvent.eventName,
        duration: selectedEvent.duration,
        eventType: selectedEvent.eventType.toString(),
        imageFile: new File([], "tmp"),
      };

      reset(editEvent);
    }
  }, [selectedEvent]);

  const handleOnClose = (refreshData = false) => {
    onClose(refreshData);
    reset(createEventInitialFormData);
  };

  const { mutate: saveEvent, isPending: isSavePending } = useMutation({
    mutationKey: ["createClient"],
    mutationFn: createEvent,
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
    saveEvent(formEvent);
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

  return (
    <form onSubmit={handleSubmit(handleSave)} className="flex flex-col">
      <div className="flex flex-col gap-3">
        <div className="ml-auto">
          <IconButton onClick={() => handleOnClose(false)}>
            <CloseIcon />
          </IconButton>
        </div>
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
          rows={10}
        />
        <div>
          <ImageUpload
            control={control}
            name="imageFile"
            error={errors.imageFile?.message}
          />
        </div>
      </div>
      <div className="pt-4 ml-auto gap-3 flex">
        <Button
          type="button"
          onClick={() => handleOnClose(false)}
          disabled={isSavePending}
        >
          Cancel
        </Button>
        <Button type="submit" variant="outlined" disabled={isSavePending}>
          Save
        </Button>
      </div>
    </form>
  );
};

export { EventDetails };
