using CMS.Data;
using CMS.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMS.Services;
public class VisitorCounterService
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public VisitorCounterService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task IncrementPageVisitAsync(int webSiteId, string pageUrl)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var visit = await context.WebSiteVisits
            .FirstOrDefaultAsync(v => v.WebSiteId == webSiteId && v.PageUrl == pageUrl);

        if (visit == null)
        {
            visit = new WebSiteVisit
            {
                WebSiteId = webSiteId,
                PageUrl = pageUrl,
                VisitCount = 1
            };
            context.WebSiteVisits.Add(visit);
        }
        else
        {
            visit.VisitCount++;
        }

        await context.SaveChangesAsync();
    }

    public async Task<int> GetPageVisitCountAsync(int webSiteId, string pageUrl)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var visit = await context.WebSiteVisits
            .FirstOrDefaultAsync(v => v.WebSiteId == webSiteId && v.PageUrl == pageUrl);

        return visit?.VisitCount ?? 0;
    }

    public async Task<IEnumerable<WebSiteVisit>> GetAllPageVisitsAsync(int webSiteId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return await context.WebSiteVisits
            .Where(v => v.WebSiteId == webSiteId)
            .ToListAsync();
    }
}


