using CMS.Data;
using Microsoft.AspNetCore.Identity;

namespace CMS.Services
{
    public class CreateUserService: ICreateUserService
    {
        private readonly IUserStore<ApplicationUser> UserStore;
        private readonly UserManager<ApplicationUser> UserManager;

        public CreateUserService(IUserStore<ApplicationUser> userStore, UserManager<ApplicationUser> userManager)
        {
            this.UserManager = userManager;
            this.UserStore = userStore;
        }

        public async Task<IdentityResult?> CreateUser(string email, string password)
        {

            var user = new ApplicationUser();

            await UserStore.SetUserNameAsync(user, email, CancellationToken.None);

            var emailStore = (IUserEmailStore<ApplicationUser>)UserStore;
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            user.EmailConfirmed = true;

            return await UserManager.CreateAsync(user, password);
        }
    }
}
