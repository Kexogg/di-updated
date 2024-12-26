using Autofac;
using Autofac.Extensions.DependencyInjection;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Autofac.DependencyInjection;
using TagsCloudContainerCLI.CLI;
using TagsCloudContainerCore.Facade;

namespace TagsCloudContainerCLI;

internal class Program
{
    private static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureServices((_, services) =>
            {
                services.AddOptions();
                services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
            })
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterSerilog(new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .WriteTo.File("logs/log-.log", rollingInterval: RollingInterval.Day));

                builder.RegisterType<TagCloudFactory>()
                    .As<ITagCloudFactory>()
                    .InstancePerLifetimeScope();
                builder.RegisterType<FileMode>().AsSelf();
                builder.RegisterType<Demo>().AsSelf().SingleInstance();
            })
            .Build();

        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;
        try
        {
            var cliHandler = new CliHandler(services.GetRequiredService<ILogger<CliHandler>>());
            Parser.Default.ParseArguments<CliOptions>(args)
                .WithParsed(opts =>
                {
                    cliHandler.RunOptionsAndReturnExitCode(opts);
                    if (opts.Demo)
                    {
                        var demo = scope.ServiceProvider.GetRequiredService<Demo>();
                        demo.GenerateDemo();
                    }
                    else
                    {
                        var fileMode = scope.ServiceProvider.GetRequiredService<FileMode>();
                        fileMode.Generate(
                            "testFile.docx",
                            "results/cloud.png"
                        );
                    }
                })
                .WithNotParsed(cliHandler.HandleParseError);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred.");
        }
    }
}