import { getCroppedImg } from "@ms/Pages/Admin/Events/EventDetails/ImageUpload/getCroppedImg.utils";
import { Button } from "@mui/material";
import { useState } from "react";
import Cropper from "react-easy-crop";
import type { Area } from "react-easy-crop";

interface ImageCropperProps {
  value: File;
  onChange: (imgFile: File) => void;
}

const ImageCropper = ({
  value,

  onChange,
}: ImageCropperProps) => {
  const [crop, setCrop] = useState({ x: 0, y: 0 });
  const [zoom, setZoom] = useState(1);
  const [croppedAreaPixels, setCroppedAreaPixels] = useState<Area | null>(null);

  return (
    <div className="flex flex-col gap-x-64">
      <div>Incorrect Image Ratio: Please fix</div>
      <div className="relative w-full h-64">
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
      </div>
      <Button
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
        }}
      >
        Update Image
      </Button>
    </div>
  );
};

export { ImageCropper };
