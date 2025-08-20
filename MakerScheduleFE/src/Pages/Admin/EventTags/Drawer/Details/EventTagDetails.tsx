import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import { useEffect, useState } from "react";

import { useMutation, useQueryClient } from "@tanstack/react-query";
import { z } from "zod";
import { FormFooter } from "@ms/Components/FormComponents/FormFooter/FormFooter";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import {
  selectAdminState,
  setAdminDrawerOpen,
  setSelectedEventTag,
} from "@ms/redux/slices/adminSlice";
import { ConfirmationDialog } from "@ms/Components/Dialogs/Confirmation";
import {
  createEventTag,
  deleteEventTag,
  patchEventTag,
} from "@ms/api/event-tag.api";
import type { CreateEventTag, EventTag } from "@ms/types/event-tags.types";
import { FormColorPicker } from "@ms/Components/FormComponents/FormColorPicker/FormColorPicker";
const createEventTagvalidationSchema = z.object({
  name: z
    .string()
    .min(3, { error: "Event name must be at least 3 characters" }),
});

type CreateEventFormData = z.infer<typeof createEventTagvalidationSchema>;

const createEventTagInitialFormData = {
  name: "",
};

interface BasicEventDetailsProps {
  onClose: (refreshData: boolean) => void;
}

const EventTagDetails = ({ onClose }: BasicEventDetailsProps) => {
  const { selectedEventTag } = useAppSelector(selectAdminState);
  const queryClient = useQueryClient();
  const dispatch = useAppDispatch();
  const [showDeleteConfirmation, setShowDeleteConfirmation] =
    useState<boolean>(false);
  const { getValues, control, handleSubmit, formState, reset } =
    useForm<CreateEventFormData>({
      resolver: zodResolver(createEventTagvalidationSchema),
      defaultValues: createEventTagInitialFormData,
    });

  useEffect(() => {
    if (!selectedEventTag) return;

    if (selectedEventTag.meta?.isNew) {
      reset(structuredClone(createEventTagInitialFormData));
      return;
    }

    if (selectedEventTag) {
      const editEvent: CreateEventFormData = {
        name: selectedEventTag.name,
      };
      reset(structuredClone(editEvent));
    }
  }, [selectedEventTag, reset]);

  const handleOnClose = (refreshData = false) => {
    onClose(refreshData);
    reset(createEventTagInitialFormData);
  };

  const handleCreateSuccess = (eventTag: EventTag) => {
    queryClient.setQueryData(["eventTags"], (oldData: EventTag[]) => {
      if (!oldData) return undefined;
      return [...oldData, eventTag];
    });
    dispatch(setSelectedEventTag(eventTag));
  };

  const handleUpdateSuccess = (data: EventTag) => {
    queryClient.setQueryData(["eventTags"], (oldData: EventTag[]) => {
      if (!oldData) return undefined;
      return oldData.map((event: EventTag) =>
        event.id === data.id ? data : event
      );
    });
    data.meta = {
      isUpdated: true,
    };
    dispatch(setSelectedEventTag(data));
  };

  const handleDeleteSuccess = () => {
    if (!selectedEventTag) return;

    queryClient.setQueryData(["eventTags"], (oldEvents: EventTag[]) => {
      if (!oldEvents) return oldEvents;
      return oldEvents.filter((evt) => evt.id !== selectedEventTag.id);
    });
    dispatch(setSelectedEventTag(undefined));
    dispatch(setAdminDrawerOpen(false));
  };

  const { mutate: saveEventQuery, isPending: isSavePending } = useMutation({
    mutationKey: ["createEventTag"],
    mutationFn: createEventTag,
    onSuccess: handleCreateSuccess,
    meta: {
      successMessage: "Event Created",
      errorMessage: "Error Creating Event",
    },
  });

  const { mutate: patchEventQuery, isPending: isPatchPending } = useMutation({
    mutationKey: ["patchEvent"],
    mutationFn: ({
      id,
      eventTag,
    }: {
      id: string;
      eventTag: Partial<EventTag>;
    }) => patchEventTag(id, eventTag),
    onSuccess: handleUpdateSuccess,
    meta: {
      successMessage: "Event Update",
      errorMessage: "Error Updating Event",
    },
  });

  const { mutate: deleteEventMutation } = useMutation({
    mutationKey: ["deleteEvent"],
    mutationFn: deleteEventTag,
    onSuccess: handleDeleteSuccess,
  });

  const handleCancelDeleteEvent = () => setShowDeleteConfirmation(false);
  const handleConfirmDeleteEvent = () => {
    if (!selectedEventTag?.id) return;

    deleteEventMutation(selectedEventTag.id);
  };

  const handleSave = () => {
    if (!selectedEventTag) return;

    const { name } = getValues();

    if (selectedEventTag.meta?.isNew) {
      const eventTag: CreateEventTag = {
        name,
      };
      saveEventQuery(eventTag);
      return;
    }

    if (selectedEventTag.id) {
      const allValues = getValues();
      const dirtyValues = Object.keys(formState.dirtyFields).reduce(
        (acc: CreateEventFormData, key) => {
          (acc as any)[key] = allValues[key as keyof CreateEventFormData];
          return acc;
        },
        {} as CreateEventFormData
      );

      const dirtyValuesWithNumberEventType: Partial<EventTag> = {
        ...dirtyValues,
      };

      const eventTag: Partial<EventTag> = {
        ...dirtyValuesWithNumberEventType,
      };

      patchEventQuery({
        eventTag: eventTag,
        id: selectedEventTag.id,
      });
    }
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
              <FormTextField name="name" label="Name" control={control} />
            </div>
          </div>
        </div>
        <FormFooter
          onCancel={handleOnClose}
          areActionsDisabled={isSavePending || isPatchPending}
          isLoading={isSavePending || isPatchPending}
          showDelete={!selectedEventTag?.meta?.isNew}
          onDelete={() => setShowDeleteConfirmation(true)}
          saveButtonText={selectedEventTag?.meta?.isNew ? "Create" : "Update"}
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

export { EventTagDetails };
