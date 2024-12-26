using SkiaSharp;

namespace TagsCloudContainerCore.ImageEncoders;

public class JpegEncoder : IImageEncoder
{
    public byte[] Encode(SKImage image)
    {
        ArgumentNullException.ThrowIfNull(image);
        return image.Encode(SKEncodedImageFormat.Jpeg, 100).ToArray();
    }
}