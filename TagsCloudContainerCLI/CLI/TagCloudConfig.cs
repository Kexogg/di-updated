namespace TagsCloudContainerCLI.CLI;

public class TagCloudConfig
{
    public string FontFamily { get; set; } = "Arial";
    public float RenderScale { get; set; } = 1.0f;
    public int MaxWords { get; set; } = 100;
    public float MinFontSize { get; set; } = 12.0f;
    public float MaxFontSize { get; set; } = 48.0f;
    public float LayoutSpacing { get; set; } = 1.0f;
    public string[] ExcludedWords { get; set; } = [];
    public string[] ExcludedPartsOfSpeech { get; set; } = ["PART", "ADV", "PR", "CONJ"];
}