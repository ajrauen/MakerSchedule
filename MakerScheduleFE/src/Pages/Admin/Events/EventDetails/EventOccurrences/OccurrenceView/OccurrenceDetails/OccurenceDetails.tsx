import { zodResolver } from "@hookform/resolvers/zod";
import {
  getAvailableDomainUserLeaders,
  getDomainUsers,
} from "@ms/api/domain-user.api";
import { createOccurrence, updateOccurrence } from "@ms/api/occurrence.api";
import { FormDateTime } from "@ms/Components/FormComponents/FormDateTime/FormDateTime";
import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import { durationOptions } from "@ms/Pages/Admin/Events/utils/event.utils";
import type { SelectOption } from "@ms/types/form.types";
import type {
  CreateOccurrence,
  Occurrence,
  UpdateOccurrence,
} from "@ms/types/occurrence.types";
import type { PickerValidDate } from "@mui/x-date-pickers";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState, useMemo } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import type { DomainUser } from "@ms/types/domain-user.types";
import { FormFooter } from "@ms/Components/FormComponents/FormFooter/FormFooter";
import {
  selectAdminState,
  setSelectedEventOccurrence,
} from "@ms/redux/slices/adminSlice";
import { useAppDispatch, useAppSelector } from "@ms/redux/hooks";
import type { AxiosResponse } from "axios";
import type { EventOffering } from "@ms/types/event.types";

interface OccurrenceDetailsProps {
  onCancel: () => void;
}

