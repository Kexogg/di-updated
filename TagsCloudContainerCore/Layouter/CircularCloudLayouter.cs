using TagsCloudContainerCore.Models;

namespace TagsCloudContainerCore.Layouter;

public class CircularCloudLayouter : ILayouter
{
    private const double Step = 0.1;
    private readonly List<Rectangle> _rectangles = new();
    private double _angle;
    private Point _center;
    

    public void SetCenter(Point center)
    {
        _rectangles.Clear();
        _center = center;
        _angle = 0;
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
        var radius = Step * _angle;
        var x = (float)(_center.X + radius * Math.Cos(_angle));
        var y = (float)(_center.Y + radius * Math.Sin(_angle));

        _angle += Step;

        return new Point(x, y);
    }
}