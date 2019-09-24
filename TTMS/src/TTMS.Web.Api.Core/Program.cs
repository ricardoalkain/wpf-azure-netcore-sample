using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace TTMS.Web.Api.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logConfig = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("logsettings.json")
                                .AddJsonFile("logsettings.Development.json")
                                .Build();

            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(logConfig)
                        .CreateLogger();

            try
            {
                Log.Logger.Information("Application starting...");

                CreateWebHostBuilder(args).Build().Run();

                Log.Logger.Information("Application shut down.");
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(ex, "An unhandled exception stopped the application.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.UseApplicationInsights()
                .UseSerilog()
                .UseStartup<Startup>();
    }
}
