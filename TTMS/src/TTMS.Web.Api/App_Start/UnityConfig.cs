using System.Configuration;
using System.Web.Http;
using TTMS.Common.Abstractions;
using TTMS.Data.Repositories;
using TTMS.Data.Services;
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

            container.RegisterType<ITravelerRepository, TravelerSqlRepository>(new InjectionConstructor(dbConnectionStr));
            container.RegisterType<ITravelerService, TravelerService>();
            container.RegisterType<ITravelerApiService, TravelerApiService>();


            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}