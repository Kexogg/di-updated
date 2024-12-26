using SkiaSharp;

namespace TagsCloudContainerCore.ImageEncoders;

public interface IImageEncoder
{
    byte[] Encode(SKImage image);
}