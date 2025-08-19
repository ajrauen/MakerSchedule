import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "@mui/material";
import { useForm } from "react-hook-form";
import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import { useEffect, useState } from "react";
import type { CreateEventOffering, EventOffering } from "@ms/types/event.types";
import {
  createSaveForm,
  createUpdateForm,
  durationOptions,
} from "@ms/Pages/Admin/Events/AdminEventView/utils/event.utils";
import { ImageUpload } from "@ms/Pages/Admin/Events/AdminEventView/AdminEventTable/EventDetails/ImageUpload/ImageUpload";
import { useMutation, useQueryClient } from "@tanstack/react-query";
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
const createEventvalidationSchema = z
  .object({
    eventName: z
      .string()
      .min(3, { error: "Event name must be at least 3 characters" }),
    description: z
      .string()
      .min(3, { error: "Description must be at least 3 characters" }),

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
      };
      reset(structuredClone(editEvent));
    }
  }, [selectedEvent, reset]);

  const handleOnClose = (refreshData = false) => {
    onClose(refreshData);
    reset(createEventInitialFormData);
  };

  const handleSaveSuccess = (eventOffering: EventOffering) => {
    queryClient.setQueryData(["events"], (oldData: EventOffering[]) => {
      if (!oldData) return undefined;
      return [...oldData, eventOffering];
    });
    dispatch(setSelectedEvent(eventOffering));
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
    onSuccess: handleSaveSuccess,
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

    const { description, eventName, duration, thumbnailFile } = getValues();

    if (selectedEvent.meta?.isNew) {
      if (!thumbnailFile) return;

      const eventOffering: CreateEventOffering = {
        description,
        eventName,
        duration: duration,
        thumbnailFile,
      };
      const formEvent = createSaveForm(eventOffering);
      saveEventQuery(formEvent);
      return;
    }

    if (selectedEvent.id) {
      const allValues = getValues();
      const dirtyValues = Object.keys(formState.dirtyFields).reduce(
        (acc: CreateEventFormData, key) => {
          (acc as any)[key] = allValues[key as keyof CreateEventFormData];
          return acc;
        },
        {} as CreateEventFormData
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

  console.log(getValues("thumbnailUrl"));

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
            <div className="flex flex-col gap-3 w-1/2"></div>
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
