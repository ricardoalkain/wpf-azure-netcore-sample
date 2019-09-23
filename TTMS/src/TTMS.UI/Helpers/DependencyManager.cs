﻿using TTMS.Common.Abstractions;
using TTMS.Common.Logging;
using TTMS.Common.Insights;
using TTMS.Messaging.Config;
using TTMS.Messaging.Producers;
using TTMS.UI.Properties;
using TTMS.UI.Services;
using TTMS.Web.Client;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using MEL = Microsoft.Extensions.Logging;
using Microsoft.ApplicationInsights;

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

            Container.RegisterSerilog("TTMS.UI", Settings.Default.LogLevel, Settings.Default.LogFile);
            Container.RegisterTelemetry(Settings.Default.InstrumentationKey);

            Container.RegisterType(typeof(IMessageProducer<>), typeof(AzureServiceBusProducer<>),
                new SingletonLifetimeManager(), new InjectionConstructor(typeof(MEL.ILogger),
                typeof(TelemetryClient), msgConfig)); // Pubilhes to Service Bus

            Container.RegisterType<ITravelerReader, TravelerHttpReader>(
                new InjectionConstructor(typeof(MEL.ILogger), apiUrl)); // Read from API

            Container.RegisterType<ITravelerWriter, TravelerMessageWriter>(); // Write to Message Bus

            Container.RegisterType<ITravelerService, TravelerService>();
        }

        public static IUnityContainer Container { get; }
    }
}
