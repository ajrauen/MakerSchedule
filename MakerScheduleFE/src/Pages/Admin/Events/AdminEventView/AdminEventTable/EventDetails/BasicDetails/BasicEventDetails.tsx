import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "@mui/material";
import { useForm } from "react-hook-form";
import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import { useEffect, useMemo, useState } from "react";
import type { CreateEventOffering, EventOffering } from "@ms/types/event.types";
import {
  createSaveForm,
  createUpdateForm,
  durationOptions,
} from "@ms/Pages/Admin/Events/AdminEventView/utils/event.utils";
import { ImageUpload } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/EventDetails/ImageUpload/ImageUpload";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { createEvent, deleteEvent, patchEvent } from "@ms/api/event.api";
import { z } from "zod";
import { FormFooter } from "@ms/Components/FormComponents/FormFooter/FormFooter";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import {
  selectAdminState,
  setAdminDrawerOpen,
  setSelectedEvent,
} from "@ms/redux/slices/adminSlice";
import RestoreIcon from "@mui/icons-material/Restore";
import { ConfirmationDialog } from "@ms/Components/Dialogs/Confirmation";
import { getEventTags } from "@ms/api/event-tag.api";
const createEventvalidationSchema = z
  .object({
    eventName: z
      .string()
      .min(3, { error: "Event name must be at least 3 characters" }),
    description: z
      .string()
      .min(3, { error: "Description must be at least 3 characters" }),
    eventTagIds: z.array(z.string()),
    duration: z.number().optional(),
    thumbnailUrl: z.string().optional().nullable(),
    thumbnailFile: z
      .instanceof(File, { error: "Event Image is required" })
      .optional(),
  })
  .refine((data) => data.thumbnailUrl || data.thumbnailFile, {
    message: "A Thumbnail is required",
    path: ["thumbnailFile"], // This will show the error on the thumbnailFile field
  });

type CreateEventFormData = z.infer<typeof createEventvalidationSchema>;

const createEventInitialFormData = {
  eventName: "",
  description: "",
  eventTagIds: [],
  duration: undefined,
  thumbnailUrl: undefined,
  thumbnailFile: undefined,
};

interface BasicEventDetailsProps {
  onClose: (refreshData: boolean) => void;
}

