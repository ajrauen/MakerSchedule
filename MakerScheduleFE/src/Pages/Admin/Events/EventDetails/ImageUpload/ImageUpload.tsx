import { useEffect, useState } from "react";
import { Controller, type Control } from "react-hook-form";

import { FormDropZone } from "@ms/Components/FormComponents/FormDropZone";
import { ImageCropper } from "@ms/Pages/Admin/Events/EventDetails/ImageUpload/ImageCropper/ImageCropper";

interface ImageUploadProps {
  control: Control<any>;
  name: string;
  error?: string;
}

const ImageUpload = ({ control, name, error }: ImageUploadProps) => {
  const [showCropper, setShowCropper] = useState(false);
  const [file, setFile] = useState<File | null>(null);

  let img = new window.Image();

  return (
    <Controller
      name={name}
      control={control}
      render={({
        field: { value, onChange },
        fieldState: { error: fieldError },
      }) => {
        useEffect(() => {
          if (value === file) return;
        }, [value]);

        useEffect(() => {
          if (file && file.type?.startsWith("image/")) {
            img = new window.Image();
            img.onload = () => {
              const aspect = img.width / img.height;
              if (Math.abs(aspect - 4 / 3) < 0.01) {
                setShowCropper(false);
              } else {
                setShowCropper(true);
              }
              URL.revokeObjectURL(img.src);
            };
            img.src = URL.createObjectURL(file);
          }
        }, [file]);

        return showCropper && file ? (
          <ImageCropper
            value={file}
            onChange={(value: File) => {
              setShowCropper(false);
              onChange(value);
            }}
          />
        ) : (
          <FormDropZone
            accept={{
              "image/jpeg": [],
              "image/png": [],
            }}
            error={error || fieldError?.message}
            onDrop={(files) => {
              if (Array.isArray(files)) {
                setFile(files[0]);
              } else {
                setFile(files);
              }
            }}
          />
        );
      }}
    />
  );
};

export { ImageUpload };
