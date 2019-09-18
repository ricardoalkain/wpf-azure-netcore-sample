using TTMS.Common.Abstractions;
using TTMS.ConsumerService.Properties;
using TTMS.Messaging.Config;
using TTMS.Messaging.Consumers;
using TTMS.Web.Client;
using Unity;
using Unity.Injection;

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

            Container.RegisterType<ITravelerWriter, TravelerHttpWriter>(new InjectionConstructor(Settings.Default.ApiUrl));
            Container.RegisterType<IMessageConsumer, TravelerConsumer>(new InjectionConstructor(messageConfig, typeof(ITravelerWriter)));
        }

        public static IUnityContainer Container { get; }
    }
}
