import { useEffect, useState } from "react";
import { Controller, type Control } from "react-hook-form";

import { FormDropZone } from "@ms/Components/FormComponents/FormDropZone";
import { ImageCropper } from "@ms/Pages/Admin/Events/EventDetails/ImageUpload/ImageCropper/ImageCropper";
import { Button } from "@mui/material";
import { set } from "zod";

interface ImageUploadProps {
  control: Control<any>;
  name: string;
  error?: string;
}

const ImageUpload = ({ control, name, error }: ImageUploadProps) => {
  const [showCropper, setShowCropper] = useState(false);
  const [newEventFileUrl, setNewEventFileUrl] = useState<string | null>(null);

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
          if (!value && newEventFileUrl) {
            setShowCropper(false);
            URL.revokeObjectURL(img.src);
          }
        }, [value]);

        const handleReset = () => {
          if (img.src) {
            URL.revokeObjectURL(img.src);
          }
          setNewEventFileUrl(null);
          onChange(null);
          setShowCropper(false);
        };

        useEffect(() => {
          if (value && value.type?.startsWith("image/")) {
            img = new window.Image();
            img.onload = () => {
              const aspect = img.width / img.height;
              if (Math.abs(aspect - 4 / 3) < 0.01) {
                setNewEventFileUrl(img.src);
                setShowCropper(false);
              } else {
                setShowCropper(true);
              }
            };
            img.src = URL.createObjectURL(value);
            return () => URL.revokeObjectURL(img.src);
          } else {
            setNewEventFileUrl(null);
            URL.revokeObjectURL(img.src);
            setShowCropper(false);
          }
        }, [value]);

        return (
          <>
            {newEventFileUrl || (value && !showCropper) ? (
              <div className="flex flex-row">
                <img
                  src={newEventFileUrl ?? value}
                  alt="Event Preview"
                  className="w-full h-64 object-cover mb-4"
                />
                <span>
                  <Button variant="contained" onClick={handleReset}>
                    Reset
                  </Button>
                </span>
              </div>
            ) : showCropper ? (
              <ImageCropper
                value={value}
                onChange={(value: File) => {
                  URL.revokeObjectURL(img.src);
                  setShowCropper(false);
                  onChange(value);
                }}
              />
            ) : (
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
            )}
          </>
        );
      }}
    />
  );
};

export { ImageUpload };
