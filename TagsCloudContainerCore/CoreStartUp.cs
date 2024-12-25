using Autofac;
using TagsCloudContainerCore.DataProvider;
using TagsCloudContainerCore.ImageEncoders;
using TagsCloudContainerCore.Layouter;
using TagsCloudContainerCore.Renderer;
using TagsCloudContainerCore.TextProcessor;

namespace TagsCloudContainerCore;

public static class CoreStartUp
{
    public static void RegisterServices(ContainerBuilder builder)
    {
        builder.RegisterType<FileDataProvider>()
            .As<IDataProvider>();

        builder.RegisterType<MyStemWordProcessor>()
            .As<IWordProcessor>();

        builder.RegisterType<CircularClouldLayouterFactory>()
            .As<ILayouterFactory>();

        builder.RegisterType<Renderer.Renderer>()
            .As<IRenderer>();

        builder.RegisterType<PngEncoder>()
            .As<IImageEncoder>();
    }
}