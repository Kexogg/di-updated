using Autofac;
using Microsoft.Extensions.Logging;
using TagsCloudContainerCore.DataProvider;
using TagsCloudContainerCore.ImageEncoders;
using TagsCloudContainerCore.Layouter;
using TagsCloudContainerCore.Renderer;
using TagsCloudContainerCore.TextProcessor;

namespace TagsCloudContainerCore.Facade;

public class TagCloudModule : Module
{
    private readonly TagCloudOptions _options;

    public TagCloudModule(TagCloudOptions options)
    {
        _options = options;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TagCloud>().As<ITagCloud>().InstancePerLifetimeScope();
        
        builder.RegisterType(_options.DataProviderType).As<IDataProvider>().SingleInstance();
        builder.RegisterType(_options.WordProcessorType).As<IWordProcessor>().SingleInstance();
        builder.RegisterType(_options.LayouterType).As<ILayouterFactory>().SingleInstance();
        builder.RegisterType(_options.RendererType).As<IRenderer>().SingleInstance();
        builder.RegisterType(_options.ImageEncoderType).As<IImageEncoder>().SingleInstance();
        

        foreach (var (serviceType, implementationType) in _options.ServiceMap)
        {
            builder.RegisterType(implementationType).As(serviceType);
        }
    }
}