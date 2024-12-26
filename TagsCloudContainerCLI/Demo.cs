using System.Text;
using TagsCloudContainerCore.DataProvider;
using TagsCloudContainerCore.Facade;
using TagsCloudContainerCore.ImageEncoders;
using TagsCloudContainerCore.Layouter;
using TagsCloudContainerCore.Models.Graphics;
using TagsCloudContainerCore.Renderer;
using TagsCloudContainerCore.TextProcessor;

namespace TagsCloudContainerCLI;

public class Demo
{
    private ITagCloudFactory _cloudFactory;

    public Demo(ITagCloudFactory cloudFactory)
    {
        _cloudFactory = cloudFactory;
    }

    public void GenerateDemo()
    {
        Directory.CreateDirectory("results");

        GenerateRandomCloud(10);
        GenerateRandomCloud(50);
        GenerateRandomCloud(100);
    }

    private void GenerateRandomCloud(int count)
    {
        var tagCloud = _cloudFactory.Create(builder => builder
            .UseWordProcessor<MyStemTextProcessor>(p =>
            {
                p.ExcludedWords = ["тест"];
                p.ExcludedPartsOfSpeech = [];
            })
            .UseLayouter<CircularCloudLayouterFactory>(p => { p.SpiralStep = 0.5; })
            .UseRenderer<Renderer>(r =>
            {
                r.BackgroundColor = new Color(200, 200, 255);
                r.RenderingScale = 5;
                r.TextColor = new Color(0, 0, 100);
            })
            .UseImageEncoder<PngEncoder>());

        var imageBytes = tagCloud.FromString(GenerateRandomString(count));

        File.WriteAllBytes($"results/random_cloud_{count}", imageBytes);
    }

    private static string GenerateRandomString(int count)
    {
        var randomString =
            "Lorem ipsum — классический текст-«рыба» (условный, зачастую бессмысленный текст-заполнитель, вставляемый в макет страницы, используемый для образца шрифта и текста, поля размещения на странице). "
                .Split();
        var random = new Random();
        var sb = new StringBuilder();
        for (var i = 0; i < count; i++)
        {
            sb.Append(randomString[random.Next(randomString.Length)]);
            sb.Append(' ');
        }

        return sb.ToString();
    }
}