namespace TagsCloudContainerCore.Facade;

public interface ITagCloudFactory
{
    ITagCloud Create(Action<TagCloudBuilder> configure);
}