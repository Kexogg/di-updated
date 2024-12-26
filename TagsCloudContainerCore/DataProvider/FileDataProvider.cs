using System.Text;
using Microsoft.Extensions.Logging;

namespace TagsCloudContainerCore.DataProvider;

public class FileDataProvider : IDataProvider
{
    private readonly ILogger<IDataProvider> _logger;

    public FileDataProvider(ILogger<IDataProvider> logger)
    {
        _logger = logger;
    }

    public string GetData(byte[] data)
    {
        _logger.LogInformation("Start reading data");
        var text = Encoding.UTF8.GetString(data);
        _logger.LogInformation("Read {n} bytes from file", data.Length);
        return text;
    }
}