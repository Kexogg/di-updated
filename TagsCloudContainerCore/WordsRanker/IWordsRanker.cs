namespace TagsCloudContainerCore.WordsRanker;

public interface IWordsRanker
{
    public Dictionary<string, double> GetWordsRank(string[] words);
}