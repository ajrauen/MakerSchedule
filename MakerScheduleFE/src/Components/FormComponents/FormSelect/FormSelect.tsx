import { MenuItem, TextField, type TextFieldProps } from "@mui/material";
import {
  Controller,
  type Control,
  type FieldValues,
  type Path,
} from "react-hook-form";

type FormSelectOption = {
  label: string;
  value: string | number;
};

type FormSelectProps<T extends FieldValues, C> = TextFieldProps & {
  name: Path<T>;
  control: Control<T>;
  options: (C & FormSelectOption)[];
};

const FormSelect = <T extends FieldValues, C>({
  name,
  control,
  options,
  ...props
}: FormSelectProps<T, C>) => {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field, fieldState }) => (
        <TextField
          id="outlined-select-currency"
          select
          label="Select"
          {...props}
          {...field}
          error={!!fieldState.error}
          helperText={fieldState.error?.message}
        >
          {options.map((option) => (
            <MenuItem key={option.value} value={option.value}>
              {option.label}
            </MenuItem>
          ))}
        </TextField>
      )}
    />
  );
};

export { FormSelect };
