using CMS.Data;
using CMS.Seed;
using CMS.Services;
using Microsoft.AspNetCore.Identity;

namespace CMS.Extensions
{
    public static class WebAppExtension
    {

        public static async void SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {

                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var registerService = scope.ServiceProvider.GetRequiredService<ICreateUserService>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                try
                {
                    await SeedWithoutWebsites.InitAsync(context, registerService, roleManager);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

    }
}
