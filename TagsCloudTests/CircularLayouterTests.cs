using TagsCloudContainerCore.Layouter;
using TagsCloudContainerCore.Models.Graphics;

namespace TagsCloudTests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private Font _font;
    private const float MinFontSize = 12f;
    private const float MaxFontSize = 48f;
    private const double InitialAngle = 0;
    private const double SpiralStep = 0.1;

    [SetUp]
    public void SetUp()
    {
        _font = new Font("Arial", 12f);
    }
    
    [TearDown]
    public void TearDown()
    {
        _font.Dispose();
    }

    [Test]
    public void LayoutTags_ReturnsEmptyArray_WhenEmptyDictionary()
    {
        var layouter = new CircularCloudLayouter(SpiralStep, _font, MinFontSize, MaxFontSize, InitialAngle);
        var result = layouter.LayoutTags(new Dictionary<string, double>());
        result.Should().BeEmpty();
    }

    [Test]
    public void LayoutTags_ReturnsOneTag_OnSingleWord()
    {
        var layouter = new CircularCloudLayouter(SpiralStep, _font, MinFontSize, MaxFontSize, InitialAngle);
        var words = new Dictionary<string, double> { { "1", 1.0 } };
        
        var result = layouter.LayoutTags(words);
        
        result.Should().HaveCount(1);
        result[0].Text.Should().Be("1");
        result[0].FontSize.Should().Be(MinFontSize);
    }

    [Test]
    public void LayoutTags_ReturnsTagsThatDontOverlap()
    {
        var layouter = new CircularCloudLayouter(SpiralStep, _font, MinFontSize, MaxFontSize, InitialAngle);
        var words = new Dictionary<string, double> 
        { 
            { "1", 1.0 },
            { "2", 2.0 },
            { "3", 3.0 }
        };
        
        var result = layouter.LayoutTags(words);
        
        for (var i = 0; i < result.Length; i++)
        {
            for (var j = i + 1; j < result.Length; j++)
            {
                result[i].BBox.IntersectsWith(result[j].BBox).Should().BeFalse();
            }
        }
    }

    [Test]
    public void LayoutTags_DifferentWeights_AffectFontSizeCorrectly()
    {
        var layouter = new CircularCloudLayouter(SpiralStep, _font, MinFontSize, MaxFontSize, InitialAngle);
        var words = new Dictionary<string, double>
        {
            { "1", 1.0 },
            { "2", 2.0 }
        };

        var result = layouter.LayoutTags(words);

        var smallTag = result.First(t => t.Text == "1");
        var largeTag = result.First(t => t.Text == "2");
        smallTag.FontSize.Should().Be(MinFontSize);
        largeTag.FontSize.Should().Be(MaxFontSize);
    }

    [Test]
    public void Rectangles_ReturnsAllRectangles_AfterLayouting()
    {
        var layouter = new CircularCloudLayouter(SpiralStep, _font, MinFontSize, MaxFontSize, InitialAngle);
        var words = new Dictionary<string, double> { { "1", 1.0 }, { "2", 2.0 } };
        
        var tags = layouter.LayoutTags(words);
        
        layouter.Rectangles.Should().HaveCount(2);
        layouter.Rectangles.Should().Contain(tags[0].BBox);
        layouter.Rectangles.Should().Contain(tags[1].BBox);
    }

    [Test]
    public void LayoutTags_UsesMinFontSize_WhenSameWeight()
    {
        var layouter = new CircularCloudLayouter(SpiralStep, _font, MinFontSize, MaxFontSize, InitialAngle);
        var words = new Dictionary<string, double>
        {
            { "1", 1.0 },
            { "2", 1.0 }
        };

        var result = layouter.LayoutTags(words);

        result.Should().AllSatisfy(tag => tag.FontSize.Should().Be(MinFontSize));
    }
}