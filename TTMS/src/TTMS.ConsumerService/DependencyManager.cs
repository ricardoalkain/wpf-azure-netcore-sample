using System;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using TTMS.Common.Abstractions;
using TTMS.ConsumerService.Properties;
using TTMS.Messaging.Config;
using TTMS.Messaging.Consumers;
using TTMS.Web.Client;
using Unity;
using Unity.Injection;
using MEL = Microsoft.Extensions.Logging;

namespace TTMS.ConsumerService
{
    class DependencyManager
    {
        static DependencyManager()
        {
            Container = new UnityContainer();

            var messageConfig = new MessagingConfig
            {
                ServerConnection = Settings.Default.MessageBusConnection,
                IncomingQueue = Settings.Default.IncomingMessageQueue
            };

            RegisterLogger();

            Container.RegisterType<ITravelerWriter, TravelerHttpWriter>(new InjectionConstructor(typeof(MEL.ILogger), Settings.Default.ApiUrl));
            Container.RegisterType<IMessageConsumer, TravelerConsumer>(new InjectionConstructor(typeof(MEL.ILogger), messageConfig, typeof(ITravelerWriter)));
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

            var ilogger = new SerilogLoggerProvider(Log.Logger).CreateLogger("TTMS.Local");

            Container.RegisterInstance(ilogger);
        }
    }
}
