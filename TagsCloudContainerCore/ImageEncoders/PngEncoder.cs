using SkiaSharp;

namespace TagsCloudContainerCore.ImageEncoders;

public class PngEncoder : IImageEncoder
{
    public byte[] Encode(SKImage image)
    {
        ArgumentNullException.ThrowIfNull(image);
        return image.Encode(SKEncodedImageFormat.Png, 100).ToArray();
    }
}