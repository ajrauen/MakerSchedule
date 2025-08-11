import { FormControl, FormHelperText, type SelectProps } from "@mui/material";
import {
  Controller,
  type Control,
  type FieldValues,
  type Path,
} from "react-hook-form";
import Select from "@ms/Components/FormComponents/FormSelect/Select/Select";

export type FormSelectOption = {
  label: string;
  value: string | number;
};

type FormSelectProps<T extends FieldValues, C> = Omit<
  SelectProps,
  "multiple"
> & {
  name?: Path<T> | string;
  control?: Control<T>;
  options: (C & FormSelectOption)[];
  multiSelect?: boolean;
  label?: string;
  helperText?: string;
  isLoading?: boolean;
  noOptionsText?: string;
  formControlClassName?: string;
};

const FormSelect = <T extends FieldValues, C>({
  name,
  control,
  options,
  multiSelect = false,
  label,
  helperText,
  isLoading,
  noOptionsText,
  formControlClassName,

  ...props
}: FormSelectProps<T, C>) => {
  return control && name ? (
    <Controller
      name={name as Path<T>}
      control={control}
      render={({ field, fieldState }) => (
        <FormControl
          fullWidth
          error={!!fieldState.error}
          className={formControlClassName}
        >
          <Select
            options={options}
            multiSelect={multiSelect}
            label={label}
            isLoading={isLoading}
            error={!!fieldState.error?.message}
            noOptionsText={noOptionsText}
            {...field}
            {...props}
            value={
              multiSelect
                ? field.value || []
                : field.value === undefined
                  ? ""
                  : field.value
            }
            onChange={(e, child) => {
              if (field.onChange) field.onChange(e);
              if (props.onChange) props.onChange(e, child);
            }}
            onDelete={(value) => {
              if (field.onChange) {
                const newValue = multiSelect
                  ? field.value.filter((v: string | number) => v !== value)
                  : "";
                field.onChange(newValue);
              }
            }}
          />
          {(fieldState.error?.message || helperText) && (
            <FormHelperText>
              {fieldState.error?.message ?? helperText}
            </FormHelperText>
          )}
        </FormControl>
      )}
    />
  ) : (
    <FormControl
      fullWidth
      error={!!(control && name ? undefined : props.error)}
      className={formControlClassName}
    >
      <Select
        name={name}
        options={options}
        multiSelect={multiSelect}
        label={label}
        helperText={helperText}
        isLoading={isLoading}
        noOptionsText={noOptionsText}
        {...props}
        error={!!helperText}
      />
    </FormControl>
  );
};

export { FormSelect };
