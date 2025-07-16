import { useForm } from "react-hook-form";
import type { Occurence } from "@ms/types/occurence.types";
import { useEffect } from "react";
import { FormDateTime } from "@ms/Components/FormComponents/FormDateTime/FormDateTime";

interface OccurenceDetailsProps {
  occurence?: Occurence;
  onSubmit?: (data: Occurence) => void;
}

const defaultOccurenceData = {
  defaultValues: {
    eventId: 0,
    scheduleStart: Date.now(),
    attendees: [],
    leaders: [],
  },
};

const OccurenceDetails = ({ occurence, onSubmit }: OccurenceDetailsProps) => {
  const { handleSubmit, setValue, control } = useForm<Occurence>({
    defaultValues: defaultOccurenceData,
  });

  // Set scheduleStart as ISO string for input value
  useEffect(() => {
    setValue(
      "scheduleStart",
      new Date(occurence?.scheduleStart ?? Date.now())
        .toISOString()
        .slice(0, 16) as any
    );
  }, [occurence?.scheduleStart, setValue]);

  const submitHandler = (data: any) => {
    // Convert scheduleStart back to ms and duration to ms
    const updated: Occurence = {
      ...occurence,
      ...data,
      scheduleStart: new Date(data.scheduleStart).getTime(),
      duration: data.duration ? Number(data.duration) * 60000 : undefined,
    };
    onSubmit?.(updated);
  };

  return (
    <form onSubmit={handleSubmit(submitHandler)} className="space-y-4">
      <FormDateTime
        name="scheduleStart"
        control={control}
        label="Start Time"
        fullWidth
      />

      <button
        type="submit"
        className="bg-blue-500 text-white px-4 py-2 rounded"
      >
        Save
      </button>
    </form>
  );
};

export { OccurenceDetails };
