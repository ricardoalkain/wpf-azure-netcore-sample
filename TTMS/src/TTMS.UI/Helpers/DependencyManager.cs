using System;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using TTMS.Common.Abstractions;
using TTMS.Messaging;
using TTMS.Messaging.Config;
using TTMS.Messaging.Producers;
using TTMS.UI.Properties;
using TTMS.UI.Services;
using TTMS.Web.Client;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using MEL = Microsoft.Extensions.Logging;

namespace TTMS.UI.Helpers
{
    static class DependencyManager
    {
        static DependencyManager()
        {
            Container = new UnityContainer();

            var apiUrl = Settings.Default.ApiUrl;
            var msgConfig = new MessagingConfig
            {
                ServerConnection = Settings.Default.MessageBusConnection,
                OutgoingQueue = Settings.Default.OutgoingMessageQueue
            };

            RegisterLogger();

            Container.RegisterType(typeof(IMessageProducer<>), typeof(RabbitMqProducer<>),
                new SingletonLifetimeManager(), new InjectionConstructor(typeof(MEL.ILogger), msgConfig)); // Pubilhes to RabbitMQ

            Container.RegisterType<ITravelerReader, TravelerHttpReader>(
                new InjectionConstructor(typeof(MEL.ILogger), apiUrl)); // Read from API

            Container.RegisterType<ITravelerWriter, TravelerMessageWriter>(); // Write to Message Bus

            Container.RegisterType<ITravelerService, TravelerService>();
        }

        public static IUnityContainer Container { get; }
        private static void RegisterLogger()
        {
            var logFolder = Settings.Default.LogFile;
            if (!Enum.TryParse(Settings.Default.LogLevel, out LogEventLevel logLevel))
            {
                logLevel = LogEventLevel.Debug;
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(logLevel)
                .WriteTo.Console()
                .WriteTo.RollingFile(logFolder)
                .CreateLogger();

            var ilogger = new SerilogLoggerProvider(Log.Logger).CreateLogger("TTMS.UI");

            Container.RegisterInstance(ilogger);
        }
    }
}
