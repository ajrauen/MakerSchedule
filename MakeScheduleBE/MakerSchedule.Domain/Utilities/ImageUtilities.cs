   using SixLabors.ImageSharp;
   using SixLabors.ImageSharp.PixelFormats;

namespace MakerSchedule.Domain.Utilties.ImageUtilities;

public static class ImageUtilities
{
    public static bool IsEventImageAspectRatioValid(Stream stream, double imageAspect, double tolerance = 0.01)
    {

        stream.Position = 0;
        using var image = Image.Load<Rgba32>(stream);
        double ratio = (double)image.Width / image.Height;
        return Math.Abs(ratio - imageAspect) < tolerance;
    }
}
