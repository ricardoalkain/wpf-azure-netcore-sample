using System;
using Topshelf;
using TTMS.Messaging.Consumers;
using Unity;

namespace TTMS.ConsumerService
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {

                x.Service<IMessageConsumer>(service =>
                {
                    service.ConstructUsing(s => DependencyManager.Container.Resolve<IMessageConsumer>());
                    service.WhenStarted(s => s.StartListening());
                    service.WhenStopped(s => s.Dispose());
                });

                x.RunAsLocalSystem();

                x.SetServiceName("TTMSLocalConsumer");
                x.SetDisplayName("TTMS Message Consumer");
                x.SetDescription("This service monitors incoming messages from TTMS queue and process them.");
            });

            var exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
