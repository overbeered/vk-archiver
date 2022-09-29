using Overbeered.VkArchiver.Core.Services;
using Overbeered.VkArchiver.Services.AuthService.Middlewares;
using Overbeered.VkArchiver.Services.AuthService.Services;
using Overbeered.VkArchiver.Services.VkArchiverService;

namespace Overbeered.VkArchiver.MVC;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
        services.AddTransient<IVkArchiver, VkArchiver>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IVkArchiverService, VkArchiverService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();
        app.UseMiddleware<JwtMiddleware>();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}