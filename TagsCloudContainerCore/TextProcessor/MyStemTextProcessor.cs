using Microsoft.Extensions.Logging;
using TagsCloudContainerCore.Models;
using TagsCloudContainerCore.TextProcessor.MyStem;

namespace TagsCloudContainerCore.TextProcessor;

public class MyStemTextProcessor : ITextProcessor
{
    private ILogger<MyStemTextProcessor> _logger;

    public PartOfSpeech[] ExcludedPartsOfSpeech { get; set; } =
        [PartOfSpeech.PART, PartOfSpeech.ADV, PartOfSpeech.PR, PartOfSpeech.CONJ];

    public string[] ExcludedWords { get; set; } = [];
    
    public SortOrder SortOrder { get; set; } = SortOrder.Descending;
    
    public int MaxWordsCount { get; set; } = 50;

    public MyStemTextProcessor(ILogger<MyStemTextProcessor> logger)
    {
        _logger = logger;
        _myStemWrapper = new MyStemWrapper();
    }

    private readonly MyStemWrapper _myStemWrapper;


    public Dictionary<string, double> ProcessText(string text)
    {
        _logger.LogInformation("Start processing text with MyStem");
        var words = text.Split();
        _logger.LogInformation("Got {n} words", words.Length);
        
        _myStemWrapper.StartProcess();
        var weightedWords = GetWeightedWords(words);
        _myStemWrapper.Dispose();

        _logger.LogInformation("Finished processing text. Got {n} weighted words, excluded {e}", weightedWords.Count,
            words.Distinct().Count() - weightedWords.Count);
        
        return ProcessWeightedWords(weightedWords);
    }

    private Dictionary<string, double> GetWeightedWords(string[] words)
    {
        var weightedWords = new Dictionary<string, double>();
        foreach (var word in words)
        {
            if (word.Any(char.IsDigit)) continue;
            var wordChars = word.ToCharArray();
            for (var i = 0; i < wordChars.Length; i++)
            {
                if (!char.IsLetter(wordChars[i]))
                {
                    wordChars[i] = ' ';
                }

                wordChars[i] = char.ToLower(wordChars[i]);
            }

            var wordWithoutSpecialChars = new string(wordChars);
            if (string.IsNullOrWhiteSpace(wordWithoutSpecialChars)) continue;
            var processedWord = _myStemWrapper.ProcessWord(wordWithoutSpecialChars);
            if (processedWord == null) continue;
            if (ExcludedPartsOfSpeech.Contains(processedWord.PartOfSpeech) ||
                ExcludedWords.Contains(processedWord.NormalForm)) continue;
            if (!weightedWords.TryGetValue(processedWord.NormalForm, out var value))
            {
                weightedWords.Add(processedWord.NormalForm, 1);
            }
            else
            {
                weightedWords[processedWord.NormalForm] = ++value;
            }
        }

        return weightedWords;
    }

    private Dictionary<string, double> ProcessWeightedWords(Dictionary<string, double> weightedWords)
    {
        weightedWords = weightedWords
            .OrderByDescending(pair => pair.Value)
            .Take(MaxWordsCount)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
        weightedWords = SortOrder switch
        {
            SortOrder.Ascending => weightedWords.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value),
            SortOrder.Descending => weightedWords,
            SortOrder.Random => weightedWords.OrderBy(_ => Guid.NewGuid()).ToDictionary(pair => pair.Key, pair => pair.Value),
            _ => weightedWords
        };
        return weightedWords;
    }
}

public enum SortOrder
{
    Ascending,
    Descending,
    Random
}