using TagsCloudContainerCore.DataProvider;
using TagsCloudContainerCore.ImageEncoders;
using TagsCloudContainerCore.Layouter;
using TagsCloudContainerCore.Renderer;
using TagsCloudContainerCore.TextProcessor;

namespace TagsCloudContainerCore.Facade;

public class TagCloudBuilder
{
    private readonly TagCloudOptions _options = new();

    public TagCloudBuilder UseDataProvider<T>() where T : IDataProvider
    {
        _options.DataProviderType = typeof(T);
        return this;
    }
    
    public TagCloudBuilder UseLayouter<T>() where T : ILayouterFactory
    {
        _options.LayouterType = typeof(T);
        return this;
    }

    public TagCloudBuilder UseWordProcessor<T>() where T : IWordProcessor
    {
        _options.WordProcessorType = typeof(T);
        return this;
    }

    public TagCloudBuilder UseRenderer<T>() where T : IRenderer
    {
        _options.RendererType = typeof(T);
        return this;
    }

    public TagCloudBuilder UseImageEncoder<T>() where T : IImageEncoder
    {
        _options.ImageEncoderType = typeof(T);
        return this;
    }

    public TagCloudBuilder RegisterService<TService, TImplementation>() where TImplementation : TService
    {
        _options.RegisterService<TService, TImplementation>();
        return this;
    }

    public TagCloudOptions Build() => _options;
}