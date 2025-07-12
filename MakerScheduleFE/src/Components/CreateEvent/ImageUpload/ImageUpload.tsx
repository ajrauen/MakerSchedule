import React from "react";
import { Controller, type Control } from "react-hook-form";
import Cropper from "react-easy-crop";
import type { Area } from "react-easy-crop";
import getCroppedImg from "@ms/Components/CreateEvent/ImageUpload/getCroppedImg";
import { FormDropZone } from "@ms/Components/FormComponents/FormDropZone";

interface ImageUploadProps {
  control: Control<any>;
  name: string;
  error?: string;
}

const ImageUpload = ({ control, name, error }: ImageUploadProps) => {
  const [showCropper, setShowCropper] = React.useState(false);
  const [crop, setCrop] = React.useState({ x: 0, y: 0 });
  const [zoom, setZoom] = React.useState(1);
  const [croppedAreaPixels, setCroppedAreaPixels] = React.useState<Area | null>(
    null
  );

  return (
    <Controller
      name={name}
      control={control}
      render={({
        field: { value, onChange },
        fieldState: { error: fieldError },
      }) => {
        React.useEffect(() => {
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

        return value && value.type?.startsWith("image/") ? (
          showCropper ? (
            <div className="relative w-full h-64 bg-gray-200">
              <Cropper
                image={URL.createObjectURL(value)}
                crop={crop}
                zoom={zoom}
                aspect={4 / 3}
                onCropChange={setCrop}
                onZoomChange={setZoom}
                onCropComplete={(_, croppedAreaPixels) =>
                  setCroppedAreaPixels(croppedAreaPixels)
                }
              />
              <button
                className="absolute bottom-2 right-2 bg-blue-600 text-white px-4 py-2 rounded"
                onClick={async () => {
                  if (!croppedAreaPixels) return;
                  const croppedBlob = await getCroppedImg(
                    URL.createObjectURL(value),
                    croppedAreaPixels
                  );
                  const croppedFile = new File([croppedBlob], value.name, {
                    type: value.type,
                  });
                  onChange(croppedFile);
                  setShowCropper(false);
                }}
              >
                Save Crop
              </button>
            </div>
          ) : (
            <div className="flex flex-col items-center">
              <img
                src={URL.createObjectURL(value)}
                alt={value.name}
                className="max-w-xs max-h-48 rounded shadow"
              />
              <p className="mt-2 text-sm text-gray-700">{value.name}</p>
            </div>
          )
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
        );
      }}
    />
  );
};

export { ImageUpload };
