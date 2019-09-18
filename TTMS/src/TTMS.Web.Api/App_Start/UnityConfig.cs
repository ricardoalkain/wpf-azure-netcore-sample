using System.Configuration;
using System.Web.Http;
using TTMS.Common.Abstractions;
using TTMS.Web.Api.Services;
using Unity;
using Unity.Injection;
using Unity.WebApi;

namespace TTMS.Web.Api
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            var dbConnectionStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

            container.RegisterType<ITravelerReader, TravelerSqlReader>(new InjectionConstructor(dbConnectionStr));
            container.RegisterType<ITravelerWriter, TravelerSqlWriter>(new InjectionConstructor(dbConnectionStr));
            container.RegisterType<ITravelerDbService, TravelerDbService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}