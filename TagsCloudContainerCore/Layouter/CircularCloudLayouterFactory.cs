namespace TagsCloudContainerCore.Layouter;

public class CircularCloudLayouterFactory : ILayouterFactory
{
    public double SpiralStep { get; set; } = 0.1;
    public ILayouter Create()
    {
        return new CircularCloudLayouter(SpiralStep);
    }
}