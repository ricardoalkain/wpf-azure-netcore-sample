using System;
using System.IO;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Extensions.Logging;
using Topshelf;
using TTMS.Common.Abstractions;
using TTMS.Messaging.Config;
using TTMS.Messaging.Consumers;
using TTMS.Web.Client;

namespace TTMS.ConsumerService.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var logConfig = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("logsettings.json", optional: true)
                                .AddJsonFile($"logsettings.{env}.json", optional: true)
                                .Build();

            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(logConfig)
                        .CreateLogger();

            try
            {
                var configuration = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json", optional: true)
                                    .AddJsonFile($"appsettings.{env}.json", optional: true)
                                    .Build();

                var serviceBusConnection = configuration["MessagingConfig:ServerConnection"];
                var incomingQueueName = configuration["MessagingConfig:IncomingQueue"];

                var services = new ServiceCollection();
                services.AddSingleton<IConfiguration>(configuration);
                services.AddSingleton(new SerilogLoggerProvider(Log.Logger).CreateLogger("TTMS.ConsumerService.Core"));
                services.AddSingleton<IQueueClient>(new QueueClient(serviceBusConnection, incomingQueueName));
                services.AddSingleton<ITravelerWriter, TravelerHttpWriter>();
                services.AddSingleton<IMessageConsumer, TravelerConsumer>();

                var provider = services.BuildServiceProvider();

                var winService = HostFactory.Run(config =>
                {
                    config.Service<IMessageConsumer>(service =>
                    {
                        service.ConstructUsing(factory => provider.GetService<IMessageConsumer>());
                        service.WhenStarted(s => s.StartListening());
                        service.WhenStopped(s => s.Dispose());
                    });
                    config.RunAsLocalSystem();

                    config.SetServiceName("TTMSLocalConsumer");
                    config.SetDisplayName("TTMS Message Consumer");
                    config.SetDescription("This service monitors incoming messages from TTMS queue and process them.");
                });

                var exitCode = (int)Convert.ChangeType(winService, winService.GetTypeCode());
                Environment.ExitCode = exitCode;

                Log.Logger.Information("Application shut down.");
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(ex, "An unhandled exception stopped the application.");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

    }
}
