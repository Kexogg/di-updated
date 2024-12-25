using TagsCloudContainerCore.TextProcessor.MyStem;

namespace TagsCloudContainerCore.TextProcessor;

public class MyStemWordProcessor : IWordProcessor
{
    public MyStemWordProcessor()
    {
        _myStemWrapper = new MyStemWrapper();
    }
    
    private readonly MyStemWrapper _myStemWrapper;
    
   
    public Dictionary<string, double> ProcessText(string text, WordProcessorOptions options)
    {
        _myStemWrapper.StartProcess();
        var words = text.Split();
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
        return weightedWords;
    }
}