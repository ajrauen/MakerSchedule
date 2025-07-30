import {
  ToggleButton,
  ToggleButtonGroup as MuiToggleButtonGroup,
} from "@mui/material";

interface ToggleButtonGroupProps {
  value: string;
  onChange: (value: string) => void;
  options: { label: string; value: string }[];
}

const ToggleButtonGroup = ({
  value,
  onChange,
  options,
}: ToggleButtonGroupProps) => {
  return (
    <MuiToggleButtonGroup
      value={value}
      exclusive
      onChange={(_, newValue) => onChange(newValue)}
    >
      {options.map((option) => (
        <ToggleButton key={option.value} value={option.value}>
          {option.label}
        </ToggleButton>
      ))}
    </MuiToggleButtonGroup>
  );
};

export { ToggleButtonGroup };
