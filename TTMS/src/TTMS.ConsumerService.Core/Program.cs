﻿using System;
using System.IO;
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
            var logConfig = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("logsettings.json")
                                .AddJsonFile("logsettings.Development.json", optional: true)
                                .Build();

            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(logConfig)
                        .CreateLogger();

            try
            {
                var configuration = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .AddJsonFile("appsettings.Development.json", optional: true)
                                    .Build();

                var services = new ServiceCollection();
                services.AddSingleton<IConfiguration>(configuration);
                services.AddSingleton(configuration.GetSection("MessagingConfig").Get<MessagingConfig>());
                services.AddSingleton(new SerilogLoggerProvider(Log.Logger).CreateLogger("TTMS.ConsumerService.Core"));
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