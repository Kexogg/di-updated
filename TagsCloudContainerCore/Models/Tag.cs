using TagsCloudContainerCore.Models.Graphics;

namespace TagsCloudContainerCore.Models;

public class Tag
{
    public int FontSize;
    public required Color Color;
    public required Rectangle BBox;
    public required string Text;
}