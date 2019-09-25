using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace TTMS.Web.Api.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure log/telemtry first to catch every information possible
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var logConfig = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("logsettings.json", optional: true)
                                .AddJsonFile($"logsettings.{env}.json", optional: true)
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
                .UseSerilog()
                .UseStartup<Startup>();
    }
}
