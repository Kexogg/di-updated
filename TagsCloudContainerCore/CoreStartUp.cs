using Autofac;
using TagsCloudContainerCore.DataProvider;
using TagsCloudContainerCore.ImageEncoders;
using TagsCloudContainerCore.Layouter;
using TagsCloudContainerCore.Renderer;
using TagsCloudContainerCore.WordProcessor;
using TagsCloudContainerCore.WordsRanker;

namespace TagsCloudContainerCore;

public static class CoreStartUp
{
    public static void RegisterServices(ContainerBuilder builder)
    {
        builder.RegisterType<FileDataProvider>()
            .As<IDataProvider>();

        builder.RegisterType<HunspellWordProcessor>()
            .As<IWordProcessor>();

        builder.RegisterType<WordsRanker.WordsRanker>()
            .As<IWordsRanker>();

        builder.RegisterType<Layouter.CircularCloudLayouter>()
            .As<ILayouter>();

        builder.RegisterType<Renderer.Renderer>()
            .As<IRenderer>();

        builder.RegisterType<PngEncoder>()
            .As<IImageEncoder>();
    }
}