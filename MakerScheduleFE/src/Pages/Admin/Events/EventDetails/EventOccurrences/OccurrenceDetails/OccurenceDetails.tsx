import { zodResolver } from "@hookform/resolvers/zod";
import {
  getAvailableDomainUserLeaders,
  getDomainUsers,
} from "@ms/api/domain-user.api";
import { createOccurrence } from "@ms/api/occurrence.api";
import { FormDateTime } from "@ms/Components/FormComponents/FormDateTime/FormDateTime";
import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import { durationOptions } from "@ms/Pages/Admin/Events/utils/event.utils";
import type { EventOffering } from "@ms/types/event.types";
import type { SelectOption } from "@ms/types/form.types";
import type { CreateOccurrence, Occurrence } from "@ms/types/occurrence.types";
import { Button } from "@mui/material";
import type { PickerValidDate } from "@mui/x-date-pickers";
import { useMutation, useQuery } from "@tanstack/react-query";
import { useEffect, useMemo } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";

interface OccurenceDetailsProps {
  event: EventOffering;
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
  event,
  onCancel,
}: OccurenceDetailsProps) => {
  const creatOccurrenceFormData = {
    duration: undefined,
    leaders: [],
  };

  const { control, handleSubmit, reset, watch, getValues } =
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
      queryKey: ["available-leaders", time, duration, event.duration],
      queryFn: () => {
        const isoString = time.toISOString();

        const apiDuration = duration ?? event.duration;

        if (!apiDuration) return;

        return getAvailableDomainUserLeaders(
          isoString,
          apiDuration,
          occurrence?.leaders
        );
      },
      staleTime: 3000,
      enabled: false,
    });

  const { mutate: createOccurrenceMutation } = useMutation({
    mutationKey: ["createMutation"],
    mutationFn: ({ occurrence }: { occurrence: CreateOccurrence }) =>
      createOccurrence(occurrence),
  });

  useEffect(() => {
    if (occurrence?.meta?.isNew) {
      const now = new Date();
      const todayAt10am = new Date(
        now.getFullYear(),
        now.getMonth(),
        now.getDate(),
        10,
        0,
        0,
        0
      );
      reset({
        ...creatOccurrenceFormData,
        scheduleStart: todayAt10am,
      });
    } else {
      let scheduleStartDate: Date;
      if (occurrence?.scheduleStart) {
        const isoString = occurrence.scheduleStart;

        if (isoString.endsWith("Z")) {
          const utcDate = new Date(isoString);
          scheduleStartDate = new Date(
            utcDate.getTime() - utcDate.getTimezoneOffset() * 60000
          );
        } else {
          scheduleStartDate = new Date(isoString);
        }
      } else {
        scheduleStartDate = new Date();
      }

      const processedLeaders = occurrence?.leaders ?? [];

      reset({
        scheduleStart: scheduleStartDate,
        duration: occurrence?.duration,
        leaders: processedLeaders,
      });
    }
  }, [occurrence]);

  useEffect(() => {
    if (time && duration) {
      getAvailableLeaders();
    }
  }, [duration, time]);

  const leaderOptions = useMemo(() => {
    let options: SelectOption[] = [];
    if (availableLeaderResponse?.data) {
      options = availableLeaderResponse.data.map((user) => ({
        label: `${user.firstName} ${user.lastName}`,
        value: user.id,
      }));
    }
    return options;
  }, [availableLeaderResponse?.data]);

  const shouldDisableDate = (date: PickerValidDate) => {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    return date < today;
  };

  const onSave = () => {
    if (!event.id) return;

    const { scheduleStart, duration, leaders } = getValues();

    const occurrence: CreateOccurrence = {
      eventId: event.id,
      scheduleStart: scheduleStart.toISOString(),
      duration,
      leaders,
    };

    createOccurrenceMutation({ occurrence });
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
        />

        <FormSelect
          name="leaders"
          control={control}
          options={leaderOptions}
          label="Availabe Leaders"
          multiSelect
          disabled={!availableLeaderResponse?.status}
          helperText={
            !availableLeaderResponse ? "Select Time and Duration" : ""
          }
        />
        <div className="flex gap-4">
          <Button
            className=" px-4 py-2 bg-gray-200 rounded hover:bg-gray-300 !ml-auto"
            onClick={onCancel}
          >
            Cancel
          </Button>

          <Button
            variant="contained"
            type="submit"
            className=" px-4 py-2 bg-gray-200 rounded hover:bg-gray-300 "
          >
            {occurrence?.id ? "Update" : "Save"}
          </Button>
        </div>
      </div>
    </form>
  );
};

export { OccurenceDetails };
