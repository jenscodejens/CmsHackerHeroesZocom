using CMS.Data;
using CMS.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMS.Services;

// Services/VisitorCounterService.cs
public class VisitorCounterService
{
    private readonly ApplicationDbContext _context;

    public VisitorCounterService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task IncrementPageVisitAsync(string websiteId, string pageUrl)
    {
        var pageVisit = await _context.WebSiteVisits
            .FirstOrDefaultAsync(wv => wv.WebSiteId == websiteId && wv.PageUrl == pageUrl);

        if (pageVisit == null)
        {
            pageVisit = new WebSiteVisit 
            { 
                WebSiteId = websiteId, 
                PageUrl = pageUrl, 
                VisitCount = 1 
            };
            _context.WebSiteVisits.Add(pageVisit);
        }
        else
        {
            pageVisit.VisitCount++;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<int> GetPageVisitCountAsync(string websiteId, string pageUrl)
    {
        var pageVisit = await _context.WebSiteVisits
            .FirstOrDefaultAsync(wv => wv.WebSiteId == websiteId && wv.PageUrl == pageUrl);
        return pageVisit?.VisitCount ?? 0;
    }

    public async Task<IEnumerable<WebSiteVisit>> GetAllPageVisitsAsync(string websiteId)
    {
        return await _context.WebSiteVisits
            .Where(wv => wv.WebSiteId == websiteId)
            .ToListAsync();
    }
}


/*
public class VisitorCounterService
{
    private readonly ApplicationDbContext _context;

    public VisitorCounterService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task IncrementWebsiteVisitAsync(string websiteId)
    {
        // Find the existing website visit record or create a new one
        var websiteVisit = await _context.WebSiteVisits.FirstOrDefaultAsync(wv => wv.WebSiteId == websiteId);

        if (websiteVisit == null)
        {
            websiteVisit = new WebSiteVisit { WebSiteId = websiteId, VisitCount = 1 };
            _context.WebSiteVisits.Add(websiteVisit);
        }
        else
        {
            websiteVisit.VisitCount++;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<int> GetWebsiteVisitCountAsync(string websiteId)
    {
        var websiteVisit = await _context.WebSiteVisits.FirstOrDefaultAsync(wv => wv.WebSiteId == websiteId);
        return websiteVisit?.VisitCount ?? 0;
    }
}
*/