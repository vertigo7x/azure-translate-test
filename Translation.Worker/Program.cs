using Translation.Service.Extensions;
using TranslationWorker;


var configuration = new ConfigurationBuilder()
       .AddEnvironmentVariables()
       .AddCommandLine(args)
       .AddJsonFile("appsettings.json")
       .Build();


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<TranslationJobWorker>();
        services.AddTranslationServices(configuration);
    })
    .Build();

await host.RunAsync();
