using BlazorBootstrap;
using CMS.Entities;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System;

namespace CMS.Shared
{
    public class DragAndDrop
    {
      
        //@using CMS.Components.BlazorComponents


        <SortableList TItem = "ContentRenderingOrder"
                      Data= "contentRenderingOrders"
                      Context= "item"
                      OnUpdate= "@OnContentListUpdate" >
            < ItemTemplate >
                @item.Content?.ContentName(ID: @item.ContentId)
            </ ItemTemplate >
        </ SortableList >

        public WebPage? WebPage { get; set; }
        public List<ContentRenderingOrder> contentRenderingOrders = new();

        public DragAndDrop(WebPage webPage, string ErrorMessage)
        {
            using var context = DbFactory.CreateDbContext();

            // Fetch WebPage along with ContentRenderingOrders and Content data
            //WebPage = await context.WebPages
            //    .Include(wp => wp.ContentRenderingOrders)
            //        .ThenInclude(cro => cro.Content) // Include Content to access ContentName
            //    .FirstOrDefaultAsync(m => m.WebPageId == WebPageId);

            if (webPage is null)
            {
                NavigationManager.NavigateTo(@"{errorMessage}");
            }
            else
            {
                // Initial list for OnInitializedAsync
                contentRenderingOrders = webPage.Contents
                    .OrderBy(cnt => cnt.ContentId)
                    .ToList();
            }
        }

        private async Task OnContentListUpdate(SortableListEventArgs args)
        {
            // Get the item to be moved
            var itemToMove = contentRenderingOrders[args.OldIndex];

            // Remove the item from the old position
            contentRenderingOrders.RemoveAt(args.OldIndex);

            // Insert the item at the new position
            if (args.NewIndex < contentRenderingOrders.Count)
                contentRenderingOrders.Insert(args.NewIndex, itemToMove);
            else
                contentRenderingOrders.Add(itemToMove);

            await UpdateContentIdsBasedOnNewOrder();
        }

        private async Task UpdateContentIdsBasedOnNewOrder()
        {
            using var context = DbFactory.CreateDbContext();

            // Rearrange the list, Order is fixed but ContentId's are not
            var orderedContentIds = contentRenderingOrders
                .OrderBy(cro => cro.Order)
                .Select(cro => cro.ContentId)
                .ToList();

            for (int i = 0; i < contentRenderingOrders.Count; i++)
            {
                var cro = contentRenderingOrders[i];
                var dbEntry = await context.ContentRenderingOrders.FindAsync(cro.ContentOrderId);

                if (dbEntry != null)
                {
                    // Maintain the original Order and update the ContentId
                    dbEntry.ContentId = orderedContentIds[i];
                }
            }

            await context.SaveChangesAsync();
        }
    }

}
}
