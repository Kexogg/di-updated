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
        builder.RegisterType<TagCloud>().AsSelf().InstancePerLifetimeScope();
        /*builder.RegisterType<FileDataProvider>().As<IDataProvider>();
        builder.RegisterType<MyStemWordProcessor>().As<IWordProcessor>();
        builder.RegisterType<CircularClouldLayouterFactory>().As<ILayouterFactory>();
        builder.RegisterType<Renderer.Renderer>().As<IRenderer>();
        builder.RegisterType<PngEncoder>().As<IImageEncoder>();*/
        
        builder.RegisterType(_options.DataProviderType).As<IDataProvider>();
        builder.RegisterType(_options.WordProcessorType).As<IWordProcessor>();
        builder.RegisterType(_options.LayouterType).As<ILayouterFactory>();
        builder.RegisterType(_options.RendererType).As<IRenderer>();
        builder.RegisterType(_options.ImageEncoderType).As<IImageEncoder>();
        

        foreach (var (serviceType, implementationType) in _options.ServiceMap)
        {
            builder.RegisterType(implementationType).As(serviceType);
        }
    }
}