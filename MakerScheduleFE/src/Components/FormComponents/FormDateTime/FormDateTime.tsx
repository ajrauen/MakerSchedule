import {
  Controller,
  type Control,
  type FieldValues,
  type Path,
} from "react-hook-form";
import { type TextFieldProps } from "@mui/material";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { DateTimePicker } from "@mui/x-date-pickers/DateTimePicker";
import type { PickerValidDate, TimeView } from "@mui/x-date-pickers";

interface FormDateTimeProps<T extends FieldValues>
  extends Omit<TextFieldProps, "name" | "value" | "onChange"> {
  name: Path<T>;
  control: Control<T>;
  label?: string;
  shouldDisableDate?: ((day: PickerValidDate) => boolean) | undefined;
  shouldDisableTime?:
    | ((value: PickerValidDate, view: TimeView) => boolean)
    | undefined;
}

const FormDateTime = <T extends FieldValues>({
  name,
  control,
  label,
  shouldDisableDate,
  shouldDisableTime,
  ...props
}: FormDateTimeProps<T>) => {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field, fieldState }) => (
        <LocalizationProvider dateAdapter={AdapterDateFns}>
          <DateTimePicker
            label={label}
            value={field.value}
            slotProps={{
              textField: {
                ...props,
                error: !!fieldState.error,
                helperText: fieldState.error?.message,
              },
            }}
            shouldDisableDate={shouldDisableDate}
            shouldDisableTime={shouldDisableTime}
            onAccept={field.onChange}
          />
        </LocalizationProvider>
      )}
    />
  );
};

export { FormDateTime };
