using SkiaSharp;
using TagsCloudContainerCore.Models;

namespace TagsCloudContainerCore.Layouter;

public interface ILayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
    void SetCenter(Point center);
    public IReadOnlyList<Rectangle> Rectangles { get; }
}