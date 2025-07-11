using System.Xml.Linq;

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

    public static bool IsSvg(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        try
        {
            var doc = System.Xml.Linq.XDocument.Load(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return doc.Root?.Name.LocalName == "svg";
        }
        catch
        {
            stream.Seek(0, SeekOrigin.Begin);
            return false;
        }
    }
    public static bool IsSvgAspectRatioValid(Stream stream, double imageAspect)
    {
        stream.Seek(0, SeekOrigin.Begin);
        var doc = XDocument.Load(stream);
        var svg = doc.Root;
        if (svg == null || svg.Name.LocalName != "svg")
            return false;

        var widthAttr = svg.Attribute("width")?.Value;
        var heightAttr = svg.Attribute("height")?.Value;
        if (widthAttr != null && heightAttr != null &&
            double.TryParse(widthAttr, out var width) &&
            double.TryParse(heightAttr, out var height))
        {
            var aspect = width / height;
            return Math.Abs(aspect - imageAspect) < 0.01;
        }

        // Optionally, check viewBox if width/height are missing
        var viewBox = svg.Attribute("viewBox")?.Value;
        if (viewBox != null)
        {
            var parts = viewBox.Split(' ');
            if (parts.Length == 4 &&
                double.TryParse(parts[2], out var vbWidth) &&
                double.TryParse(parts[3], out var vbHeight))
            {
                var aspect = vbWidth / vbHeight;
                return Math.Abs(aspect - imageAspect) < 0.01;
            }
        }

        return false;
    }

}