const BasicEventDetails = ({ onClose }: BasicEventDetailsProps) => {
  const { selectedEvent } = useAppSelector(selectAdminState);
  const queryClient = useQueryClient();
  const dispatch = useAppDispatch();
  const [showDeleteConfirmation, setShowDeleteConfirmation] =
    useState<boolean>(false);
  const { getValues, control, handleSubmit, formState, reset, watch } =
    useForm<CreateEventFormData>({
      resolver: zodResolver(createEventvalidationSchema),
      defaultValues: createEventInitialFormData,
    });

  const { data: eventTags } = useQuery({
    queryKey: ["eventTags"],
    queryFn: () => getEventTags(),
    staleTime: Infinity,
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
        thumbnailUrl: selectedEvent.thumbnailUrl,
        thumbnailFile: undefined,
        eventTagIds: selectedEvent?.eventTagIds ?? [],
      };
      reset(structuredClone(editEvent));
    }
  }, [selectedEvent]);

  const handleOnClose = (refreshData = false) => {
    onClose(refreshData);
    reset(createEventInitialFormData);
  };

  const handleCreateSuccess = (eventOffering: EventOffering) => {
    queryClient.setQueryData(["events"], (oldData: EventOffering[]) => {
      if (!oldData) return undefined;
      return [...oldData, eventOffering];
    });
    dispatch(setSelectedEvent(eventOffering));
    queryClient.invalidateQueries({
      queryKey: ["eventTags"],
    });
  };

  const handleUpdateSuccess = (data: EventOffering) => {
    queryClient.setQueryData(["events"], (oldData: EventOffering[]) => {
      if (!oldData) return undefined;
      return oldData.map((event: EventOffering) =>
        event.id === data.id ? data : event
      );
    });
    data.meta = {
      isUpdated: true,
    };
    dispatch(setSelectedEvent(data));
    queryClient.invalidateQueries({
      queryKey: ["eventTags"],
    });
  };

  const handleDeleteSuccess = () => {
    if (!selectedEvent) return;

    queryClient.setQueryData(["events"], (oldEvents: EventOffering[]) => {
      if (!oldEvents) return oldEvents;
      return oldEvents.filter((evt) => evt.id !== selectedEvent.id);
    });
    dispatch(setSelectedEvent(undefined));
    dispatch(setAdminDrawerOpen(false));
  };

  const { mutate: saveEventQuery, isPending: isSavePending } = useMutation({
    mutationKey: ["createEvent"],
    mutationFn: createEvent,
    onSuccess: handleCreateSuccess,
    meta: {
      successMessage: "Event Created",
      errorMessage: "Error Creating Event",
    },
  });

  const { mutate: patchEventQuery, isPending: isPatchPending } = useMutation({
    mutationKey: ["patchEvent"],
    mutationFn: ({ id, event }: { id: string; event: FormData }) =>
      patchEvent(id, event),
    onSuccess: handleUpdateSuccess,
    meta: {
      successMessage: "Event Update",
      errorMessage: "Error Updating Event",
    },
  });

  const { mutate: deleteEventMutation } = useMutation({
    mutationKey: ["deleteEvent"],
    mutationFn: deleteEvent,
    onSuccess: handleDeleteSuccess,
  });

  const handleCancelDeleteEvent = () => setShowDeleteConfirmation(false);
  const handleConfirmDeleteEvent = () => {
    if (!selectedEvent?.id) return;

    deleteEventMutation(selectedEvent.id);
  };

  const handleSave = () => {
    if (!selectedEvent) return;

    const { description, eventName, duration, thumbnailFile, eventTagIds } =
      getValues();

    if (selectedEvent.meta?.isNew) {
      if (!thumbnailFile) return;

      const eventOffering: CreateEventOffering = {
        description,
        eventName,
        duration: duration,
        thumbnailFile,
        eventTagIds: eventTagIds,
      };
      const formEvent = createSaveForm(eventOffering);
      saveEventQuery(formEvent);
      return;
    }

    if (selectedEvent.id) {
      const allValues = getValues();
      const dirtyValues = Object.keys(formState.dirtyFields).reduce(
        (acc: Record<string, unknown>, key) => {
          const value = allValues[key as keyof CreateEventFormData];
          acc[key] = value === null ? undefined : value;
          return acc;
        },
        {} as Record<string, unknown>
      );

      const dirtyValuesWithNumberEventType: Partial<CreateEventOffering> = {
        ...dirtyValues,
      };

      const formEvent = createUpdateForm(dirtyValuesWithNumberEventType);

      patchEventQuery({
        event: formEvent,
        id: selectedEvent.id,
      });
    }
  };

  const thumbnailUrl = watch("thumbnailUrl");
  const thumbnailFile = watch("thumbnailFile");

  const handleThunbnailReset = () => {
    reset({
      ...getValues(),
      thumbnailFile: undefined,
      thumbnailUrl: undefined,
    });
  };

  const eventTagsOptions = useMemo(() => {
    if (!eventTags) return [];
    return eventTags.map((tag) => ({
      value: tag.id,
      label: tag.name,
    }));
  }, [eventTags]);

  return (
    <>
      <form
        onSubmit={handleSubmit(handleSave)}
        className="flex flex-col h-full"
      >
        <div className="flex flex-col gap-3">
          <div className="flex flex-row gap-3 ">
            <div className="flex flex-col gap-3 w-1/2">
              <FormTextField name="eventName" label="Name" control={control} />
            </div>
            <div className="flex flex-col gap-3 w-1/2">
              <FormSelect
                name="duration"
                label={"Duration"}
                control={control}
                options={durationOptions}
              />
            </div>
          </div>
          <div className="flex flex-row gap-3 ">
            <div className="flex flex-col gap-3 w-1/2">
              <FormSelect
                name="eventTagIds"
                label={"Event Tags"}
                control={control}
                options={eventTagsOptions}
                multiSelect
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
                        (thumbnailUrl ?? undefined)
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
                    error={formState.errors.thumbnailFile?.message}
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
          showDelete={!selectedEvent?.meta?.isNew}
          onDelete={() => setShowDeleteConfirmation(true)}
          saveButtonText={selectedEvent?.meta?.isNew ? "Create" : "Update"}
        />
      </form>

      <ConfirmationDialog
        open={showDeleteConfirmation}
        onCancel={handleCancelDeleteEvent}
        onConfirm={handleConfirmDeleteEvent}
        title="Delete Event"
        details="You sure? Because you cant come back from this!"
      />
    </>
  );
};

export { BasicEventDetails };
