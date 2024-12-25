using TagsCloudContainerCore.Models;

namespace TagsCloudContainerCore.TextProcessor;

public record WordProcessorOptions(PartOfSpeech[] ExcludedPartsOfSpeech, string[] ExcludedWords)
{
    public readonly PartOfSpeech[] ExcludedPartsOfSpeech = ExcludedPartsOfSpeech;
    public readonly string[] ExcludedWords = ExcludedWords;
}