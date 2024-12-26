using DocumentFormat.OpenXml.Packaging;
using Microsoft.Extensions.Logging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Presentation;
using System.Text;
using DocumentFormat.OpenXml;

namespace TagsCloudContainerCore.DataProvider;

public class OpenXmlDocumentsProvider : IDataProvider
{
    private readonly ILogger<IDataProvider> _logger;

    public OpenXmlDocumentsProvider(ILogger<IDataProvider> logger)
    {
        _logger = logger;
    }

    public string GetData(byte[] data)
    {
        _logger.LogInformation("Reading data with OpenXmlDocumentsProvider");
        using var stream = new MemoryStream(data);

        try
        {
            using var doc = WordprocessingDocument.Open(stream, false);
            if (doc.MainDocumentPart?.Document.Body != null)
            {
                var result = doc.MainDocumentPart.Document.Body.InnerText;
                _logger.LogInformation("Read {w} characters from document", result.Length);
                return result;
            }
        }
        catch (Exception ex) when (ex is OpenXmlPackageException or InvalidDataException)
        {
            throw new InvalidDataException("Not a document", ex);
        }

        return string.Empty;
    }
}