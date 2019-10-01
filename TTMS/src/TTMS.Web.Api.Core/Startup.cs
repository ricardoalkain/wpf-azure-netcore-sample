using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using TTMS.Common.Abstractions;
using TTMS.Data.Abstractions;
using TTMS.Data.Azure;
using TTMS.Web.Api.Core.Services;

namespace TTMS.Web.Api.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IAzureCloudFactory, AzureCloudFactory>();
            services.AddSingleton<ITravelerReader, TravelerTableReader>();
            services.AddSingleton<ITravelerWriter, TravelerTableWriter>();
            services.AddTransient<ITravelerDbService, TravelerDbService>();

            services.AddApplicationInsightsTelemetry();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("beta", new Info { Title = "TTMS v0.1 beta API", Version = "v0.1" });
                config.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                config.DescribeAllEnumsAsStrings();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                config.IncludeXmlComments(xmlFile);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/beta/swagger.json", "");
            });

            app.UseSerilogRequestLogging(); // Better logging than ASP.NET Core

            app.UseMvc();
        }
    }
}
