namespace TagsCloudContainerCore.Facade;

public class TagCloudOptions
{
    public Type? LayouterType { get; internal set; }
    public Type? WordProcessorType { get; internal set; }
    public Type? RendererType { get; internal set; }
    public Type? ImageEncoderType { get; internal set; }
    
    internal Dictionary<Type, Type> ServiceMap { get; } = new();
    
    public void RegisterService<TService, TImplementation>() where TImplementation : TService
    {
        ServiceMap[typeof(TService)] = typeof(TImplementation);
    }
}