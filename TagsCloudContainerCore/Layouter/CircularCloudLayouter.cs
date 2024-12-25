using SkiaSharp;

namespace TagsCloudContainerCore.Layouter;

public class CircularCloudLayouter : ICircularCloudLayouter
{
    private const double Step = 0.1;
    private readonly List<SKRect> _rectangles = new List<SKRect>();
    private double _angle;
    private SKPoint _center;
    

    public void SetCenter(SKPoint center)
    {
        _rectangles.Clear();
        _center = center;
        _angle = 0;
    }

    public SKRect PutNextRectangle(SKSize rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Rectangle size must be positive", nameof(rectangleSize));

        SKRect rectangle;

        do
        {
            var centerOfRectangle = GetNextPosition();
            var rectanglePosition = new SKPoint(centerOfRectangle.X - rectangleSize.Width / 2,
                centerOfRectangle.Y - rectangleSize.Height / 2);

            rectangle = new SKRect(
                rectanglePosition.X,
                rectanglePosition.Y,
                rectanglePosition.X + rectangleSize.Width,
                rectanglePosition.Y + rectangleSize.Height);
        } while (_rectangles.Any(r => r.IntersectsWith(rectangle)));

        _rectangles.Add(rectangle);
        return rectangle;
    }

    public IReadOnlyList<SKRect> Rectangles => _rectangles.AsReadOnly();

    private SKPoint GetNextPosition()
    {
        var radius = Step * _angle;
        var x = (float)(_center.X + radius * Math.Cos(_angle));
        var y = (float)(_center.Y + radius * Math.Sin(_angle));

        _angle += Step;

        return new SKPoint(x, y);
    }
}