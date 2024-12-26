using TagsCloudContainerCore.Models.Graphics;

namespace TagsCloudContainerCore.Models;

public class Tag
{
    public float FontSize;
    public required Color Color;
    public required Rectangle BBox;
    public required string Text;
}