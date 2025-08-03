import type { CreateEventType, EventType } from "@ms/types/event.types";
import { FormFooter } from "@ms/Components/FormComponents/FormFooter/FormFooter";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import { useEffect } from "react";
import { createEventType, patchEventType } from "@ms/api/eventTypes.api";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { AxiosResponse } from "axios";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import {
  selectAdminState,
  setAdminDrawerOpen,
  setSelectedEventType,
} from "@ms/redux/slices/adminSlice";

interface CreateEventProps {}

const schema = z.object({
  eventTypeName: z.string().min(3, "Name is required"),
});

const EventTypeDetails = ({}: CreateEventProps) => {
  const { selectedEventType } = useAppSelector(selectAdminState);

  const isNew = selectedEventType?.meta?.isNew;
  const queryClient = useQueryClient();
  const dispatch = useAppDispatch();

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
      queryClient.invalidateQueries({
        queryKey: ["event"],
      });
      queryClient.setQueryData(
        ["eventTypes"],
        (oldEventsTypes: AxiosResponse<EventType[]>) => {
          if (!oldEventsTypes) return oldEventsTypes;
          const newEventType: EventType = {
            id: res.data,
            name: createdEventType.name,
          };
          dispatch(setSelectedEventType(newEventType));
          return {
            ...oldEventsTypes,
            data: [...oldEventsTypes.data, newEventType],
          };
        }
      );
    },
    meta: {
      successMessage: "Event Type Created",
      errorMessage: "Failed to create Event Type",
    },
  });

  const { mutate: patchEventQuery, isPending: isPatchPending } = useMutation({
    mutationKey: ["patchEvent"],
    mutationFn: (event: EventType) => patchEventType(event),
    onSuccess: (res, savedEvent) => {
      //
      queryClient.invalidateQueries({
        queryKey: ["events"],
        type: "all",
        refetchType: "all",
      });
      queryClient.invalidateQueries({
        queryKey: ["events-metadata"],
        type: "all",
        refetchType: "all",
      });
      queryClient.setQueryData(
        ["eventTypes"],
        (oldEventsTypes: AxiosResponse<EventType[]>) => {
          if (!oldEventsTypes) return oldEventsTypes;
          return {
            ...oldEventsTypes,
            data: oldEventsTypes.data.map((et) =>
              et.id === selectedEventType?.id ? { ...et, ...savedEvent } : et
            ),
          };
        }
      );
    },
    meta: {
      successMessage: "Event Type Updated",
      errorMessage: "Failed to update Event Type",
    },
  });

  const handleSave = () => {
    const { eventTypeName } = getValues();

    if (isNew) {
      const newEventType: CreateEventType = {
        name: eventTypeName,
      };

      saveEventTypeQuery(newEventType);
      return;
    } else {
      if (!selectedEventType?.id) return;

      const updatedEventType: EventType = {
        id: selectedEventType.id,
        name: eventTypeName,
      };

      patchEventQuery(updatedEventType);
    }
  };

  const handleClose = () => {
    dispatch(setSelectedEventType());
    dispatch(setAdminDrawerOpen(false));
    reset();
  };

  return (
    <div>
      <form
        onSubmit={handleSubmit(handleSave)}
        className="flex flex-col h-full"
      >
        <FormTextField name="eventTypeName" label="Name" control={control} />
        <FormFooter
          onCancel={handleClose}
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
