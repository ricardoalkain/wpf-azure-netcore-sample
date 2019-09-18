using TTMS.Common.Abstractions;
using TTMS.Messaging;
using TTMS.Messaging.Config;
using TTMS.UI.Properties;
using TTMS.UI.Services;
using TTMS.Web.Client;
using Unity;
using Unity.Injection;

namespace TTMS.UI.Helpers
{
    class DependencyManager
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

            Container.RegisterType<ITravelerReader,
                TravelerHttpReader>(new InjectionConstructor(apiUrl)); // Read from API
            Container.RegisterType<ITravelerWriter,
                TravelerMessageWriter>(new InjectionConstructor(msgConfig)); // Write to Message Bus

            Container.RegisterType<ITravelerService, TravelerService>();
        }

        public static IUnityContainer Container { get; }
    }
}
