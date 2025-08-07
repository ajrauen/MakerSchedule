import { zodResolver } from "@hookform/resolvers/zod";
import { getAvailableDomainUserLeaders } from "@ms/api/domain-user.api";
import { createOccurrence } from "@ms/api/occurrence.api";
import { FormDateTime } from "@ms/Components/FormComponents/FormDateTime/FormDateTime";
import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import { FormFooter } from "@ms/Components/FormComponents/FormFooter/FormFooter";
import { durationOptions } from "@ms/Pages/Admin/Events/AdminEventView/utils/event.utils";
import type { SelectOption } from "@ms/types/form.types";
import type { CreateOccurrence, Occurrence } from "@ms/types/occurrence.types";
import type { PickerValidDate } from "@mui/x-date-pickers";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import type { DomainUser } from "@ms/types/domain-user.types";
import { setSelectedEventOccurrence } from "@ms/redux/slices/adminSlice";
import { useAppDispatch } from "@ms/redux/hooks";
import type { AxiosResponse } from "axios";
import { useAdminEventsData } from "@ms/hooks/useAdminEventsData";

interface CalendarOccurrenceCreateProps {
  selectedDate: Date;
  onCancel: () => void;
  onSuccess?: () => void;
}

// Simple, focused validation schema for calendar creation
const calendarOccurrenceValidationSchema = z.object({
  eventId: z.string().min(1, { message: "Event is required" }),
  scheduleStart: z.date(),
  duration: z.number().min(1, { message: "Duration is required" }),
  leaders: z.array(z.string()).optional(),
});

type CalendarOccurrenceFormData = z.infer<
  typeof calendarOccurrenceValidationSchema
>;

const CalendarOccurrenceCreate = ({
  selectedDate,
  onCancel,
  onSuccess,
}: CalendarOccurrenceCreateProps) => {
  const [availableLeaderOptions, setAvailableLeaderOptions] = useState<
    SelectOption[]
  >([]);
  const [removedLeaders, setRemovedLeaders] = useState<DomainUser[]>([]);

  const { events } = useAdminEventsData();
  const queryClient = useQueryClient();
  const dispatch = useAppDispatch();

  const { control, handleSubmit, watch, getValues } =
    useForm<CalendarOccurrenceFormData>({
      resolver: zodResolver(calendarOccurrenceValidationSchema),
      defaultValues: {
        scheduleStart: selectedDate,
        leaders: [],
      },
    });

  const time = watch("scheduleStart");
  const duration = watch("duration");

  const {
    data: availableLeaderResponse,
    isFetching: isLoadingAvailableLeaders,
  } = useQuery({
    queryKey: ["available-leaders-calendar", time, duration],
    queryFn: async () => {
      if (!time || !duration) return null;

      return getAvailableDomainUserLeaders(
        time.toISOString(),
        duration,
        "", // No existing occurrence ID for new creation
        [] // No existing leaders
      );
    },
    staleTime: 4000,
    enabled: !!time && !!duration,
  });

  // Handle successful creation - update calendar cache
  const handleCreateSuccess = (data: AxiosResponse<Occurrence>) => {
    // Update the occurrences cache for calendar view
    queryClient.setQueryData(
      ["occurrences"],
      (oldData: AxiosResponse<Occurrence[]>) => {
        if (!oldData) return { data: [data.data] };
        return {
          ...oldData,
          data: [...oldData.data, data.data],
        };
      }
    );

    // Set the created occurrence in Redux for potential navigation
    dispatch(setSelectedEventOccurrence(data.data));

    onSuccess?.();
  };

  const { mutate: createOccurrenceMutation, isPending: isCreatePending } =
    useMutation({
      mutationFn: ({ occurrence }: { occurrence: CreateOccurrence }) =>
        createOccurrence(occurrence),
      onSuccess: handleCreateSuccess,
      meta: {
        successMessage: "Occurrence Created",
        errorMessage: "Failed to create occurrence",
      },
    });

  // Set up available leaders when data changes
  useEffect(() => {
    if (!availableLeaderResponse?.data) {
      setAvailableLeaderOptions([]);
      return;
    }

    const leaderOptions: SelectOption[] = availableLeaderResponse.data.map(
      (user: DomainUser) => ({
        label: `${user.firstName} ${user.lastName}`,
        value: user.id,
      })
    );

    setAvailableLeaderOptions(leaderOptions);
    setRemovedLeaders([]); // Clear any removed leaders for new creation
  }, [availableLeaderResponse?.data]);

  const shouldDisableDate = (date: PickerValidDate) => {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    return date < today;
  };

  const onSave = () => {
    const { eventId, scheduleStart, duration, leaders } = getValues();

    const createOccurrenceObj: CreateOccurrence = {
      eventId,
      scheduleStart: scheduleStart.toISOString(),
      duration,
      leaders: leaders ?? [],
    };

    createOccurrenceMutation({ occurrence: createOccurrenceObj });
  };

  const eventOptions = events
    .filter((event) => event.id)
    .map((event) => ({
      label: event.eventName,
      value: event.id!,
    }));

  return (
    <form onSubmit={handleSubmit(onSave)}>
      <div className="p-4 space-y-2 bg-white rounded shadow flex flex-col gap-6">
        <h2 className="text-xl font-semibold">Create New Occurrence</h2>

        <FormSelect
          name="eventId"
          control={control}
          options={eventOptions}
          label="Event"
        />

        <FormDateTime
          control={control}
          name="scheduleStart"
          label="Start Time"
          shouldDisableDate={shouldDisableDate}
        />

        <FormSelect
          name="duration"
          control={control}
          options={durationOptions}
          label="Duration"
        />

        <FormSelect
          name="leaders"
          control={control}
          options={availableLeaderOptions}
          noOptionsText="No Leaders available"
          displayEmpty={true}
          label="Assign Leaders"
          multiSelect
          isLoading={isLoadingAvailableLeaders}
        />

        {removedLeaders && removedLeaders.length > 0 && (
          <div>
            Leaders removed due to unavailability:
            {removedLeaders.map((leader, idx) => (
              <span key={leader.id} className="pl-2">
                {`${leader.lastName}${idx === removedLeaders.length - 1 ? "" : ", "}`}
              </span>
            ))}
          </div>
        )}

        <FormFooter
          onCancel={onCancel}
          areActionsDisabled={isCreatePending}
          isLoading={isCreatePending}
          cancelButtonText="Cancel"
          saveButtonText="Create Occurrence"
        />
      </div>
    </form>
  );
};

export { CalendarOccurrenceCreate };
