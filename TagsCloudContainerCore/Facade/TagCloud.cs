using System.Text;
using TagsCloudContainerCore.DataProvider;
using TagsCloudContainerCore.ImageEncoders;
using TagsCloudContainerCore.Layouter;
using TagsCloudContainerCore.Models;
using TagsCloudContainerCore.Models.Graphics;
using TagsCloudContainerCore.Renderer;
using TagsCloudContainerCore.TextProcessor;

namespace TagsCloudContainerCore.Facade;

public class TagCloud : ITagCloud
{
    private readonly ITextProcessor _textProcessor;
    private readonly ILayouterFactory _layouterFactory;
    private readonly IRenderer _renderer;
    private readonly IImageEncoder _imageEncoder;
    private readonly IDataProvider _dataProvider;

    public TagCloud(
        ITextProcessor textProcessor,
        ILayouterFactory layouterFactory, 
        IRenderer renderer,
        IImageEncoder imageEncoder, 
        IDataProvider dataProvider)
    {
        _textProcessor = textProcessor;
        _layouterFactory = layouterFactory;
        _renderer = renderer;
        _imageEncoder = imageEncoder;
        _dataProvider = dataProvider;
    }

    public byte[] FromFile(string filePath)
    {
        var data = _dataProvider.GetData(File.ReadAllBytes(filePath));
        return ProcessString(data);
    }
    
    public byte[] FromString(string data)
    {
        return ProcessString(data);
    }
    
    public byte[] FromBytes(byte[] data)
    {
        var text = Encoding.UTF8.GetString(data);
        return ProcessString(text);
    }
    
    private byte[] ProcessString(string data)
    {
        var layouter = _layouterFactory.Create();
        var processedWords = _textProcessor.ProcessText(data);
        var tags = layouter.LayoutTags(processedWords);
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