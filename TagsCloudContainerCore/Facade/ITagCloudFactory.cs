namespace TagsCloudContainerCore.Facade;

public interface ITagCloudFactory
{
    TagCloud Create(Action<TagCloudBuilder> configure);
}