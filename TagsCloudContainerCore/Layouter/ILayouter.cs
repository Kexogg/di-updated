using SkiaSharp;
using TagsCloudContainerCore.Models;
using TagsCloudContainerCore.Models.Graphics;

namespace TagsCloudContainerCore.Layouter;

public interface ILayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
    public IReadOnlyList<Rectangle> Rectangles { get; }
}