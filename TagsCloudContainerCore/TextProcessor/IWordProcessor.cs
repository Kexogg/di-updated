namespace TagsCloudContainerCore.TextProcessor;

public interface IWordProcessor
{
    Dictionary<string, double> ProcessText(string word);
}

