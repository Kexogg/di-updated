using TagsCloudContainerCore.Models;

namespace TagsCloudContainerCore.Layouter;

public interface ILayouter
{
    public Tag[] LayoutTags(Dictionary<string, double> words);
}