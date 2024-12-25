using Microsoft.Extensions.Logging;
using Moq;
using TagsCloudContainerCore.Models;
using TagsCloudContainerCore.TextProcessor;

namespace TagsCloudTests;

public class MyStemWordProcessorTests
{
    private readonly MyStemWordProcessor _myStemWordProcessor;
    
    public MyStemWordProcessorTests()
    {
        var mockLogger = new Mock<ILogger<MyStemWordProcessor>>();
        _myStemWordProcessor = new MyStemWordProcessor(mockLogger.Object);
    }

    [Test]
    public void MyStemWordProcessor_ShouldProcessWord()
    {
        var result = _myStemWordProcessor.ProcessText("слово", new WordProcessorOptions([], []));

        result.Should().ContainSingle().Which.Should().Be(new KeyValuePair<string, double>("слово", 1));
    }

    [Test]
    public void MyStemWordProcessor_ShouldProcessMultipleWords()
    {
        var result = _myStemWordProcessor.ProcessText("слово слова", new WordProcessorOptions([], []));

        result.Should().HaveCount(1);
        result.Should().ContainSingle(pair => pair.Key == "слово" && pair.Value == 2);
    }

    [Test]
    public void MyStemWordProcessor_ShouldProcessWordWithExcludedPartOfSpeech()
    {
        var result = _myStemWordProcessor.ProcessText("слово", new WordProcessorOptions([PartOfSpeech.S], []));

        result.Should().BeEmpty();
    }

    [Test]
    public void MyStemWordProcessor_ShouldProcessWordWithExcludedWord()
    {
        var result = _myStemWordProcessor.ProcessText("слово", new WordProcessorOptions([], ["слово"]));

        result.Should().BeEmpty();
    }

    [Test]
    public void MyStemWordProcessor_ShouldProcessLongText()
    {
        var result = _myStemWordProcessor.ProcessText("Это текст, который нужно обработать. Текст, текст, текст.",
            new WordProcessorOptions([], []));

        result.Should().HaveCount(5);
        result.Should().Contain(pair => pair.Key == "это" && pair.Value == 1);
        result.Should().Contain(pair => pair.Key == "текст" && pair.Value == 4);
        result.Should().Contain(pair => pair.Key == "который" && pair.Value == 1);
        result.Should().Contain(pair => pair.Key == "нужно" && pair.Value == 1);
        result.Should().Contain(pair => pair.Key == "обрабатывать" && pair.Value == 1);
    }
}