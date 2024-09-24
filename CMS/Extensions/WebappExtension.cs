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
                    // Ändra till: await SeedData.InitAsync(context, registerService, roleManager);
                    // Om du vill ha tidigare seed med mock users/websites/webpages
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
