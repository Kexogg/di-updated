namespace TagsCloudContainerCore.TextProcessor;

public interface ITextProcessor
{
    Dictionary<string, double> ProcessText(string word);
}

