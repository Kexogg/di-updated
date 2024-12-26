using TagsCloudContainerCore.DataProvider;
using TagsCloudContainerCore.ImageEncoders;
using TagsCloudContainerCore.Layouter;
using TagsCloudContainerCore.Renderer;
using TagsCloudContainerCore.TextProcessor;

namespace TagsCloudContainerCore.Facade;

public class TagCloudBuilder
{
    private readonly TagCloudOptions _options = new();

    public TagCloudBuilder UseDataProvider<T>(Action<T>? configure = null) where T : IDataProvider
    {
        _options.DataProviderType = typeof(T);
        if (configure != null)
            _options.ConfigureService(configure);
        return this;
    }
    
    public TagCloudBuilder UseLayouter<T>(Action<T>? configure = null) where T : ILayouterFactory
    {
        _options.LayouterType = typeof(T);
        if (configure != null)
            _options.ConfigureService(configure);
        return this;
    }
    
    public TagCloudBuilder UseWordProcessor<T>(Action<T>? configure = null) where T : IWordProcessor
    {
        _options.WordProcessorType = typeof(T);
        if (configure != null)
            _options.ConfigureService(configure);
        return this;
    }
    
    public TagCloudBuilder UseRenderer<T>(Action<T>? configure = null) where T : IRenderer
    {
        _options.RendererType = typeof(T);
        if (configure != null)
            _options.ConfigureService(configure);
        return this;
    }
    
    public TagCloudBuilder UseImageEncoder<T>(Action<T>? configure = null) where T : IImageEncoder
    {
        _options.ImageEncoderType = typeof(T);
        if (configure != null)
            _options.ConfigureService(configure);
        return this;
    }

    public TagCloudBuilder RegisterService<TService, TImplementation>() where TImplementation : TService
    {
        _options.RegisterService<TService, TImplementation>();
        return this;
    }

    public TagCloudOptions Build() => _options;
}