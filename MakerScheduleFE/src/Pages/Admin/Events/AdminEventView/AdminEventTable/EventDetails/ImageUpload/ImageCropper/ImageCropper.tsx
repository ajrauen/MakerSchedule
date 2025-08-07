import { Slider } from "@mui/material";
import { useState } from "react";
import Cropper from "react-easy-crop";
import type { Area } from "react-easy-crop";

interface ImageCropperProps {
  imageUrl: string | null;
  onChange: (imgFile: File) => void;
  onCroppedAreaPixels: (area: Area) => void;
  croppedAreaPixels?: Area | null;
}

const ImageCropper = ({ imageUrl, onCroppedAreaPixels }: ImageCropperProps) => {
  const [crop, setCrop] = useState({ x: 0, y: 0 });
  const [zoom, setZoom] = useState(1);
  const [minZoom, setMinZoom] = useState(1);
  const [cropSize, setCropSize] = useState<
    { width: number; height: number } | undefined
  >(undefined);

  // the image isnt centered and it makes me angry but its not worth the effort to fix right now >:|
  const handleMediaLoaded = (size: { width: number; height: number }) => {
    const containerHeight = 256;
    const containerWidth = containerHeight * (4 / 3);
    setCropSize({ width: containerWidth, height: containerHeight });
    const zoomToFill = containerHeight / size.height;
    setZoom(zoomToFill);
    setMinZoom(zoomToFill);
    setCrop({ x: 0, y: 0 });
  };

  return (
    <div className="h-full " style={{ overflow: "hidden" }}>
      {imageUrl && (
        <div className="p-6">
          <div className="relative w-full h-64 ">
            <Cropper
              image={imageUrl}
              crop={crop}
              zoom={zoom}
              minZoom={minZoom}
              aspect={4 / 3}
              cropSize={cropSize}
              onCropChange={setCrop}
              onZoomChange={setZoom}
              onCropComplete={(_, croppedAreaPixels) => {
                onCroppedAreaPixels(croppedAreaPixels);
              }}
              onMediaLoaded={handleMediaLoaded}
            />
          </div>
          <Slider
            aria-label="Volume"
            max={3}
            min={1}
            value={zoom}
            step={0.1}
            onChange={(e, zoom) => setZoom(zoom)}
          />
        </div>
      )}
    </div>
  );
};

export { ImageCropper };
