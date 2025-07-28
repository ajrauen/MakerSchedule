import type { EventType, PatchEventType } from "@ms/types/event.types";
import { IconButton, TextField, Box } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { FormFooter } from "@ms/Components/FormComponents/FormFooter/FormFooter";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import { useEffect } from "react";
import { createEventType, patchEventType } from "@ms/api/eventTypes.api";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { AxiosResponse } from "axios";

interface CreateEventProps {
  onClose: (refreshData: boolean) => void;
  selectedEventType: EventType;
}

const schema = z.object({
  eventTypeName: z.string().min(3, "Name is required"),
});

const EventTypeDetails = ({ onClose, selectedEventType }: CreateEventProps) => {
  const isNew = selectedEventType?.meta?.isNew;
  const queryClient = useQueryClient();

  const { handleSubmit, control, reset, getValues } = useForm<
    z.infer<typeof schema>
  >({
    resolver: zodResolver(schema),
    defaultValues: {
      eventTypeName: selectedEventType?.name || "",
    },
  });

  useEffect(() => {
    if (!selectedEventType) return;

    reset({
      eventTypeName: selectedEventType.name,
    });
  }, [selectedEventType]);

  const { mutate: saveEventTypeQuery, isPending: isSavePending } = useMutation({
    mutationKey: ["createEventType"],
    mutationFn: createEventType,
    onSuccess: (res, createdEventType) => {
      queryClient.setQueryData(
        ["eventTypes"],
        (oldEventsTypes: AxiosResponse<EventType[]>) => {
          if (!oldEventsTypes) return oldEventsTypes;
          return {
            ...oldEventsTypes,
            data: [
              ...oldEventsTypes.data,
              { id: res.data, name: createdEventType.name },
            ],
          };
        }
      );
    },
  });

  const { mutate: patchEventQuery, isPending: isPatchPending } = useMutation({
    mutationKey: ["patchEvent"],
    mutationFn: (event: EventType) => patchEventType(event),
    onSuccess: (res, savedEvent) => {
      queryClient.setQueryData(
        ["eventTypes"],
        (oldEventsTypes: AxiosResponse<EventType[]>) => {
          if (!oldEventsTypes) return oldEventsTypes;
          return {
            ...oldEventsTypes,
            data: oldEventsTypes.data.map((et) =>
              et.id === selectedEventType.id ? { ...et, ...savedEvent } : et
            ),
          };
        }
      );
    },
  });

  const handleSave = () => {
    const { eventTypeName } = getValues();

    if (isNew) {
      const newEventType: EventType = {
        name: eventTypeName,
      };

      saveEventTypeQuery(newEventType);
      return;
    } else {
      if (!selectedEventType.id) return;

      const updatedEventType: EventType = {
        id: selectedEventType.id,
        name: eventTypeName,
      };

      patchEventQuery(updatedEventType);
    }
  };

  return (
    <div>
      <form
        onSubmit={handleSubmit(handleSave)}
        className="flex flex-col h-full"
      >
        <FormTextField name="eventTypeName" label="Name" control={control} />
        <FormFooter
          onCancel={onClose}
          areActionsDisabled={isSavePending || isPatchPending}
          isLoading={isSavePending || isPatchPending}
          cancelButtonText={isNew ? "Cancel" : "Back"}
          saveButtonText={isNew ? "Save" : "Update"}
        />
      </form>
    </div>
  );
};

export { EventTypeDetails };
