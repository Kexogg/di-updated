using Microsoft.Extensions.Logging;
using TagsCloudContainerCore.WordProcessor;

namespace TagsCloudContainerCore.WordsRanker;

public class WordsRanker : IWordsRanker
{
    private readonly ILogger<IWordsRanker> _logger;
    private readonly IWordProcessor _processor;

    public WordsRanker(IWordProcessor processor, ILogger<IWordsRanker> logger)
    {
        _processor = processor;
        _logger = logger;
    }

    public Dictionary<string, double> GetWordsRank(string[] words)
    {
        _logger.LogInformation("Ranking {words} words", words.Length);
        var rank = new Dictionary<string, double>();
        foreach (var word in words)
        {
            var processedWord = _processor.ProcessWord(word);
            if (rank.TryGetValue(processedWord, out var value))
                rank[processedWord] = ++value;
            else
                rank[processedWord] = 1;
        }

        _logger.LogInformation("Finished ranking words. Found {rankedWords} unique words", rank.Count);
        return rank;
    }
}