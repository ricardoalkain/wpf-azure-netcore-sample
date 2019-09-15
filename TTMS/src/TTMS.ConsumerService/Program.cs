using System;
using Topshelf;
using Unity;

namespace TTMS.ConsumerService
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {

                x.Service<IListener>(service =>
                {
                    service.ConstructUsing(s => DependencyManager.Container.Resolve<IListener>());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
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
