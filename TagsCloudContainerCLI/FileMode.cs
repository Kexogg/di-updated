using Microsoft.Extensions.Logging;
using TagsCloudContainerCLI.CLI;
using TagsCloudContainerCore.DataProvider;
using TagsCloudContainerCore.Facade;
using TagsCloudContainerCore.ImageEncoders;
using TagsCloudContainerCore.Layouter;
using TagsCloudContainerCore.Models;
using TagsCloudContainerCore.Models.Graphics;
using TagsCloudContainerCore.Renderer;
using TagsCloudContainerCore.TextProcessor;


namespace TagsCloudContainerCLI;

public class FileMode
{
    private readonly ILogger<FileMode> _logger;
    private readonly ITagCloudFactory _cloudFactory;
    private readonly TagCloudConfig _config;

    public FileMode(ILogger<FileMode> logger, ITagCloudFactory cloudFactory, TagCloudConfig config)
    {
        _logger = logger;
        _cloudFactory = cloudFactory;
        _config = config;
    }

    
    
    public void Generate(string filePath, string outputPath)
    {
        _logger.LogInformation("Generating tag cloud to {Path}", outputPath);

        
        var tagCloud = _cloudFactory.Create(builder => builder
            .GuessDataProvider(Path.GetExtension(filePath))
            .UseWordProcessor<MyStemTextProcessor>(p =>
            {
                p.ExcludedWords = _config.ExcludedWords;
                p.ExcludedPartsOfSpeech = _config.ExcludedPartsOfSpeech
                    .Select(pos =>
                    {
                        if (Enum.TryParse<PartOfSpeech>(pos, true, out var parsedPos))
                        {
                            return parsedPos;
                        }

                        _logger.LogWarning("Invalid PartOfSpeech value: {Value}", pos);
                        return (PartOfSpeech?)null;
                    })
                    .Where(pos => pos.HasValue)
                    .Select(pos => pos!.Value)
                    .ToArray();
            })
            .UseLayouter<CircularCloudLayouterFactory>(p =>
            {
                p.MaxFontSize = _config.MaxFontSize;
                p.MinFontSize = _config.MinFontSize;
                p.SpiralStep = _config.LayoutSpacing;
                p.Font = new Font(_config.FontFamily);
            })
            .UseRenderer<Renderer>(r => r.TagFont = new Font(_config.FontFamily))
            .UseImageEncoder<PngEncoder>());

        var imageBytes = tagCloud.FromFile(filePath);

        var directory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }
        var fullpath = Path.GetFullPath(outputPath);
        _logger.LogInformation("Saving tag cloud to {Path}", fullpath);
        File.WriteAllBytes(outputPath, imageBytes);
    }
}

public static class BuilderExtensions
{
    public static TagCloudBuilder GuessDataProvider(this TagCloudBuilder b, string ext)
    {
        return ext switch
        {
            ".docx" => b.UseDataProvider<OpenXmlProvider>(),
            ".txt" => b.UseDataProvider<FileDataProvider>(),
            _ => b.UseDataProvider<FileDataProvider>(),
        };
    }
}