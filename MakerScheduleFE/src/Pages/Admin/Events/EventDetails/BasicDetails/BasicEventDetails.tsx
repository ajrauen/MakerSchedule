import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "@mui/material";
import { useForm } from "react-hook-form";
import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import { useEffect, useMemo } from "react";
import type { CreateEventOffering, EventType } from "@ms/types/event.types";
import {
  createSaveForm,
  createUpdateForm,
  durationOptions,
} from "@ms/Pages/Admin/Events/utils/event.utils";
import { ImageUpload } from "@ms/Pages/Admin/Events/EventDetails/ImageUpload/ImageUpload";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createEvent, patchEvent } from "@ms/api/event.api";
import { z } from "zod";
import { FormFooter } from "@ms/Components/FormComponents/FormFooter/FormFooter";
import { useAppSelector } from "@ms/redux/hooks";
import { selectAdminState } from "@ms/redux/slices/adminSlice";
import RestoreIcon from "@mui/icons-material/Restore";
const createEventvalidationSchema = z.object({
  eventName: z
    .string()
    .min(3, { error: "Event name must be at least 3 characters" }),
  description: z
    .string()
    .min(3, { error: "Description must be at least 3 characters" }),

  duration: z.number().optional(),
  thumbnailUrl: z.string().optional(),
  thumbnailFile: z
    .instanceof(File, { error: "Event Image is required" })
    .optional(),
  eventTypeId: z.string(),
});

type CreateEventFormData = z.infer<typeof createEventvalidationSchema>;

const createEventInitialFormData = {
  eventName: "",
  eventTypeId: "",
  description: "",
  duration: undefined,
  imageUrl: undefined,
  thumbnailFile: undefined,
};

interface BasicEventDetailsProps {
  onClose: (refreshData: boolean) => void;
  eventTypes: EventType[];
}

const BasicEventDetails = ({ onClose, eventTypes }: BasicEventDetailsProps) => {
  const { selectedEvent } = useAppSelector(selectAdminState);
  const queryClicent = useQueryClient();

  const {
    getValues,
    control,
    handleSubmit,
    formState: { errors, dirtyFields },
    reset,
    watch,
  } = useForm<CreateEventFormData>({
    resolver: zodResolver(createEventvalidationSchema),
    defaultValues: createEventInitialFormData,
  });

  useEffect(() => {
    if (!selectedEvent) return;

    if (selectedEvent.meta?.isNew) {
      reset(structuredClone(createEventInitialFormData));
      return;
    }

    if (selectedEvent) {
      const editEvent: CreateEventFormData = {
        description: selectedEvent.description,
        eventName: selectedEvent.eventName,
        duration: selectedEvent.duration,
        eventTypeId: selectedEvent.eventType?.id || "",
        thumbnailUrl: selectedEvent.thumbnailUrl,
        thumbnailFile: undefined,
      };
      reset(structuredClone(editEvent));
    }
  }, [selectedEvent]);

  const handleOnClose = (refreshData = false) => {
    onClose(refreshData);
    reset(createEventInitialFormData);
    queryClicent.invalidateQueries({
      queryKey: ["events"],
    });
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
    if (!selectedEvent) return;

    const { description, eventName, duration, thumbnailFile, eventTypeId } =
      getValues();

    if (selectedEvent.meta?.isNew) {
      if (!thumbnailFile) return;

      const eventOffering: CreateEventOffering = {
        description,
        eventName,
        duration: duration,
        eventTypeId: eventTypeId,
        thumbnailFile,
      };
      const formEvent = createSaveForm(eventOffering);
      saveEventQuery(formEvent);
      return;
    }

    if (selectedEvent.id) {
      const allValues = getValues();
      const dirtyValues = Object.keys(dirtyFields).reduce(
        (acc: CreateEventFormData, key) => {
          (acc as any)[key] = allValues[key as keyof CreateEventFormData];
          return acc;
        },
        {} as CreateEventFormData
      );

      if (dirtyValues.eventTypeId) {
        (dirtyValues as any).eventType = dirtyValues.eventTypeId;
      }

      const dirtyValuesWithNumberEventType: Partial<CreateEventOffering> = {
        ...dirtyValues,
        eventTypeId: dirtyValues.eventTypeId
          ? dirtyValues.eventTypeId
          : undefined,
      };

      const formEvent = createUpdateForm(dirtyValuesWithNumberEventType);

      patchEventQuery({
        event: formEvent,
        id: selectedEvent.id,
      });
    }
  };

  const eventTypeOptions = useMemo(() => {
    const options = eventTypes.map((event) => ({
      value: event.id ?? "",
      label: event.name,
    }));

    return options;
  }, [eventTypes]);

  const thumbnailUrl = watch("thumbnailUrl");
  const thumbnailFile = watch("thumbnailFile");

  const handleThunbnailReset = () => {
    reset({
      ...getValues(),
      thumbnailFile: undefined,
      thumbnailUrl: undefined,
    });
  };

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
              name="eventTypeId"
              control={control}
              options={eventTypeOptions}
            />
          </div>
          <div className="flex flex-col gap-3 w-1/2">
            {thumbnailUrl || thumbnailFile ? (
              <div className="flex flex-row ">
                <div className="flex flex-col ">
                  <img
                    className="w-18 h-full object-cover mb-4"
                    src={
                      (thumbnailFile && URL.createObjectURL(thumbnailFile)) ||
                      thumbnailUrl
                    }
                    alt="Event Preview"
                  />
                </div>

                <span className="">
                  <Button
                    startIcon={<RestoreIcon />}
                    onClick={handleThunbnailReset}
                  >
                    Reset
                  </Button>
                </span>
              </div>
            ) : (
              <div>
                <ImageUpload
                  control={control}
                  name="thumbnailFile"
                  error={errors.thumbnailFile?.message}
                />
              </div>
            )}
          </div>
        </div>
        <FormTextField
          name="description"
          label="Description"
          control={control}
          multiline
          rows={7}
        />
      </div>
      <FormFooter
        onCancel={handleOnClose}
        areActionsDisabled={isSavePending || isPatchPending}
        isLoading={isSavePending || isPatchPending}
      />
    </form>
  );
};

export { BasicEventDetails };
