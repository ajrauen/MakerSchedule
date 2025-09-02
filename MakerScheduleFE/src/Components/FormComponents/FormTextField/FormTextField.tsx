import {
  Controller,
  type Control,
  type FieldValues,
  type Path,
} from "react-hook-form";
import { TextField, type TextFieldProps } from "@mui/material";

type FormTextFieldProps<T extends FieldValues> = TextFieldProps & {
  name: Path<T>;
  control: Control<T>;
  restrict?: RegExp;
};

const FormTextField = <T extends FieldValues>({
  name,
  control,
  ...props
}: FormTextFieldProps<T>) => (
  <Controller
    name={name}
    control={control}
    render={({ field, fieldState }) => (
      <TextField
        {...field}
        {...props}
        onChange={(e) => {
          if (props.restrict) {
            const value = e.target.value;
            if (!props.restrict.test(value)) {
              e.preventDefault();
              return;
            }
          }
          field.onChange(e);
        }}
        error={!!fieldState.error}
        helperText={fieldState.error?.message}
      />
    )}
  />
);

export default FormTextField;
