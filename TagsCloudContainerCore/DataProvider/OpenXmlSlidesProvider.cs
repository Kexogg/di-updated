using DocumentFormat.OpenXml.Packaging;
using Microsoft.Extensions.Logging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Presentation;
using System.Text;
using DocumentFormat.OpenXml;

namespace TagsCloudContainerCore.DataProvider;

public class OpenXmlSlidesProvider : IDataProvider
{
    private readonly ILogger<IDataProvider> _logger;

    public OpenXmlSlidesProvider(ILogger<IDataProvider> logger)
    {
        _logger = logger;
    }

    public string GetData(byte[] data)
    {
        _logger.LogInformation("Reading data with OpenXmlProvider");
        using var stream = new MemoryStream(data);
        string result;

        try
        {
            using var doc = PresentationDocument.Open(stream, false);
            var text = new StringBuilder();
            var slides = doc.PresentationPart?.SlideParts;

            if (slides != null)
            {
                foreach (var slide in slides)
                {
                    text.AppendLine(slide.Slide.InnerText);
                }

                result = text.ToString();
                _logger.LogInformation("Read {w} characters from presentation", result.Length);
                return result;
            }
        }
        catch (Exception ex) when (ex is OpenXmlPackageException or InvalidDataException)
        {
            throw new InvalidDataException("Not a presentation", ex);
            stream.Position = 0;
        }

        return string.Empty;
    }
}