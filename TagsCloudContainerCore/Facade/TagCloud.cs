using TagsCloudContainerCore.ImageEncoders;
using TagsCloudContainerCore.Layouter;
using TagsCloudContainerCore.Models;
using TagsCloudContainerCore.Models.Graphics;
using TagsCloudContainerCore.Renderer;
using TagsCloudContainerCore.TextProcessor;

namespace TagsCloudContainerCore.Facade;

public class TagCloud
{
    private readonly IWordProcessor _wordProcessor;
    private readonly ILayouterFactory _layouterFactory;
    private readonly IRenderer _renderer;
    private readonly IImageEncoder _imageEncoder;

    public TagCloud(
        IWordProcessor wordProcessor,
        ILayouterFactory layouterFactory, 
        IRenderer renderer,
        IImageEncoder imageEncoder)
    {
        _wordProcessor = wordProcessor;
        _layouterFactory = layouterFactory;
        _renderer = renderer;
        _imageEncoder = imageEncoder;
    }

    public byte[] From(string words)
    {
        var layouter = _layouterFactory.Create();
        var processedWords = _wordProcessor.ProcessText(words, new WordProcessorOptions([], []));
        var tags = processedWords.Select(word => new Tag
        {
            Text = word.Key,
            FontSize = (float)word.Value * 100,
            Color = new Color(0, 0, 0),
            BBox = layouter.PutNextRectangle(new Size(MeasureText(word.Key, (float)word.Value * 100), (float)word.Value * 100))
        }).ToArray();

        var renderedCloud = _renderer.DrawTags(tags);
        return _imageEncoder.Encode(renderedCloud);
    }

    private static float MeasureText(string word, float fontSize)
    {
        var font = new Font();
        font.Size = fontSize;
        return font.MeasureText(word);
    }
}