using SkiaSharp;

namespace TagsCloudContainerCore.Layouter;

public interface ICircularCloudLayouter
{
    SKRect PutNextRectangle(SKSize rectangleSize);
    void SetCenter(SKPoint center);
    public IReadOnlyList<SKRect> Rectangles { get; }
}