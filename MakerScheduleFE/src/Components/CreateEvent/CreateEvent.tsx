import { getEmployeeList } from "@ms/api/employee.api";
import { useMutation, useQuery } from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import { Button } from "@mui/material";
import { createEvent } from "@ms/api/event.api";
import type { EventOffering } from "@ms/types/event.types";
import { FormDateTime } from "@ms/Components/FormComponents/FormDateTime/FormDateTime";

const createEventvalidationSchema = z.object({
  eventName: z.string().min(3),
  description: z.string().min(3),
  scheduleStart: z
    .date()
    .optional()
    .refine(
      (date) => {
        if (date) {
          return new Date(date).getTime() > Date.now();
        }
        return true;
      },
      {
        message: "Date must be in the future",
      }
    ),
  leaders: z.any(),
  duration: z.number().optional(),
});

type CreateEventFormData = z.infer<typeof createEventvalidationSchema>;

const createEventInitialFormData = {
  eventName: "",
  description: "",
  leaders: [],
};

const CreateEvent = () => {
  const durationOptions = [
    // 15-minute intervals: 15, 30, ..., 120
    ...Array.from({ length: 8 }, (_, i) => {
      const minutes = (i + 1) * 15;
      const hours = Math.floor(minutes / 60);
      const mins = minutes % 60;
      let label = "";
      if (hours > 0) {
        label += `${hours} hour${hours > 1 ? "s" : ""}`;
        if (mins > 0) label += ` ${mins} minutes`;
      } else {
        label = `${mins} minutes`;
      }
      return {
        label,
        value: minutes * 60 * 1000,
      };
    }),
    // 30-minute intervals: 150, 180, 210, 240
    ...Array.from({ length: 4 }, (_, i) => {
      const minutes = 120 + (i + 1) * 30;
      const hours = Math.floor(minutes / 60);
      const mins = minutes % 60;
      let label = "";
      if (hours > 0) {
        label += `${hours} hour${hours > 1 ? "s" : ""}`;
        if (mins > 0) label += ` ${mins} minutes`;
      } else {
        label = `${mins} minutes`;
      }
      return {
        label,
        value: minutes * 60 * 1000,
      };
    }),
  ];

  const { getValues, control, handleSubmit } = useForm<CreateEventFormData>({
    resolver: zodResolver(createEventvalidationSchema),
    defaultValues: createEventInitialFormData,
  });

  const { data: employeeListData } = useQuery({
    queryKey: ["employeeList"],
    queryFn: getEmployeeList,
  });

  const { mutate: saveEvent } = useMutation({
    mutationKey: ["createClient"],
    mutationFn: createEvent,
  });

  const handleSave = () => {
    const { description, eventName, leaders, scheduleStart, duration } =
      getValues();
    const startDate = scheduleStart?.getTime();

    const eventOffering: EventOffering = {
      description,
      eventName,
      leaders: [leaders],
      scheduleStart: startDate,
      duration: duration,
      eventType: 1,
    };

    saveEvent(eventOffering);
  };

  return (
    <div>
      <div>
        <FormTextField name="eventName" label="Name" control={control} />
        <FormTextField
          name="description"
          label="Description"
          control={control}
        />
        <FormSelect
          name="leaders"
          control={control}
          options={
            employeeListData?.data?.map((e) => ({
              label: e.firstName + " " + e.lastName,
              value: e.id,
              ...e,
            })) ?? []
          }
        />
        <FormDateTime control={control} name="scheduleStart" />
        <FormSelect
          name="duration"
          control={control}
          options={durationOptions}
        />
      </div>
      <div>
        <Button onClick={handleSubmit(handleSave)}>Save</Button>
      </div>
    </div>
  );
};

export { CreateEvent };
