using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using Serilog;

namespace SocialMedia.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configura Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug() // O el nivel que desees
                .WriteTo.Console()
                .WriteTo.File("logs/SocialMedia.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                //Enable postgress datetime.now
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog() // Usa Serilog
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
