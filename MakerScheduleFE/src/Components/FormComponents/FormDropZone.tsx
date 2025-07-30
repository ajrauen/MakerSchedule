import React from "react";
import { useDropzone, type Accept, type DropzoneOptions } from "react-dropzone";
import { Controller, type Control } from "react-hook-form";

interface FormDropZoneProps {
  onDrop?: (files: File[]) => void;
  accept?: Accept;
  multiple?: boolean;
  control?: Control<any>;
  name?: string;
  error?: string;
  showFileNames?: boolean;
  value?: File | File[];
  onChange?: (value: File | File[]) => void;
  dropText?: string;
}

const DropZoneInner = ({
  onDrop,
  accept,
  multiple = false,
  value,
  onChange,
  error,
  dropText,
  showFileNames = false,
}: FormDropZoneProps) => {
  const [rejectedFiles, setRejectedFiles] = React.useState<File[]>([]);

  const handleDrop = React.useCallback(
    (acceptedFiles: File[], rejectedFiles: any[]) => {
      setRejectedFiles([]);

      if (rejectedFiles.length > 0) {
        const rejectedFileNames = rejectedFiles
          .map((file: any) => file.file.name)
          .join(", ");
        setRejectedFiles(rejectedFiles.map((file: any) => file.file));
        return;
      }

      if (onChange) onChange(multiple ? acceptedFiles : acceptedFiles[0]);
      if (onDrop) onDrop(acceptedFiles);
    },
    [onChange, onDrop, multiple]
  );

  const { getRootProps, getInputProps, isDragActive, acceptedFiles } =
    useDropzone({
      onDrop: handleDrop,
      accept,
      multiple,
    } as unknown as DropzoneOptions);

  const filesToShow = value
    ? Array.isArray(value)
      ? value
      : [value]
    : acceptedFiles;

  const hasError = error || rejectedFiles.length > 0;
  const errorMessage =
    error ||
    (rejectedFiles.length > 0
      ? `Invalid file type(s): ${rejectedFiles.map((f) => f.name).join(", ")}`
      : "");

  return (
    <div>
      <div
        {...getRootProps()}
        className={`w-full p-4 text-center border-2 border-dashed rounded-md cursor-pointer transition-colors duration-200 ${
          isDragActive
            ? "border-blue-500 bg-blue-50"
            : hasError
              ? "border-red-500 bg-red-50"
              : "border-gray-300 bg-white"
        }`}
      >
        <input
          {...(getInputProps() as React.InputHTMLAttributes<HTMLInputElement>)}
        />
        <p className="text-gray-500">{dropText}</p>
        {showFileNames && filesToShow.length > 0 && (
          <ul className="mt-2 text-left text-sm text-gray-700">
            {filesToShow.map((file) => (
              <li key={file.name}>{file.name}</li>
            ))}
          </ul>
        )}
      </div>
      {hasError && <p className="mt-1 text-sm text-red-600">{errorMessage}</p>}
    </div>
  );
};

const FormDropZone = ({
  onDrop,
  accept,
  multiple = false,
  control,
  name,
  error,
  dropText = "Drop the files here...",
}: FormDropZoneProps) => {
  if (control && name) {
    return (
      <Controller
        name={name}
        control={control}
        render={({
          field: { value, onChange },
          fieldState: { error: fieldError },
        }) => {
          return (
            <DropZoneInner
              onDrop={onDrop}
              accept={accept}
              multiple={multiple}
              value={value}
              onChange={onChange}
              error={error || fieldError?.message}
              dropText={dropText}
            />
          );
        }}
      />
    );
  }
  return (
    <DropZoneInner
      onDrop={onDrop}
      accept={accept}
      multiple={multiple}
      error={error}
      dropText={dropText}
    />
  );
};

export { FormDropZone };
