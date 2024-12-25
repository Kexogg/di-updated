using Microsoft.Extensions.Logging;
using SkiaSharp;
using TagsCloudContainerCore.Models;

namespace TagsCloudContainerCore.Renderer;

public class Renderer : IRenderer
{
    private readonly SKFont _font;
    private readonly ILogger<IRenderer> _logger;
    private readonly SKPaint _paint;

    public Renderer(ILogger<IRenderer> logger)
    {
        _logger = logger;
        _font = SKTypeface.Default.ToFont();
        _paint = new SKPaint
        {
            Color = SKColors.Black,
            IsStroke = true
        };
    }

    public SKImage DrawTags(IEnumerable<Tag> tags)
    {
        var tagList = tags.ToList();
        var size = CalculateImageSize(tagList);
        var bitmap = new SKBitmap((int)size.Width, (int)size.Height);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.LightGray);
        canvas.Translate(-size.Left, -size.Top);
        _logger.LogInformation("Drawing {t} tags", tagList.Count);
        foreach (var tag in tagList)
        {
            ValidateRectangle(tag.Rectangle);
            _paint.Color = new SKColor(tag.Color.ToUint());
            _font.Size = tag.FontSize;

            var x = tag.Rectangle.Left;
            var y = tag.Rectangle.Bottom - _font.Metrics.Descent;

            canvas.DrawText(tag.Text, x, y, _font, _paint);
        }

        _logger.LogInformation("Finished drawing tags");
        return SKImage.FromBitmap(bitmap);
    }

    private static Rectangle CalculateImageSize(IEnumerable<Tag> tags)
    {
        var endY = 0;
        var endX = 0;
        var startX = int.MaxValue;
        var startY = int.MaxValue;
        foreach (var tag in tags)
        {
            endX = Math.Max(endX, (int)tag.Rectangle.Right);
            endY = Math.Max(endY, (int)tag.Rectangle.Bottom);
            startX = Math.Min(startX, (int)tag.Rectangle.Left);
            startY = Math.Min(startY, (int)tag.Rectangle.Top);
        }
        return new Rectangle(startX, startY, endX, endY);
    }

    private void ValidateRectangle(Rectangle rectangle)
    {
        if (rectangle.Left >= rectangle.Right || rectangle.Top >= rectangle.Bottom)
            _logger.LogWarning("Rectangle is invalid");
    }
}