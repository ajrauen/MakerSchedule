import { zodResolver } from "@hookform/resolvers/zod";
import { getAvailableDomainUserLeaders } from "@ms/api/domain-user.api";
import { FormDateTime } from "@ms/Components/FormComponents/FormDateTime/FormDateTime";
import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import { durationOptions } from "@ms/Pages/Admin/Events/utils/event.utils";
import type { EventOffering } from "@ms/types/event.types";
import type { SelectOption } from "@ms/types/form.types";
import type { Occurrence } from "@ms/types/occurrence.types";
import type { PickerValidDate } from "@mui/x-date-pickers";
import { useQuery } from "@tanstack/react-query";
import { useEffect, useMemo } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";

interface OccurenceDetailsProps {
  event: EventOffering;
  occurrence?: Occurrence;
  onCancel: () => void;
}

const createOccurrenceValidationSchema = z.object({
  scheduleStart: z.number(),
  duration: z.number().optional(),
  leaders: z.array(z.number()).optional(),
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

  const {
    getValues,
    control,
    handleSubmit,
    formState: { errors },
    reset,
    watch,
  } = useForm<CreateOccurrenceFormData>({
    resolver: zodResolver(createOccurrenceValidationSchema),
    defaultValues: creatOccurrenceFormData,
  });

  const time = watch("scheduleStart");
  const duration = watch("duration");

  const { refetch: getAvailableLeaders, data: availableLeaderResponse } =
    useQuery({
      queryKey: ["available-leaders", time, duration, event.duration],
      queryFn: () => {
        const timeMS = new Date(time).getTime();
        const utcTimeMS = timeMS - new Date().getTimezoneOffset() * 60 * 1000;

        const apiDuration = duration ?? event.duration;

        if (!apiDuration) return;

        return getAvailableDomainUserLeaders(utcTimeMS, apiDuration);
      },
      staleTime: 3000,
      enabled: false,
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
      ).getTime();

      reset({
        ...creatOccurrenceFormData,
        scheduleStart: todayAt10am,
      });
    } else {
      reset(occurrence);
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

  return (
    <div className="p-4 space-y-2 bg-white rounded shadow flex flex-col gap-6">
      <FormDateTime
        control={control}
        name="scheduleStart"
        label="Start Time"
        shouldDisableDate={shouldDisableDate}
      />
      <FormSelect name="duration" control={control} options={durationOptions} />

      <FormSelect
        name="leaders"
        control={control}
        options={leaderOptions}
        label="Availabe Leaders"
        multiSelect
        disabled={!availableLeaderResponse?.status}
        helperText={!availableLeaderResponse ? "Select Time and Duration" : ""}
      />
    </div>
  );
};

export { OccurenceDetails };
