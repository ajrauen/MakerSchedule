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
import { Button } from "@mui/material";
import type { PickerValidDate } from "@mui/x-date-pickers";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useMemo, useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { toast } from "react-toastify";
import type { DomainUser } from "@ms/types/domain-user.types";

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
  const [removedLeaders, setRemovedLeaders] = useState<DomainUser[]>([]);
  const [availableLeaderOptions, setAvailableLeaderOptions] = useState<
    SelectOption[]
  >([]);

  const creatOccurrenceFormData = {
    duration: undefined,
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

  const { refetch: getAvailableLeaders, data: availableLeaderResponse } =
    useQuery({
      queryKey: ["available-leaders", selectedEvent.id],
      queryFn: () => {
        const isoString = time.toISOString();

        const apiDuration = duration ?? selectedEvent.duration;

        if (!apiDuration) return;

        return getAvailableDomainUserLeaders(
          isoString,
          apiDuration,
          occurrence?.leaders?.map((leader) => leader.id)
        );
      },
      staleTime: 10000,
      enabled: false,
    });

  const { mutate: createOccurrenceMutation } = useMutation({
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

  const { mutate: updateOccurrenceMutation } = useMutation({
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
      reset({
        ...creatOccurrenceFormData,
        scheduleStart: undefined,
      });
    } else {
      let scheduleStartDate: Date;
      if (occurrence?.scheduleStart) {
        scheduleStartDate = new Date(occurrence.scheduleStart);
      } else {
        scheduleStartDate = new Date();
      }

      const processedLeaders =
        occurrence?.leaders?.map((leader) => leader.id) ?? [];

      reset({
        scheduleStart: scheduleStartDate,
        duration: occurrence?.duration,
        leaders: processedLeaders,
      });
    }
  }, [occurrence]);

  useEffect(() => {
    if (occurrence?.status.toLowerCase() === "complete") return;

    if (time && duration) {
      getAvailableLeaders();
    }
  }, [duration, time]);

  useEffect(() => {
    if (occurrence?.status.toLowerCase() === "complete") {
      const leaders: SelectOption[] =
        occurrence.leaders?.map((leader) => ({
          label: `${leader.firstName} ${leader.lastName}`,
          value: leader.id,
        })) ?? [];
      setAvailableLeaderOptions(leaders);
      return;
    }

    if (availableLeaderResponse?.data) {
      const leaders: SelectOption[] = availableLeaderResponse.data.map(
        (user) => ({
          label: `${user.firstName} ${user.lastName}`,
          value: user.id,
        })
      );
      setAvailableLeaderOptions(leaders);
    }
  }, [availableLeaderResponse?.data, occurrence, duration, time]);

  useEffect(() => {
    const leaders = getValues("leaders") ?? [];

    const availableLeader: string[] = [];
    const unavailbeLeader = leaders.filter((leader) => {
      const leaderOption = availableLeaderOptions?.find(
        (availLeader) => availLeader.value === leader
      );
      if (leaderOption) {
        availableLeader.push(leaderOption.value.toString());
      }
      return !leaderOption;
    });

    if (unavailbeLeader.length > 0) {
      setValue("leaders", availableLeader);

      let unavailableLeaderObjectArray: DomainUser[] = [];
      for (const leaderIdx in unavailbeLeader) {
        const domainLeader = domainLeaderResponse?.data.find(
          (dlr) => dlr.id === unavailbeLeader[leaderIdx]
        );
        if (domainLeader) {
          unavailableLeaderObjectArray.push(domainLeader);
        }
      }

      setRemovedLeaders(unavailableLeaderObjectArray);
    }
  }, [availableLeaderOptions, occurrence]);

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

  const isFormItemDisable =
    occurrence?.status?.toLocaleLowerCase() === "complete" ||
    occurrence?.status?.toLocaleLowerCase() === "canceled";

  return (
    <form onSubmit={handleSubmit(onSave)}>
      <div className="p-4 space-y-2 bg-white rounded shadow flex flex-col gap-6">
        <div>Status : {occurrence?.status}</div>

        <FormDateTime
          control={control}
          name="scheduleStart"
          label="Start Time"
          shouldDisableDate={shouldDisableDate}
          disabled={isFormItemDisable}
        />
        <FormSelect
          name="duration"
          control={control}
          options={durationOptions}
          label={"Duration"}
          disabled={isFormItemDisable}
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
          disabled={!availableLeaderResponse?.status || isFormItemDisable}
          helperText={
            !availableLeaderResponse ? "Select Time and Duration" : ""
          }
        />
        {removedLeaders && removedLeaders.length > 0 && (
          <div>Removed {removedLeaders.map((leader) => leader.lastName)}</div>
        )}

        <div className="flex gap-4">
          <Button
            className=" px-4 py-2 bg-gray-200 rounded hover:bg-gray-300 !ml-auto"
            onClick={onCancel}
          >
            {occurrence?.status.toLowerCase() === "pending" ? "Cancel" : "Back"}
          </Button>
          {occurrence?.status.toLowerCase() === "pending" && (
            <Button
              variant="contained"
              type="submit"
              className=" px-4 py-2 bg-gray-200 rounded hover:bg-gray-300 "
            >
              {occurrence?.id ? "Update" : "Save"}
            </Button>
          )}
        </div>
      </div>
    </form>
  );
};

export { OccurenceDetails };
