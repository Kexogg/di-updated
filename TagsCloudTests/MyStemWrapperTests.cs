using TagsCloudContainerCore.Models;
using TagsCloudContainerCore.TextProcessor.MyStem;

namespace TagsCloudTests;

public class MyStemWrapperTests
{
    private readonly MyStemWrapper _myStemWrapper = new();
    
    [SetUp]
    public void SetUp()
    {
        _myStemWrapper.StartProcess();
    }
    
    [TearDown]
    public void TearDown()
    {
        _myStemWrapper.Dispose();
    }
    
    [Test]
    public void MyStemWrapper_ShouldProcessWord()
    {
        var processedWord = _myStemWrapper.ProcessWord("слово");
        processedWord.NormalForm.Should().Be("слово");
        processedWord.PartOfSpeech.Should().Be(PartOfSpeech.S);
    }
    
    [Test]
    public void MyStemWrapper_ShouldProcessWord_WithPunctuation()
    {
        var processedWord = _myStemWrapper.ProcessWord("слово,");
        processedWord.NormalForm.Should().Be("слово");
        processedWord.PartOfSpeech.Should().Be(PartOfSpeech.S);
    }
    
    [Test]
    public void MyStemWrapper_ShouldProcessWord_InDifferentForms()
    {
        var processedWord = _myStemWrapper.ProcessWord("слова");
        processedWord.NormalForm.Should().Be("слово");
        processedWord.PartOfSpeech.Should().Be(PartOfSpeech.S);
    }
    
    [Test]
    public void MyStemWrapper_ShouldProcessWord_WithDifferentPartOfSpeech()
    {
        var processedWord = _myStemWrapper.ProcessWord("говорю");
        processedWord.NormalForm.Should().Be("говорить");
        processedWord.PartOfSpeech.Should().Be(PartOfSpeech.V);
    }
    
    [Test]
    public void MyStemWrapper_ShouldProcessMultipleWords()
    {
        var processedWord = _myStemWrapper.ProcessWord("слово");
        processedWord.NormalForm.Should().Be("слово");
        processedWord.PartOfSpeech.Should().Be(PartOfSpeech.S);
        
        processedWord = _myStemWrapper.ProcessWord("слова");
        processedWord.NormalForm.Should().Be("слово");
        processedWord.PartOfSpeech.Should().Be(PartOfSpeech.S);
        
        processedWord = _myStemWrapper.ProcessWord("говорю");
        processedWord.NormalForm.Should().Be("говорить");
        processedWord.PartOfSpeech.Should().Be(PartOfSpeech.V);
        
        processedWord = _myStemWrapper.ProcessWord("говоря");
        processedWord.NormalForm.Should().Be("говорить");
        processedWord.PartOfSpeech.Should().Be(PartOfSpeech.V);
    }
}