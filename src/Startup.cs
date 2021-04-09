using Conesoft.Hosting;
using Conesoft.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Davepermen.Website.Fitness
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddUsers("Conesoft Host", Host.GlobalStorage / "Users");
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseUsers();

            app.UseRouting();

            app.UseHostingDefaults(useDefaultFiles: true, useStaticFiles: true);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
