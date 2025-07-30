import { useEffect, useState } from "react";
import { Controller, type Control } from "react-hook-form";

import { FormDropZone } from "@ms/Components/FormComponents/FormDropZone";
import { ImageCropper } from "@ms/Pages/Admin/Events/EventDetails/ImageUpload/ImageCropper/ImageCropper";
import { ConfirmationDialog } from "@ms/Components/Dialogs/Confirmation";
import { getCroppedImg } from "@ms/Pages/Admin/Events/EventDetails/ImageUpload/getCroppedImg.utils";
import type { Area } from "react-easy-crop";

interface ImageUploadProps {
  control: Control<any>;
  name: string;
  error?: string;
}

const ImageUpload = ({ control, name, error }: ImageUploadProps) => {
  const [showCropper, setShowCropper] = useState(false);
  const [file, setFile] = useState<File | null>(null);
  const [croppedAreaPixels, setCroppedAreaPixels] = useState<Area | null>(null);
  const [imageUrl, setImageUrl] = useState<string | null>(null);

  useEffect(() => {
    if (file && file.type?.startsWith("image/")) {
      const img = new window.Image();
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

  // Handler for cropping and updating form value
  const handleCropConfirm = async (onChange: (file: File) => void) => {
    if (!file || !croppedAreaPixels) return;
    const croppedBlob = await getCroppedImg(
      URL.createObjectURL(file),
      croppedAreaPixels
    );
    const croppedFile = new File([croppedBlob], file.name, {
      type: file.type,
    });
    onChange(croppedFile);
    setShowCropper(false);
    setFile(null);
  };

  useEffect(() => {
    if (file) {
      const url = URL.createObjectURL(file);
      setImageUrl(url);
      return () => {
        URL.revokeObjectURL(url);
        setImageUrl(null);
      };
    }
  }, [file]);

  return (
    <Controller
      name={name}
      control={control}
      render={({
        field: { value, onChange },
        fieldState: { error: fieldError },
      }) => (
        <>
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
          {showCropper && file && (
            <ConfirmationDialog
              open={showCropper}
              onCancel={() => setShowCropper(false)}
              onConfirm={() => handleCropConfirm(onChange)}
              title="Crop Image"
              maxWidth="md"
            >
              <ImageCropper
                imageUrl={imageUrl}
                onCroppedAreaPixels={setCroppedAreaPixels}
                onChange={(croppedFile: File) => {
                  setShowCropper(false);
                  setFile(null);
                  onChange(croppedFile);
                }}
              />
            </ConfirmationDialog>
          )}
        </>
      )}
    />
  );
};

export { ImageUpload };
