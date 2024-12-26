using TagsCloudContainerCore.Models.Graphics;

namespace TagsCloudContainerCore.Layouter;

public class CircularCloudLayouterFactory : ILayouterFactory
{
    public double SpiralStep { get; set; } = 0.1;
    public Font Font { get; set; } = new("Arial");
    public float MinFontSize { get; set; } = 12.0f;
    public float MaxFontSize { get; set; } = 48.0f;
    public double InitialRadius { get; set; } = 0;
    public ILayouter Create()
    {
        return new CircularCloudLayouter(SpiralStep, Font, MinFontSize, MaxFontSize,  InitialRadius / SpiralStep);
    }
}