import {
  Controller,
  type Control,
  type FieldValues,
  type Path,
} from "react-hook-form";
import { TextField, type TextFieldProps } from "@mui/material";
import { NumericFormat } from "react-number-format";

type TextNumericFormatProps<T extends FieldValues> = TextFieldProps & {
  control: Control<T>;
  name: Path<T>;
};

const TextNumericFormat = <T extends FieldValues>({
  control,
  name,
  ...props
}: TextNumericFormatProps<T>) => {
  return (
    <Controller
      control={control}
      name={name}
      render={({ field: { onChange, value } }) => (
        <NumericFormat
          // @ts-expect-error whatever
          value={value}
          onValueChange={(values) => {
            onChange(values.floatValue || 0);
          }}
          customInput={TextField}
          thousandSeparator
          valueIsNumericString
          prefix="$"
          decimalScale={2}
          fixedDecimalScale={true}
          allowNegative={false}
          {...props}
        />
      )}
    />
  );
};

export { TextNumericFormat };
