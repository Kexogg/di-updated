using CommandLine;

namespace TagsCloudContainerCLI.CLI;

public class CliOptions
{
    private static readonly TagCloudConfig DefaultConfig = new TagCloudConfig();

    [Option('d', "demo", Required = false, HelpText = "Run the application in demo mode.")]
    public bool Demo { get; set; }

    [Option('f', "file", Required = false, HelpText = "Specify the file path.")]
    public string File { get; set; }
    
    [Option('o', "output", Required = false, HelpText = "Specify the output path.")]
    public string? Output { get; set; }

    [Option("font", Required = false, HelpText = "Specify the font family.")]
    public string FontFamily { get; set; } = DefaultConfig.FontFamily;

    [Option("scale", Required = false, HelpText = "Specify the render scale.")]
    public float RenderScale { get; set; } = DefaultConfig.RenderScale;

    [Option("max-words", Required = false, HelpText = "Maximum number of words to include.")]
    public int MaxWords { get; set; } = DefaultConfig.MaxWords;

    [Option("min-font", Required = false, HelpText = "Minimum font size.")]
    public float MinFontSize { get; set; } = DefaultConfig.MinFontSize;

    [Option("max-font", Required = false, HelpText = "Maximum font size.")]
    public float MaxFontSize { get; set; } = DefaultConfig.MaxFontSize;

    [Option("spacing", Required = false, HelpText = "Space between words.")]
    public float LayoutSpacing { get; set; } = DefaultConfig.LayoutSpacing;
    
    [Option("radius", Required = false, HelpText = "Initial radius from center.")]
    public double InitialRadius { get; set; } = DefaultConfig.InitialRadius;

    [Option("excluded-words", Required = false, HelpText = "Words to exclude.")]
    public string ExcludedWords { get; set; } = string.Join(",", DefaultConfig.ExcludedWords);

    [Option("excluded-parts", Required = false, HelpText = "Parts of speech to exclude.")]
    public string ExcludedPartsOfSpeech { get; set; } = string.Join(",", DefaultConfig.ExcludedPartsOfSpeech);}
    