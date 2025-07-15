import { useEffect, useState } from "react";
import { Controller, type Control } from "react-hook-form";

import { FormDropZone } from "@ms/Components/FormComponents/FormDropZone";
import { ImageCropper } from "@ms/Pages/Admin/Events/EventDetails/ImageUpload/ImageCropper/ImageCropper";
import { Button } from "@mui/material";

interface ImageUploadProps {
  control: Control<any>;
  name: string;
  error?: string;
}

const ImageUpload = ({ control, name, error }: ImageUploadProps) => {
  const [showCropper, setShowCropper] = useState(false);

  return (
    <Controller
      name={name}
      control={control}
      render={({
        field: { value, onChange },
        fieldState: { error: fieldError },
      }) => {
        useEffect(() => {
          if (value && value.type?.startsWith("image/")) {
            const img = new window.Image();
            img.onload = () => {
              const aspect = img.width / img.height;
              if (Math.abs(aspect - 4 / 3) < 0.01) {
                setShowCropper(false);
              } else {
                setShowCropper(true);
              }
            };
            img.src = URL.createObjectURL(value);
            return () => URL.revokeObjectURL(img.src);
          } else {
            setShowCropper(false);
          }
        }, [value]);

        return (
          <>
            <FormDropZone
              control={control}
              name={name}
              accept={{ "image/*": [] }}
              error={error || fieldError?.message}
              onDrop={(files) => {
                if (Array.isArray(files)) {
                  onChange(files[0]);
                } else {
                  onChange(files);
                }
              }}
            />
            <ImageCropper
              setShowCropper={setShowCropper}
              value={value}
              showCropper={showCropper}
              onChange={onChange}
            />
          </>
        );
      }}
    />
  );
};

export { ImageUpload };
