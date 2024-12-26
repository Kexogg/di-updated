using Microsoft.Extensions.Logging;
using TagsCloudContainerCore.Facade;
using TagsCloudContainerCore.ImageEncoders;
using TagsCloudContainerCore.Layouter;
using TagsCloudContainerCore.Renderer;
using TagsCloudContainerCore.TextProcessor;


namespace TagsCloudContainerCLI;

public class FileMode
{
    private readonly ILogger<FileMode> _logger;
    private readonly ITagCloudFactory _cloudFactory;

    public FileMode(ILogger<FileMode> logger, ITagCloudFactory cloudFactory)
    {
        _logger = logger;
        _cloudFactory = cloudFactory;
    }

    public void Generate(string text, string outputPath)
    {
        _logger.LogInformation("Generating tag cloud to {Path}", outputPath);
        
        var tagCloud = _cloudFactory.Create(builder => builder
            .UseLayouter<CircularCloudLayouter>()
            .UseWordProcessor<MyStemWordProcessor>()
            .UseRenderer<Renderer>()
            .UseImageEncoder<PngEncoder>());

        var imageBytes = tagCloud.From(text);
        
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        File.WriteAllBytes(outputPath, imageBytes);
        _logger.LogInformation("Tag cloud generated successfully");
    }
}