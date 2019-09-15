using TTMS.ConsumerService.Properties;
using Unity;
using Unity.Injection;

namespace TTMS.ConsumerService
{
    class DependencyManager
    {
        static DependencyManager()
        {
            Container = new UnityContainer();

            Container.RegisterInstance(Settings.Default);
            //Container.RegisterType<ITravelerService, TravelerHttpService>(
            //    new InjectionConstructor(
            //        Properties.Settings.Default.ApiUrl
            //    ));

            //Container.RegisterInstance<ITravelerService>(new TravelerHttpService(Properties.Settings.Default.ApiUrl));
        }

        public static IUnityContainer Container { get; }
    }
}
