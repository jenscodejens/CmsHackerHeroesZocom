using CMS.Data;
using CMS.Seed;
using CMS.Services;

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


                try
                {
                    await SeedData.InitAsync(context, registerService);
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
