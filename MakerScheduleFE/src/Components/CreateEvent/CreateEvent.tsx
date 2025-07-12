import { getEmployeeList } from "@ms/api/employee.api";
import { useMutation, useQuery } from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import FormTextField from "@ms/Components/FormComponents/FormTextField/FormTextField";
import { FormSelect } from "@ms/Components/FormComponents/FormSelect/FormSelect";
import { createEvent } from "@ms/api/event.api";
import type { CreateEventOffering, EventOffering } from "@ms/types/event.types";
import { FormDateTime } from "@ms/Components/FormComponents/FormDateTime/FormDateTime";
import { FormDialog } from "@ms/Components/FormComponents/FormDialog";
import { ImageUpload } from "@ms/Components/CreateEvent/ImageUpload/ImageUpload";
import { createSaveForm } from "@ms/Components/CreateEvent/create-event.utils";

const createEventvalidationSchema = z.object({
  eventName: z
    .string()
    .min(3, { error: "Event name must be at least 3 characters" }),
  description: z
    .string()
    .min(3, { error: "Description must be at least 3 characters" }),
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
        error: "Date must be in the future",
      }
    ),
  leaders: z.any(),
  duration: z.number().optional(),
  imageFile: z.instanceof(File, { error: "Event Image is required" }),
});

type CreateEventFormData = z.infer<typeof createEventvalidationSchema>;

const createEventInitialFormData = {
  eventName: "",
  description: "",
  leaders: [],
};

interface CreateFormProps {
  isOpen: boolean;
  onClose: () => void;
}

const CreateEvent = ({ isOpen, onClose }: CreateFormProps) => {
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

  const {
    getValues,
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<CreateEventFormData>({
    resolver: zodResolver(createEventvalidationSchema),
    defaultValues: createEventInitialFormData,
  });

  const handleOnClose = () => {
    reset(createEventInitialFormData);
    onClose();
  };

  const { data: employeeListData } = useQuery({
    queryKey: ["employeeList"],
    queryFn: getEmployeeList,
  });

  const { mutate: saveEvent } = useMutation({
    mutationKey: ["createClient"],
    mutationFn: createEvent,
    onSuccess: handleOnClose,
  });

  const handleSave = () => {
    const {
      description,
      eventName,
      leaders,
      scheduleStart,
      duration,
      imageFile,
    } = getValues();
    const startDate = scheduleStart?.getTime();

    const eventOffering: CreateEventOffering = {
      description,
      eventName,
      leaders: [leaders],
      scheduleStart: startDate,
      duration: duration,
      eventType: 1,
      imageFile,
    };
    const formEvent = createSaveForm(eventOffering);
    saveEvent(formEvent);
  };

  return (
    <FormDialog
      open={isOpen}
      onClose={handleOnClose}
      onSubmit={handleSubmit(handleSave)}
      maxWidth="lg"
      title="Create Event"
    >
      <div className="flex flex-col gap-3">
        <div className="flex flex-row gap-3">
          <div className="flex flex-col gap-3">
            <FormTextField name="eventName" label="Name" control={control} />
            <FormTextField
              name="description"
              label="Description"
              control={control}
            />
            <FormSelect
              name="leaders"
              control={control}
              label="Leander/s"
              options={
                employeeListData?.data?.map((e) => ({
                  label: e.firstName + " " + e.lastName,
                  value: e.id,
                  ...e,
                })) ?? []
              }
            />
          </div>
          <div className="flex flex-col gap-3">
            <FormDateTime control={control} name="scheduleStart" />
            <FormSelect
              name="duration"
              control={control}
              options={durationOptions}
            />
          </div>
        </div>
        <div>
          <ImageUpload
            control={control}
            name="imageFile"
            error={errors.imageFile?.message}
          />
        </div>
      </div>
    </FormDialog>
  );
};

export { CreateEvent };
