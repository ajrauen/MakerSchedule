import { Controller, type Control } from "react-hook-form";
import { TextField, type TextFieldProps } from "@mui/material";

type FormTextFieldProps = TextFieldProps & {
  name: string;
  control: Control<any>;
};

const FormTextField = ({ name, control, ...props }: FormTextFieldProps) => (
  <Controller
    name={name}
    control={control}
    render={({ field, fieldState }) => (
      <TextField
        {...field}
        {...props}
        error={!!fieldState.error}
        helperText={fieldState.error?.message}
      />
    )}
  />
);

export default FormTextField;
