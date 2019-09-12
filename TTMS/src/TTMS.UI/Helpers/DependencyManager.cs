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
            Container.RegisterInstance<ITravelerRepository>(new TravelerSqlRepository(Properties.Settings.Default.DBConnectionStr));
            Container.RegisterSingleton<ITravelerService, TravelerService>();
        }

        public static IUnityContainer Container { get; }
    }
}
