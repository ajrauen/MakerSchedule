import {
  MenuItem,
  Select,
  FormControl,
  InputLabel,
  FormHelperText,
  type SelectProps,
  Chip,
} from "@mui/material";
import { type FormSelectOption } from "./FormSelect";

export type PlainSelectProps = SelectProps & {
  options: FormSelectOption[];
  multiSelect?: boolean;
  label?: string;
  helperText?: string;
};

const PlainSelect = ({
  options,
  multiSelect = false,
  label,
  helperText,
  ...props
}: PlainSelectProps) => {
  return (
    <FormControl fullWidth>
      <InputLabel id={props.name ? `${props.name}-label` : undefined}>
        {label || "Select"}
      </InputLabel>
      <Select
        labelId={props.name ? `${props.name}-label` : undefined}
        multiple={multiSelect}
        label={label || "Select"}
        renderValue={
          multiSelect
            ? (selected) => (
                <div className="flex flex-wrap gap-0.5">
                  {(selected as (string | number)[]).map((value) => {
                    const option = options.find((opt) => opt.value === value);
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
                          // No field.onChange, so must be handled by parent
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
      {helperText && <FormHelperText>{helperText}</FormHelperText>}
    </FormControl>
  );
};

export default PlainSelect;
