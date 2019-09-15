using TTMS.Common.Abstractions;
using TTMS.UI.Services;
using Unity;
using Unity.Injection;

namespace TTMS.UI.Helpers
{
    class DependencyManager
    {
        static DependencyManager()
        {
            Container = new UnityContainer();

            Container.RegisterType<ITravelerService, TravelerService>(
                new InjectionConstructor(
                    Properties.Settings.Default.ApiUrl
                ));

            //Container.RegisterInstance<ITravelerService>(new TravelerHttpService(Properties.Settings.Default.ApiUrl));
        }

        public static IUnityContainer Container { get; }
    }
}
