namespace TagsCloudContainerCore.Layouter;

public class CircularClouldLayouterFactory : ILayouterFactory
{
    public ILayouter Create()
    {
        return new CircularCloudLayouter();
    }
}