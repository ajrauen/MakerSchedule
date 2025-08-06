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
import React, { useEffect } from "react";
// No-op icon for hiding dropdown arrow
const NoDropdownIcon: React.FC = () => null;
import { type FormSelectOption } from "../FormSelect";
import { no } from "zod/v4/locales";

export type SelectProps = MuiSelectProps & {
  options: FormSelectOption[];
  multiSelect?: boolean;
  label?: string;
  helperText?: string;
  onDelete?: (value: string | number) => void;
  isLoading?: boolean;
  noOptionsText?: string;
};

const Select = ({
  options,
  multiSelect = false,
  label,
  helperText,
  onDelete,
  isLoading,
  disabled,
  noOptionsText,
  ...props
}: SelectProps) => {
  const [showLoading, setShowLoading] = React.useState(false);
  const timeoutRef = React.useRef<NodeJS.Timeout | null>(null);

  useEffect(() => {
    if (isLoading) {
      if (timeoutRef.current) {
        clearTimeout(timeoutRef.current);
      }

      timeoutRef.current = setTimeout(() => {
        setShowLoading(true);
      }, 150);
    } else {
      if (timeoutRef.current) {
        clearTimeout(timeoutRef.current);
      }
      setShowLoading(false);
    }
  }, [isLoading]);

  const getRenderValue = (selected: string | number | (string | number)[]) => {
    if (showLoading) {
      return (
        <div className="flex items-center gap-1">
          Loading... <CircularProgress className="ml-auto" size={24} />
        </div>
      );
    }

    if (options.length === 0 && noOptionsText) {
      return <div className="text-gray-500">{noOptionsText}</div>;
    }

    if (multiSelect) {
      return (
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
                onDelete={onDelete ? () => onDelete(value) : undefined}
              />
            );
          })}
        </div>
      );
    }

    return options.find((opt) => opt.value === selected)?.label || "";
  };

  return (
    <FormControl fullWidth disabled={disabled || isLoading}>
      <InputLabel
        id={props.name ? `${props.name}-label` : undefined}
        shrink={
          (props.displayEmpty && options.length === 0) ||
          (props.value !== undefined &&
            props.value !== "" &&
            props.value !== null) ||
          (multiSelect && Array.isArray(props.value) && props.value.length > 0)
        }
      >
        {label || "Select"}
      </InputLabel>
      <MuiSelect
        labelId={props.name ? `${props.name}-label` : undefined}
        multiple={multiSelect}
        label={label || "Select"}
        disabled={disabled || isLoading}
        IconComponent={isLoading ? NoDropdownIcon : undefined}
        displayEmpty={props.displayEmpty}
        renderValue={(selected) =>
          getRenderValue(selected as (string | number)[])
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
