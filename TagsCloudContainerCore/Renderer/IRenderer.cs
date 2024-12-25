using SkiaSharp;
using TagsCloudContainerCore.Models;

namespace TagsCloudContainerCore.Renderer;

public interface IRenderer
{
    SKImage DrawTags(IEnumerable<Tag> tags);
}