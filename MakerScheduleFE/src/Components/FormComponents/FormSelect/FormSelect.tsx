import { FormControl, FormHelperText, type SelectProps } from "@mui/material";
import PlainSelect from "./PlainSelect";
import {
  Controller,
  type Control,
  type FieldValues,
  type Path,
} from "react-hook-form";

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
};

const FormSelect = <T extends FieldValues, C>({
  name,
  control,
  options,
  multiSelect = false,
  label,
  helperText,
  ...props
}: FormSelectProps<T, C>) => {
  return (
    <FormControl
      fullWidth
      error={!!(control && name ? undefined : props.error)}
    >
      {control && name ? (
        <Controller
          name={name as Path<T>}
          control={control}
          render={({ field, fieldState }) => (
            <>
              <PlainSelect
                options={options}
                multiSelect={multiSelect}
                label={label}
                helperText={fieldState.error?.message || helperText}
                {...field}
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
                {...props}
              />
            </>
          )}
        />
      ) : (
        <>
          <PlainSelect
            name={name}
            options={options}
            multiSelect={multiSelect}
            label={label}
            helperText={helperText}
            {...props}
          />
          {helperText && <FormHelperText>{helperText}</FormHelperText>}
        </>
      )}
    </FormControl>
  );
};

export { FormSelect };
