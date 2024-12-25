using SkiaSharp;
using TagsCloudContainerCore.ImageEncoders;
using TagsCloudContainerCore.Layouter;
using TagsCloudContainerCore.Models;
using TagsCloudContainerCore.Renderer;

namespace TagsCloudContainerCLI;

public class Demo
{
    private readonly IImageEncoder _encoder;
    private readonly ILayouterFactory _layouterFactory;
    private readonly IRenderer _renderer;

    public Demo(IImageEncoder encoder, ILayouterFactory layouterFactory, IRenderer renderer)
    {
        _layouterFactory = layouterFactory;
        _renderer = renderer;
        _encoder = encoder;
    }

    public void GenerateDemo()
    {
        Directory.CreateDirectory("results");

        RenderCloud(GenerateRandomCloud(10), "results/cloud_10.png");
        RenderCloud(GenerateRandomCloud(50), "results/cloud_50.png");
        RenderCloud(GenerateRandomCloud(100), "results/cloud_100.png");
    }

    private Tag[] GenerateRandomCloud(int count)
    {
        var layouter = _layouterFactory.Create();
        var tags = new Tag[count];


        for (var i = 0; i < count; i++)
        {
            var fontSize = new Random().Next(10, 50);
            var font = SKTypeface.Default.ToFont();
            font.Size = fontSize;
            var text = $"Tag{i}";
            var textWidth = font.MeasureText(text);

            var tag = new Tag
            {
                Text = text,
                FontSize = fontSize,
                Color = new Color((byte)new Random().Next(0, 255), (byte)new Random().Next(0, 255),
                    (byte)new Random().Next(0, 255)),
                Rectangle = layouter.PutNextRectangle(new Size(textWidth, fontSize))
            };
            tags[i] = tag;
        }

        return tags;
    }

    private void RenderCloud(Tag[] tags, string path)
    {
        var image = _renderer.DrawTags(tags);
        var encodedImage = _encoder.Encode(image);
        using var stream = File.OpenWrite(path);
        encodedImage.SaveTo(stream);
    }
}