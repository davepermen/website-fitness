using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Davepermen.Website.Fitness
{
    using Conesoft.Hosting;
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateHostBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
