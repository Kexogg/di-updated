using DocumentFormat.OpenXml.Packaging;
using Microsoft.Extensions.Logging;

namespace TagsCloudContainerCore.DataProvider;

public class OpenXmlProvider : IDataProvider
{
    private readonly ILogger<IDataProvider> _logger;

    public OpenXmlProvider(ILogger<IDataProvider> logger)
    {
        _logger = logger;
    }

    public string GetData(byte[] data)
    {
        _logger.LogInformation("Reading data with OpenXmlProvider");
        using var doc = WordprocessingDocument.Open(new MemoryStream(data), false);

        if (doc.MainDocumentPart?.Document.Body == null)
        {
            _logger.LogError("Document is empty");
            return string.Empty;
        }

        var text = doc.MainDocumentPart.Document.Body.InnerText;


        _logger.LogInformation("Read {w} characters from file", data.Length);

        return text;
    }
}