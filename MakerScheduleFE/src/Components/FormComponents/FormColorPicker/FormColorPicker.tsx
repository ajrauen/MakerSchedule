import {
  Controller,
  type Control,
  type FieldValues,
  type Path,
} from "react-hook-form";

interface FormColorPickerProps<T extends FieldValues> {
  name: Path<T>;
  control: Control<T>;
  label?: string;
  className?: string;
}

const FormColorPicker = <T extends FieldValues>({
  name,
  control,
  label,
  className = "",
}: FormColorPickerProps<T>) => {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field }) => (
        <div className={`flex flex-col gap-1 ${className}`}>
          {label && (
            <label className="text-sm font-medium text-gray-700">{label}</label>
          )}
          <input
            type="color"
            value={field.value || "#000000"}
            onChange={(e) => field.onChange(e.target.value)}
            className="h-10 w-full rounded border border-gray-300 cursor-pointer disabled:cursor-not-allowed disabled:opacity-50"
          />
        </div>
      )}
    />
  );
};

export { FormColorPicker };
