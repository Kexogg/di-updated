using Microsoft.Extensions.Logging;
using TagsCloudContainerCore.TextProcessor.MyStem;

namespace TagsCloudContainerCore.TextProcessor;

public class MyStemWordProcessor : IWordProcessor
{
    private ILogger<MyStemWordProcessor> _logger;

    public MyStemWordProcessor(ILogger<MyStemWordProcessor> logger)
    {
        _logger = logger;
        _myStemWrapper = new MyStemWrapper();
    }

    private readonly MyStemWrapper _myStemWrapper;


    public Dictionary<string, double> ProcessText(string text, WordProcessorOptions options)
    {
        _logger.LogInformation("Start processing text");
        _myStemWrapper.StartProcess();
        var words = text.Split();
        _logger.LogInformation("Got {n} words from MyStem", words.Length);
        var weightedWords = new Dictionary<string, double>();
        foreach (var word in words)
        {
            var processedWord = _myStemWrapper.ProcessWord(word.ToLower());
            if (options.ExcludedPartsOfSpeech.Contains(processedWord.PartOfSpeech) ||
                options.ExcludedWords.Contains(processedWord.NormalForm)) continue;
            if (!weightedWords.TryGetValue(processedWord.NormalForm, out var value))
            {
                weightedWords.Add(processedWord.NormalForm, 1);
            }
            else
            {
                weightedWords[processedWord.NormalForm] = ++value;
            }
        }

        _logger.LogInformation("Finished processing text. Got {n} weighted words, excluded {e}", weightedWords.Count,
            words.Length - weightedWords.Count);
        return weightedWords;
    }
}