const OccurrenceDetails = ({ onCancel }: OccurrenceDetailsProps) => {
  const [removedLeaders, setRemovedLeaders] = useState<DomainUser[]>([]);
  const [availableLeaderOptions, setAvailableLeaderOptions] = useState<
    SelectOption[]
  >([]);

  const { selectedEvent, selectedEventOccurrence } =
    useAppSelector(selectAdminState);
  const dispatch = useAppDispatch();

  const createOccurrenceValidationSchema = useMemo(() => {
    const isNewFromCalendar =
      selectedEventOccurrence?.meta?.isNew &&
      selectedEventOccurrence?.meta?.componentOrigin === "occurrenceCalendar";

    return z.object({
      scheduleStart: z.date(),
      duration: z.number().optional(),
      leaders: z.array(z.string()).optional(),
      eventId: isNewFromCalendar
        ? z.string().min(1, { message: "Event is required" })
        : z.string().optional(),
    });
  }, [
    selectedEventOccurrence?.meta?.isNew,
    selectedEventOccurrence?.meta?.componentOrigin,
  ]);

  type CreateOccurrenceFormData = z.infer<
    typeof createOccurrenceValidationSchema
  >;

  const queryClient = useQueryClient();

  const { control, handleSubmit, reset, watch, getValues, setValue } =
    useForm<CreateOccurrenceFormData>({
      resolver: zodResolver(createOccurrenceValidationSchema),
      defaultValues: undefined,
    });

  const { data: domainLeaderResponse } = useQuery({
    queryKey: ["domainUserLeaders"],
    queryFn: () => getDomainUsers("leader"),
    staleTime: Infinity,
  });

  const time = watch("scheduleStart");
  const duration = watch("duration");

  const handleSaveSuccess = (response: AxiosResponse<Occurrence>) => {
    queryClient.setQueryData(
      ["event", response.data.eventId],
      (oldData: AxiosResponse<EventOffering>) => {
        if (!oldData) return undefined;

        return {
          ...oldData,
          data: {
            ...oldData.data,
            occurrences: [...(oldData.data.occurrences || []), response.data],
          },
        };
      }
    );
    dispatch(setSelectedEventOccurrence(response.data));
  };

  const handleUpdateSuccess = (response: AxiosResponse<Occurrence>) => {
    queryClient.setQueryData(
      ["event", response.data.eventId],
      (oldData: AxiosResponse<EventOffering>) => {
        if (!oldData || !oldData.data || !oldData.data.occurrences)
          return undefined;
        const occurrenceIndex = oldData.data.occurrences.findIndex(
          (occurrence: Occurrence) => occurrence.id === response.data.id
        );

        if (occurrenceIndex >= 0) {
          return {
            ...oldData,
            data: {
              ...oldData.data,
              occurrences: oldData.data.occurrences.map(
                (occurrence: Occurrence, idx) =>
                  idx === occurrenceIndex ? response.data : occurrence
              ),
            },
          };
        }
        return oldData;
      }
    );
  };

  const {
    refetch: getAvailableLeaders,
    data: availableLeaderResponse,
    isFetching: isLoadingAvailableLeaders,
  } = useQuery({
    queryKey: ["available-leaders", selectedEventOccurrence?.id],
    queryFn: () => {
      const isoString = time.toISOString();

      const apiDuration = duration ?? selectedEvent?.duration;

      if (!apiDuration) return;

      return getAvailableDomainUserLeaders(
        isoString,
        apiDuration,
        selectedEventOccurrence?.id ?? "",
        selectedEventOccurrence?.leaders?.map((leader) => leader.id)
      );
    },
    staleTime: 4000,
    enabled: false,
  });

  const { mutate: createOccurrenceMutation, isPending: isCreatePending } =
    useMutation({
      mutationKey: ["createMutation"],
      mutationFn: ({ occurrence }: { occurrence: CreateOccurrence }) =>
        createOccurrence(occurrence),
      onSuccess: handleSaveSuccess,
      meta: {
        successMessage: "Occurrence Created",
        errorMessage: "Failed to create occurrence",
      },
    });

  const { mutate: updateOccurrenceMutation, isPending: isUpdatePending } =
    useMutation({
      mutationKey: ["updateMutation"],
      mutationFn: ({ occurrence }: { occurrence: UpdateOccurrence }) =>
        updateOccurrence(occurrence),
      onSuccess: handleUpdateSuccess,
      meta: {
        successMessage: "Occurrence Updated",
        errorMessage: "Failed to update occurrence",
      },
    });

  useEffect(() => {
    if (selectedEventOccurrence?.meta?.isNew) {
      let defaultDate = new Date(selectedEventOccurrence.scheduleStart);
      if (defaultDate < new Date()) {
        defaultDate = new Date();
        defaultDate.setHours(10, 0, 0, 0);
      }

      reset({
        scheduleStart: defaultDate,
        duration: selectedEvent?.duration,
        leaders:
          selectedEventOccurrence?.leaders?.map((leader) => leader.id) ?? [],
      });
      setAvailableLeader();
    } else {
      let scheduleStartDate: Date;
      if (selectedEventOccurrence?.scheduleStart) {
        scheduleStartDate = new Date(selectedEventOccurrence.scheduleStart);
      } else {
        scheduleStartDate = new Date();
      }
      reset({
        scheduleStart: scheduleStartDate,
        duration: selectedEventOccurrence?.duration,
        leaders:
          selectedEventOccurrence?.leaders?.map((leader) => leader.id) ?? [],
      });
      setAvailableLeader();
    }
  }, [selectedEventOccurrence]);

  useEffect(() => {
    if (!availableLeaderResponse?.data) return;
    setAvailableLeader();
  }, [availableLeaderResponse?.data]);

  const setAvailableLeader = () => {
    let availableLeaderOptions: SelectOption[] = [];
    //get the default leader options.
    if (selectedEventOccurrence?.status.toLowerCase() === "complete") {
      const leaders: SelectOption[] =
        selectedEventOccurrence.leaders?.map((leader) => ({
          label: `${leader.firstName} ${leader.lastName}`,
          value: leader.id,
        })) ?? [];
      availableLeaderOptions = leaders;
    } else if (availableLeaderResponse?.data) {
      const leaders: SelectOption[] = availableLeaderResponse.data.map(
        (user) => ({
          label: `${user.firstName} ${user.lastName}`,
          value: user.id,
        })
      );
      availableLeaderOptions = leaders;
    } else {
      return;
    }

    const leaders = getValues("leaders") ?? [];

    // Filter out from the options array leaders who are not available
    // Also add to the availableLeaders array leader who are available
    const availableLeaders: string[] = [];
    const unavailbeLeader = leaders.filter((leader) => {
      const leaderOption = availableLeaderOptions?.find(
        (availLeader) => availLeader.value === leader
      );
      if (leaderOption) {
        availableLeaders.push(leaderOption.value.toString());
      }
      return !leaderOption;
    });

    // From the unavailable leaders array, get the domain user object and make a new array
    const unavailableLeaderObjectArray: DomainUser[] = [];
    if (unavailbeLeader.length > 0) {
      for (const leaderIdx in unavailbeLeader) {
        const domainLeader = domainLeaderResponse?.data.find(
          (dlr) => dlr.id === unavailbeLeader[leaderIdx]
        );
        if (domainLeader) {
          unavailableLeaderObjectArray.push(domainLeader);
        }
      }
    }

    setAvailableLeaderOptions(availableLeaderOptions);
    setRemovedLeaders(unavailableLeaderObjectArray);
    setValue("leaders", availableLeaders);
  };

  useEffect(() => {
    if (selectedEventOccurrence?.status.toLowerCase() === "complete") return;

    if (time && duration) {
      getAvailableLeaders();
    }
  }, [duration, time, getAvailableLeaders, selectedEventOccurrence?.status]);

  const shouldDisableDate = (date: PickerValidDate) => {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    return date < today;
  };

  const onSave = () => {
    if (
      !selectedEventOccurrence ||
      (!selectedEventOccurrence.id && !selectedEventOccurrence.meta?.isNew)
    )
      return;

    const { scheduleStart, duration, leaders } = getValues();

    if (selectedEventOccurrence?.meta?.isNew) {
      const createOccurrenceObj: CreateOccurrence = {
        eventId: selectedEventOccurrence.eventId,
        scheduleStart: scheduleStart.toISOString(),
        duration,
        leaders: leaders ?? [],
      };
      createOccurrenceMutation({ occurrence: createOccurrenceObj });
    } else {
      const updateOccurrenceObj: UpdateOccurrence = {
        eventId: selectedEventOccurrence.eventId,
        scheduleStart: scheduleStart.toISOString(),
        duration,
        leaders: leaders ?? [],
        id: selectedEventOccurrence.id,
      };

      updateOccurrenceMutation({ occurrence: updateOccurrenceObj });
    }
  };

  // const eventOptions = events.map((event) => ({
  //   label: event.eventName,
  //   value: event.id!,
  // }));

  return (
    <>
      {selectedEventOccurrence?.meta?.componentOrigin ===
        "occurrenceCalendar" && <h2>{selectedEventOccurrence.eventName}</h2>}
      <form onSubmit={handleSubmit(onSave)}>
        <div className="p-4 space-y-2 bg-white rounded shadow flex flex-col gap-6">
          {/* <FormSelect
            name="eventId"
            control={control}
            options={eventOptions}
            label={"Event"}
          /> */}

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
            label={"Duration"}
          />

          <FormSelect
            name="leaders"
            control={control}
            options={availableLeaderOptions}
            noOptionsText="No Leaders available"
            displayEmpty={true}
            label={
              selectedEventOccurrence?.status.toLowerCase() === "complete"
                ? "Assigned Leaders"
                : "Assign Leaders"
            }
            multiSelect
            isLoading={isLoadingAvailableLeaders}
          />
          {removedLeaders && removedLeaders.length > 0 && (
            <div>
              Leaders removed due to unavailability:
              {removedLeaders.map((leader, idx) => (
                <span className="pl-2">{`${leader.lastName}${idx === removedLeaders.length - 1 ? "" : ", "}`}</span>
              ))}
            </div>
          )}

          <FormFooter
            onCancel={onCancel}
            areActionsDisabled={isCreatePending || isUpdatePending}
            isLoading={isCreatePending || isUpdatePending}
            cancelButtonText={
              selectedEventOccurrence?.status.toLowerCase() === "pending"
                ? "Cancel"
                : "Back"
            }
            saveButtonText={selectedEventOccurrence?.id ? "Update" : "Create"}
          />
        </div>
      </form>
    </>
  );
};

export { OccurrenceDetails };
