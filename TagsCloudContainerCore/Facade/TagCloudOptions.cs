using TagsCloudContainerCore.DataProvider;
using TagsCloudContainerCore.ImageEncoders;
using TagsCloudContainerCore.Layouter;
using TagsCloudContainerCore.TextProcessor;

namespace TagsCloudContainerCore.Facade;

public class TagCloudOptions
{
    public Type DataProviderType { get; internal set; } = typeof(FileDataProvider);
    public Type LayouterType { get; internal set; } = typeof(CircularClouldLayouterFactory);
    public Type WordProcessorType { get; internal set; } = typeof(MyStemWordProcessor);
    public Type RendererType { get; internal set; } = typeof(Renderer.Renderer);
    public Type ImageEncoderType { get; internal set; } = typeof(PngEncoder);

    internal Dictionary<Type, Type> ServiceMap { get; } = new();
    
    public void RegisterService<TService, TImplementation>() where TImplementation : TService
    {
        ServiceMap[typeof(TService)] = typeof(TImplementation);
    }
}