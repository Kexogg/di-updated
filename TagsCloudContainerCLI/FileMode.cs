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
                p.MaxWordsCount = _config.MaxWords;
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
                p.SortOrder = Enum.Parse<SortOrder>(_config.SortOrder, true);
            })
            .UseLayouter<CircularCloudLayouterFactory>(p =>
            {
                p.MaxFontSize = _config.MaxFontSize;
                p.MinFontSize = _config.MinFontSize;
                p.SpiralStep = _config.LayoutSpacing;
                p.InitialRadius = _config.InitialRadius;
                p.Font = new Font(_config.FontFamily);
            })
            .UseRenderer<Renderer>(r =>
            {
                r.TagFont = new Font(_config.FontFamily);
                r.RenderingScale = _config.RenderScale;
                r.BackgroundColor = new Color(_config.BackgroundColor);
                r.TextColor = new Color(_config.ForegroundColor);
            })
            .GuessEncoder(Path.GetExtension(outputPath)));

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
            ".docx" => b.UseDataProvider<OpenXmlDocumentsProvider>(),
            ".doc" => b.UseDataProvider<OpenXmlDocumentsProvider>(),
            ".ppt" => b.UseDataProvider<OpenXmlSlidesProvider>(),
            ".pptx" => b.UseDataProvider<OpenXmlSlidesProvider>(),
            ".txt" => b.UseDataProvider<FileDataProvider>(),
            _ => b.UseDataProvider<FileDataProvider>(),
        };
    }
    
    public static TagCloudBuilder GuessEncoder(this TagCloudBuilder b, string ext)
    {
        return ext switch
        {
            ".png" => b.UseImageEncoder<PngEncoder>(),
            ".jpeg" => b.UseImageEncoder<JpegEncoder>(),
            ".jpg" => b.UseImageEncoder<JpegEncoder>(),
            _ => b.UseImageEncoder<PngEncoder>(),
        };
    }
}