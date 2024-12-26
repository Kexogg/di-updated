using Microsoft.Extensions.Logging;
using SkiaSharp;
using TagsCloudContainerCore.Models;
using TagsCloudContainerCore.Models.Graphics;

namespace TagsCloudContainerCore.Renderer;

public class Renderer : IRenderer
{
    private readonly SKFont _font;
    private readonly ILogger<IRenderer> _logger;
    private readonly SKPaint _paint;
    public Color BackgroundColor { get; set; } = new(255, 255, 255);
    public Color TextColor { get; set; } = new(0, 0, 0);
    private float _renderingScale = 1;

    public float RenderingScale
    {
        get => _renderingScale;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(RenderingScale),
                    "Rendering scale must be greater than 0.");
            }

            _renderingScale = value;
        }
    }


    public Renderer(ILogger<IRenderer> logger)
    {
        _logger = logger;
        _font = SKTypeface.Default.ToFont();
        _paint = new SKPaint();
    }

    public SKImage DrawTags(IEnumerable<Tag> tags)
    {
        var tagList = tags.ToList();
        var size = CalculateImageSize(tagList);
        _logger.LogInformation("Rendering scale is {scale}", RenderingScale);

        var bitmap = new SKBitmap((int)(size.Width * RenderingScale), (int)(size.Height * RenderingScale));
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(new SKColor(BackgroundColor.ToUint()));
        canvas.Translate(-size.Left * RenderingScale, -size.Top * RenderingScale);
        _logger.LogInformation("Drawing {t} tags", tagList.Count);
        _paint.Color = new SKColor(TextColor.ToUint());
        foreach (var tag in tagList)
        {
            ValidateRectangle(tag.BBox);
            _font.Size = tag.FontSize * RenderingScale;

            var x = tag.BBox.Left * RenderingScale;
            var y = tag.BBox.Bottom * RenderingScale - _font.Metrics.Descent;

            canvas.DrawText(tag.Text, x, y, _font, _paint);
        }

        _logger.LogInformation("Finished drawing tags");
        return SKImage.FromBitmap(bitmap);
    }

    private Rectangle CalculateImageSize(IEnumerable<Tag> tags)
    {
        var endY = 0;
        var endX = 0;
        var startX = int.MaxValue;
        var startY = int.MaxValue;
        foreach (var tag in tags)
        {
            endX = Math.Max(endX, (int)tag.BBox.Right);
            endY = Math.Max(endY, (int)tag.BBox.Bottom);
            startX = Math.Min(startX, (int)tag.BBox.Left);
            startY = Math.Min(startY, (int)tag.BBox.Top);
        }

        _logger.LogInformation("Calculated image size: {x}x{y}", endX - startX, endY - startY);
        return new Rectangle(startX, startY, endX, endY);
    }

    private void ValidateRectangle(Rectangle rectangle)
    {
        if (rectangle.Left >= rectangle.Right || rectangle.Top >= rectangle.Bottom)
            _logger.LogWarning("Rectangle is invalid");
    }
}