using SkiaSharp;

namespace TagsCloudContainerCore.Models;

public class Tag
{
    public SKColor Color;
    public int FontSize;
    public Rectangle Rectangle;
    public required string Text;
}