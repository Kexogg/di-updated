using SkiaSharp;

namespace TagsCloudContainerCore.Models.Graphics;

public class Font : SKFont
{
    public Font(string familyName, float size) : base(SKTypeface.FromFamilyName(familyName), size)
    {
    }

    public Font(string familyName) : base(SKTypeface.FromFamilyName(familyName))
    {
    }

    public Font() : base(SKTypeface.Default)
    {
    }

    public Font(SKTypeface typeface, float size) : base(typeface, size)
    {
    }


    public Font(SKTypeface typeface) : base(typeface)
    {
    }

    public Font(SKFont font) : base(font.Typeface, font.Size)
    {
        Hinting = font.Hinting;
        Edging = font.Edging;
        Embolden = font.Embolden;
        Subpixel = font.Subpixel;
        LinearMetrics = font.LinearMetrics;
        ScaleX = font.ScaleX;
        SkewX = font.SkewX;
    }

    public static Font Default => new Font(SKTypeface.Default.ToFont());

    public Font ToFont() => this;
}