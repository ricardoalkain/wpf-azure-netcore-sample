using TTMS.Data.Repositories;
using TTMS.Data.Services;
using Unity;

namespace TTMS.UI.Helpers
{
    class DependencyManager
    {
        static DependencyManager()
        {
            Container = new UnityContainer();
            Container.RegisterInstance<ITravelerRepository>(new TravelerFileRepository(Properties.Settings.Default.DataSource));
            Container.RegisterSingleton<ITravelerService, TravelerService>();
        }

        public static IUnityContainer Container { get; }
    }
}
