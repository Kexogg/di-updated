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

        builder.RegisterType<FileDataProvider>().As<IDataProvider>();
        builder.RegisterType<MyStemWordProcessor>().As<IWordProcessor>();
        builder.RegisterType<CircularClouldLayouterFactory>().As<ILayouterFactory>();
        builder.RegisterType<Renderer.Renderer>().As<IRenderer>();
        builder.RegisterType<PngEncoder>().As<IImageEncoder>();

        foreach (var (serviceType, implementationType) in _options.ServiceMap)
        {
            builder.RegisterType(implementationType).As(serviceType);
        }
    }
}