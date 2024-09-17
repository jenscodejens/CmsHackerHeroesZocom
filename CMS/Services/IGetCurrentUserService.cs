using CMS.Data;

namespace CMS.Services;

public interface IGetCurrentUserService
{
    public  Task<ApplicationUser> GetCurrentUserAsync();

}