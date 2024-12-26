using TagsCloudContainerCore.Models;
using TagsCloudContainerCore.Models.Graphics;

namespace TagsCloudContainerCore.Layouter;

public class CircularCloudLayouter : ILayouter
{
    private readonly double _step;
    private readonly List<Rectangle> _rectangles = new();
    private double _angle;
    private readonly float _maxFontSize;
    private readonly float _minFontSize;
    private readonly Font _font;

    public CircularCloudLayouter(double spiralStep, Font font, float minFontSize, float maxFontSize, double angle)
    {
        _step = spiralStep;
        _font = font;
        _minFontSize = minFontSize;
        _maxFontSize = maxFontSize;
        _angle = angle;
    }

    public Tag[] LayoutTags(Dictionary<string, double> words)
    {
        if (words.Count == 0)
            return [];
        var minWeight = words.Values.Min();
        var maxWeight = words.Values.Max();
        
        return words.Select(word =>
        {
            var adjustedFontSize = GetAdjustedFontSize(word.Value, minWeight, maxWeight);
            return PutNextTag(word, adjustedFontSize);
        }).ToArray();
    }

    private float GetAdjustedFontSize(double wordValue, double minWeight, double maxWeight)
    {
        if (maxWeight == minWeight)
            return _minFontSize;
        var adjustedFontSize = (float)(_minFontSize + (_maxFontSize - _minFontSize) * (wordValue - minWeight) / (maxWeight - minWeight));
        return adjustedFontSize;
    }


    private Tag PutNextTag(KeyValuePair<string, double> word, float adjustedFontSize)
    {
        _font.Size = adjustedFontSize;
        var rectangleSize = new Size(_font.MeasureText(word.Key), adjustedFontSize);
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
        return new Tag
        {
            Text = word.Key,
            FontSize = _font.Size,
            Color = new Color(0, 0, 0),
            BBox = rectangle
        };
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