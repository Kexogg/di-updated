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
        builder.RegisterType<TagCloud>().AsSelf().As<ITagCloud>().InstancePerLifetimeScope();
    
        builder.RegisterType(_options.DataProviderType)
            .AsImplementedInterfaces()
            .SingleInstance()
            .OnActivated(e => _options.ConfigureInstance(e.Instance));

        builder.RegisterType(_options.WordProcessorType)
            .AsImplementedInterfaces() 
            .SingleInstance()
            .OnActivated(e => _options.ConfigureInstance(e.Instance));

        builder.RegisterType(_options.LayouterType)
            .AsImplementedInterfaces()
            .SingleInstance()
            .OnActivated(e => _options.ConfigureInstance(e.Instance));

        builder.RegisterType(_options.RendererType)
            .AsImplementedInterfaces()
            .SingleInstance()
            .OnActivated(e => _options.ConfigureInstance(e.Instance));

        builder.RegisterType(_options.ImageEncoderType)
            .AsImplementedInterfaces()
            .SingleInstance()
            .OnActivated(e => _options.ConfigureInstance(e.Instance));

        foreach (var (serviceType, implementationType) in _options.ServiceMap)
        {
            builder.RegisterType(implementationType)
                .As(serviceType)
                .SingleInstance()
                .OnActivated(e => _options.ConfigureInstance(e.Instance));
        }
    }
}