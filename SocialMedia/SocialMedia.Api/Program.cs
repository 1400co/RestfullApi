using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace SocialMedia.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Enable postgress datetime.now
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
