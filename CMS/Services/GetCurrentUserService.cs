using CMS.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CMS.Services;

public class GetCurrentUserService : IGetCurrentUserService
{
    private readonly AuthenticationStateProvider GetAuthenticationStateAsync;
    private readonly ApplicationDbContext dbContext;

    public GetCurrentUserService(AuthenticationStateProvider getAuthenticationStateAsync,
        ApplicationDbContext dbContext)
    {
        GetAuthenticationStateAsync = getAuthenticationStateAsync;
        this.dbContext = dbContext;
    }

    public async Task<ApplicationUser> GetCurrentUserAsync()
    {
        var authstate = await GetAuthenticationStateAsync.GetAuthenticationStateAsync();
        var authuser = authstate.User;
        if (authuser.Identity.IsAuthenticated) 
        {
        var Name = authuser.Identity.Name;
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == Name);
        return user;
        }

        return null;
    }
}

