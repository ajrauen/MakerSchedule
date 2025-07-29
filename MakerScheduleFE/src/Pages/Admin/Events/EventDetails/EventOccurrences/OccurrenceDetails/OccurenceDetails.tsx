import { zodResolver } from "@hookform/resolvers/zod";
import {
  getAvailableDomainUserLeaders,
  getDomainUsers,
} from "@ms/api/domain-user.api";
import { createOccurrence, updateOccurrence } from "@ms/api/occurrence.api";
import { FormDateTime } from "@ms/Components/FormComponents/FormDateTime/FormDateTime";
import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import { durationOptions } from "@ms/Pages/Admin/Events/utils/event.utils";
import type { EventOffering } from "@ms/types/event.types";
import type { SelectOption } from "@ms/types/form.types";
import type {
  CreateOccurrence,
  Occurrence,
  UpdateOccurrence,
} from "@ms/types/occurrence.types";
import type { PickerValidDate } from "@mui/x-date-pickers";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { toast } from "react-toastify";
import type { DomainUser } from "@ms/types/domain-user.types";
import { FormFooter } from "@ms/Components/FormComponents/FormFooter/FormFooter";

interface OccurenceDetailsProps {
  selectedEvent: EventOffering;
  occurrence?: Occurrence;
  onCancel: () => void;
}

const createOccurrenceValidationSchema = z.object({
  scheduleStart: z.date(),
  duration: z.number().optional(),
  leaders: z.array(z.string()).optional(),
});

type CreateOccurrenceFormData = z.infer<
  typeof createOccurrenceValidationSchema
>;

const OccurenceDetails = ({
  occurrence,
  selectedEvent,
  onCancel,
}: OccurenceDetailsProps) => {
  const [isFormDataSet, setIsFormDataSet] = useState(false);
  const [removedLeaders, setRemovedLeaders] = useState<DomainUser[]>([]);
  const [availableLeaderOptions, setAvailableLeaderOptions] = useState<
    SelectOption[]
  >([]);

  const creatOccurrenceFormData = {
    duration: selectedEvent.duration,
    leaders: [],
  };

  const queryClient = useQueryClient();

  const { control, handleSubmit, reset, watch, getValues, setValue } =
    useForm<CreateOccurrenceFormData>({
      resolver: zodResolver(createOccurrenceValidationSchema),
      defaultValues: creatOccurrenceFormData,
    });

  const { data: domainLeaderResponse } = useQuery({
    queryKey: ["domainUserLeaders"],
    queryFn: () => getDomainUsers("leader"),
    staleTime: Infinity,
  });

  const time = watch("scheduleStart");
  const duration = watch("duration");

  const {
    refetch: getAvailableLeaders,
    data: availableLeaderResponse,
    isFetching: isLoadingAvailableLeaders,
  } = useQuery({
    queryKey: ["available-leaders", selectedEvent.id],
    queryFn: () => {
      const isoString = time.toISOString();

      const apiDuration = duration ?? selectedEvent.duration;

      if (!apiDuration) return;

      return getAvailableDomainUserLeaders(
        isoString,
        apiDuration,
        occurrence?.id ?? "",
        occurrence?.leaders?.map((leader) => leader.id)
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
      onSuccess: () => {
        queryClient.invalidateQueries({
          queryKey: ["event", selectedEvent?.id],
        });
        toast("Occurrence Created Successfully");
        onCancel();
      },
    });

  const { mutate: updateOccurrenceMutation, isPending: isUpdatePending } =
    useMutation({
      mutationKey: ["updateMutation"],
      mutationFn: ({ occurrence }: { occurrence: UpdateOccurrence }) =>
        updateOccurrence(occurrence),
      onSuccess: () => {
        queryClient.invalidateQueries({
          queryKey: ["event", selectedEvent?.id],
        });
        toast("Occurrence Saved Successfully");
      },
    });

  useEffect(() => {
    if (occurrence?.meta?.isNew) {
      let defaultDate = new Date(occurrence.scheduleStart);
      if (defaultDate < new Date()) {
        defaultDate = new Date();
        defaultDate.setHours(10, 0, 0, 0);
      }

      reset({
        ...creatOccurrenceFormData,
        scheduleStart: defaultDate,
        leaders: occurrence?.leaders?.map((leader) => leader.id) ?? [],
      });
      setIsFormDataSet(true);
    } else {
      let scheduleStartDate: Date;
      if (occurrence?.scheduleStart) {
        scheduleStartDate = new Date(occurrence.scheduleStart);
      } else {
        scheduleStartDate = new Date();
      }
      reset({
        scheduleStart: scheduleStartDate,
        duration: occurrence?.duration,
        leaders: occurrence?.leaders?.map((leader) => leader.id) ?? [],
      });
      setIsFormDataSet(true);
    }
  }, [occurrence]);

  useEffect(() => {
    if (!availableLeaderResponse?.data || !isFormDataSet) return;

    let availableLeaderOptions: SelectOption[] = [];
    //get the default leader options.
    if (occurrence?.status.toLowerCase() === "complete") {
      const leaders: SelectOption[] =
        occurrence.leaders?.map((leader) => ({
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
    let unavailableLeaderObjectArray: DomainUser[] = [];
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
  }, [availableLeaderResponse?.data, isFormDataSet]);

  useEffect(() => {
    if (occurrence?.status.toLowerCase() === "complete") return;

    if (time && duration) {
      getAvailableLeaders();
    }
  }, [duration, time]);

  const shouldDisableDate = (date: PickerValidDate) => {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    return date < today;
  };

  const onSave = () => {
    if (!selectedEvent.id || !occurrence) return;

    const { scheduleStart, duration, leaders } = getValues();

    if (occurrence?.meta?.isNew) {
      const createOccurrenceObj: CreateOccurrence = {
        eventId: selectedEvent.id,
        scheduleStart: scheduleStart.toISOString(),
        duration,
        leaders: leaders ?? [],
      };
      createOccurrenceMutation({ occurrence: createOccurrenceObj });
    } else {
      const updateOccurrenceObj: UpdateOccurrence = {
        eventId: selectedEvent.id,
        scheduleStart: scheduleStart.toISOString(),
        duration,
        leaders: leaders ?? [],
        id: occurrence.id,
      };

      updateOccurrenceMutation({ occurrence: updateOccurrenceObj });
    }
  };

  return (
    <form onSubmit={handleSubmit(onSave)}>
      <div className="p-4 space-y-2 bg-white rounded shadow flex flex-col gap-6">
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
          label={
            occurrence?.status.toLowerCase() === "complete"
              ? "Assigned Leanders"
              : "Assign Leaders"
          }
          multiSelect
          helperText={
            !availableLeaderResponse ? "Select Time and Duration" : ""
          }
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
          handleOnClose={onCancel}
          areActionsDisabled={isCreatePending || isUpdatePending}
          isLoading={isCreatePending || isUpdatePending}
          cancelButtonText={
            occurrence?.status.toLowerCase() === "pending" ? "Cancel" : "Back"
          }
          saveButtonText={occurrence?.id ? "Update" : "Save"}
        />
      </div>
    </form>
  );
};

export { OccurenceDetails };
