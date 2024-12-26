using System.ComponentModel;
using System.Diagnostics;
using TagsCloudContainerCore.Models;

namespace TagsCloudContainerCore.TextProcessor.MyStem;

public class MyStemWrapper : IDisposable
{
    private string PathToBinary { get; set; } = 
        Environment.OSVersion.Platform == PlatformID.Win32NT ? "Mystem/mystem.exe" : "Mystem/mystem";
    private Process _process;
    private StreamWriter _inputWriter;
    private StreamReader _outputReader;

    public void StartProcess(string arguments = "-in")
    {
        var psi = new ProcessStartInfo
        {
            FileName = PathToBinary,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        var process = Process.Start(psi);
        _process = process ?? throw new InvalidEnumArgumentException("MyStem process failed to start");
        _inputWriter = _process.StandardInput;
        _outputReader = _process.StandardOutput;
    }
    

    public MyStemProcessedWord? ProcessWord(string word)
    {
        _inputWriter.WriteLine(word);
        _inputWriter.Flush();
        var line = _outputReader.ReadLine();
        if (line == null)
        {
            throw new InvalidEnumArgumentException("MyStem returned null");
        }

        return ParseResult(line);
    }

    private void StopProcess()
    {
        _inputWriter.Close();
        _outputReader.Close();
        _process.WaitForExit();
        _process.Dispose();
    }

    public void Dispose()
    {
        StopProcess();
    }


    //TODO: оптипизировать 
    private static MyStemProcessedWord? ParseResult(string raw)
    {
        // Пример парса: "сделал{сделать=V,сов,пе=прош,ед,изъяв,муж}"
        // Пример ошибки: ъ{ъ??}
        var startIndex = raw.IndexOf('{') + 1;
        var endIndex = raw.IndexOf('}');
        var metadata = raw.Substring(startIndex, endIndex - startIndex).Split(',');

        if (metadata.Length == 0 || !metadata[0].Contains('=') || metadata[0].Contains('{'))
        {
            return null;
        }

        var rootForm = metadata[0].Split('=')[0];
        if (rootForm.Contains('?'))
        {
            rootForm = rootForm.Remove(rootForm.IndexOf('?'));
        }
        var partOfSpeech = metadata[0].Split('=')[1];

        if (Enum.TryParse(partOfSpeech, out PartOfSpeech pos))
        {
            return new MyStemProcessedWord(rootForm, pos);
        }

        throw new ArgumentException("Invalid part of speech in raw string");
    }
}

