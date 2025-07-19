import {
  MenuItem,
  Select,
  FormControl,
  InputLabel,
  FormHelperText,
  type SelectProps,
  Chip,
  Box,
} from "@mui/material";
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

type FormSelectProps<T extends FieldValues, C> = Omit<
  SelectProps,
  "multiple"
> & {
  name: Path<T>;
  control: Control<T>;
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
    <Controller
      name={name}
      control={control}
      render={({ field, fieldState }) => (
        <FormControl fullWidth error={!!fieldState.error}>
          <InputLabel id={`${name}-label`}>{label || "Select"}</InputLabel>
          <Select
            labelId={`${name}-label`}
            multiple={multiSelect}
            {...field}
            value={
              multiSelect
                ? field.value || []
                : field.value === undefined
                  ? ""
                  : field.value
            }
            label={label || "Select"}
            renderValue={
              multiSelect
                ? (selected) => (
                    <div className="flex flex-wrap gap-0.5">
                      {(selected as (string | number)[]).map((value) => {
                        const option = options.find(
                          (opt) => opt.value === value
                        );
                        return (
                          <Chip
                            key={value}
                            label={option?.label || value}
                            size="small"
                            onMouseDown={(e) => {
                              e.stopPropagation();
                              e.preventDefault();
                            }}
                            onDelete={() => {
                              const newValue = (
                                selected as (string | number)[]
                              ).filter((item) => item !== value);
                              field.onChange(newValue);
                            }}
                          />
                        );
                      })}
                    </div>
                  )
                : undefined
            }
            {...props}
          >
            {options.map((option) => (
              <MenuItem key={option.value} value={option.value}>
                {option.label}
              </MenuItem>
            ))}
          </Select>
          {(fieldState.error || helperText) && (
            <FormHelperText>
              {fieldState.error?.message || helperText}
            </FormHelperText>
          )}
        </FormControl>
      )}
    />
  );
};

export { FormSelect };
