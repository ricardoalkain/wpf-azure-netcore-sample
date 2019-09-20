using System;
using Microsoft.ApplicationInsights;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using TTMS.Common.Abstractions;
using TTMS.Common.Insights;
using TTMS.Common.Logging;
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

            Container.RegisterSerilog("TTMS.local", Settings.Default.LogLevel, Settings.Default.LogFile);

            Container.RegisterTelemetry(Settings.Default.InstrumentationKey);

            Container.RegisterType<ITravelerWriter, TravelerHttpWriter>(
                new InjectionConstructor(typeof(MEL.ILogger), Settings.Default.ApiUrl));

            Container.RegisterSingleton<IMessageConsumer, TravelerConsumer>(
                new InjectionConstructor(typeof(MEL.ILogger), typeof(TelemetryClient), messageConfig, typeof(ITravelerWriter)));
        }

        public static IUnityContainer Container { get; }
    }
}
