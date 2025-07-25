import {
  MenuItem,
  Select as MuiSelect,
  FormControl,
  InputLabel,
  FormHelperText,
  type SelectProps as MuiSelectProps,
  Chip,
  CircularProgress,
} from "@mui/material";
import React from "react";
// No-op icon for hiding dropdown arrow
const NoDropdownIcon: React.FC = () => null;
import { type FormSelectOption } from "../FormSelect";

export type SelectProps = MuiSelectProps & {
  options: FormSelectOption[];
  multiSelect?: boolean;
  label?: string;
  helperText?: string;
  onDelete?: (value: string | number) => void;
  isLoading?: boolean;
};

const Select = ({
  options,
  multiSelect = false,
  label,
  helperText,
  onDelete,
  isLoading,
  disabled,
  ...props
}: SelectProps) => {
  return (
    <FormControl fullWidth disabled={disabled || isLoading}>
      <InputLabel id={props.name ? `${props.name}-label` : undefined}>
        {label || "Select"}
      </InputLabel>
      <MuiSelect
        labelId={props.name ? `${props.name}-label` : undefined}
        multiple={multiSelect}
        label={label || "Select"}
        disabled={disabled || isLoading}
        IconComponent={isLoading ? NoDropdownIcon : undefined}
        renderValue={
          isLoading
            ? () => (
                <div className="flex items-center gap-1">
                  Loading... <CircularProgress className="ml-auto" size={24} />
                </div>
              )
            : multiSelect
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
                          onDelete={
                            onDelete ? () => onDelete(value) : undefined
                          }
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
      </MuiSelect>
      {helperText && <FormHelperText>{helperText}</FormHelperText>}
    </FormControl>
  );
};

export default Select;
