using TagsCloudContainerCore.Models.Graphics;

namespace TagsCloudContainerCore.Layouter;

public class CircularCloudLayouter : ILayouter
{
    private readonly double _step;
    private readonly List<Rectangle> _rectangles = new();
    private double _angle;
    

    public CircularCloudLayouter(double step)
    {
        _step = step;
    }
    
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Rectangle size must be positive", nameof(rectangleSize));

        Rectangle rectangle;

        do
        {
            var centerOfRectangle = GetNextPosition();
            var rectanglePosition = new Point(centerOfRectangle.X - rectangleSize.Width / 2,
                centerOfRectangle.Y - rectangleSize.Height / 2);

            rectangle = new Rectangle(
                rectanglePosition.X,
                rectanglePosition.Y,
                rectanglePosition.X + rectangleSize.Width,
                rectanglePosition.Y + rectangleSize.Height);
        } while (_rectangles.Any(r => r.IntersectsWith(rectangle)));

        _rectangles.Add(rectangle);
        return rectangle;
    }
    

    public IReadOnlyList<Rectangle> Rectangles => _rectangles.AsReadOnly();

    private Point GetNextPosition()
    {
        var radius = _step * _angle;
        var x = (float)(radius * Math.Cos(_angle));
        var y = (float)(radius * Math.Sin(_angle));

        _angle += _step;

        return new Point(x, y);
    }
}