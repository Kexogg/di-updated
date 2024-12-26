using Microsoft.Extensions.Logging;
using Moq;
using TagsCloudContainerCore.Models;
using TagsCloudContainerCore.Models.Graphics;
using TagsCloudContainerCore.Renderer;

namespace TagsCloudTests;

[TestFixture]
public class RendererTests
{
    private Mock<ILogger<IRenderer>> _loggerMock;
    private Renderer _renderer;
    private readonly Color _defaultColor = new(0, 0, 0);

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<IRenderer>>();
        _renderer = new Renderer(_loggerMock.Object);
    }

    [Test]
    public void RenderingScale_ShouldThrowException_WhenSetToZeroOrNegative()
    {
        var zeroFunc = () => _renderer.RenderingScale = 0;
        var negativeFunc = () => _renderer.RenderingScale = -1;

        zeroFunc.Should().Throw<ArgumentOutOfRangeException>();
        negativeFunc.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void DrawTags_WithEmptyCollection_ShouldReturnEmptyImage()
    {
        var tags = Array.Empty<Tag>();

        var image = _renderer.DrawTags(tags);

        image.Width.Should().Be(1);
        image.Height.Should().Be(1);
    }

    [Test]
    public void DrawTags_ShouldReturnCorrectImage_WhenSingleTag()
    {
        var tag = new Tag
        {
            Text = "",
            FontSize = 12,
            Color = _defaultColor,
            BBox = new Rectangle(0, 0, 100, 20)
        };

        var image = _renderer.DrawTags([tag]);

        image.Width.Should().Be(100);
        image.Height.Should().Be(20);
    }

    [Test]
    public void DrawTags_ShouldReturnScaledImage()
    {
        var tag = new Tag
        {
            Text = "",
            FontSize = 12,
            Color = _defaultColor,
            BBox = new Rectangle(0, 0, 100, 20)
        };
        _renderer.RenderingScale = 2;

        var image = _renderer.DrawTags([tag]);

        image.Width.Should().Be(200);
        image.Height.Should().Be(40);
    }

    [Test]
    public void DrawTags_ShouldCalculateCorrectBounds_WithMultipleTags()
    {
        var tags = new[]
        {
            new Tag { Text = "", FontSize = 12, Color = _defaultColor, BBox = new Rectangle(0, 0, 50, 20) },
            new Tag { Text = "", FontSize = 12, Color = _defaultColor, BBox = new Rectangle(60, 0, 110, 20) }
        };

        var image = _renderer.DrawTags(tags);

        image.Width.Should().Be(110);
        image.Height.Should().Be(20);
    }

    [Test]
    public void DrawTags_ShouldReturnImage_WhenTagsHaveNegativeCoordinates()
    {
        var tag = new Tag
        {
            Text = "",
            FontSize = 12, 
            Color = _defaultColor,
            BBox = new Rectangle(-50, -50, 50, 50)
        };

        var image = _renderer.DrawTags([tag]);

        image.Width.Should().Be(100);
        image.Height.Should().Be(100);
    }
}