using TagsCloudContainerCore.DataProvider;
using TagsCloudContainerCore.ImageEncoders;
using TagsCloudContainerCore.Layouter;
using TagsCloudContainerCore.TextProcessor;

namespace TagsCloudContainerCore.Facade;

public class TagCloudOptions
{
    public Type DataProviderType { get; internal set; } = typeof(FileDataProvider);
    public Type LayouterType { get; internal set; } = typeof(CircularCloudLayouterFactory);
    public Type WordProcessorType { get; internal set; } = typeof(MyStemTextProcessor);
    public Type RendererType { get; internal set; } = typeof(Renderer.Renderer);
    public Type ImageEncoderType { get; internal set; } = typeof(PngEncoder);

    internal Dictionary<Type, Type> ServiceMap { get; } = new();
    
    public void RegisterService<TService, TImplementation>() where TImplementation : TService
    {
        ServiceMap[typeof(TService)] = typeof(TImplementation);
    }
    
    private readonly Dictionary<Type, Action<object>> _configureDelegates = new();
    public void ConfigureService<T>(Action<T> configure)
    {
        _configureDelegates[typeof(T)] = service => configure((T)service);
    }
    
    public void ConfigureInstance(object instance)
    {
        var type = instance.GetType();
        if (_configureDelegates.TryGetValue(type, out var configure))
        {
            configure(instance);
        }
    }
}