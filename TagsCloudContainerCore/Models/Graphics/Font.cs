using SkiaSharp;

namespace TagsCloudContainerCore.Models;

public class Font
{
    private readonly SKTypeface _typeface;
    private readonly float _size;
    private readonly float _scaleX;
    private readonly float _skewX;

    public Font(SKTypeface typeface, float size = 12, float scaleX = 1, float skewX = 0)
    {
        _typeface = typeface;
        _size = size;
        _scaleX = scaleX;
        _skewX = skewX;
    }

    public Font()
    {
        _typeface = SKTypeface.Default;
    }

    public SKFont ToSKFont()
    {
        return new SKFont(_typeface, _size, _scaleX, _skewX);
    }

    public float Size => _size;
    public float ScaleX => _scaleX;
    public float SkewX => _skewX;
    public SKTypeface Typeface => _typeface;
}