using SkiaSharp;
using TagsCloudContainerCore.Models;

namespace TagsCloudContainerCore.Layouter;

public interface ILayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
    public IReadOnlyList<Rectangle> Rectangles { get; }
}