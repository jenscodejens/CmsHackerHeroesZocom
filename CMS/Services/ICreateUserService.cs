using Microsoft.AspNetCore.Identity;

namespace CMS.Services
{
    public interface ICreateUserService
    {
        public Task<IdentityResult?> CreateUser(string email, string password);
    }
}
