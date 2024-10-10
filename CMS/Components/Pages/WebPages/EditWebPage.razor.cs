
using CMS.Data;
using CMS.Entities;
using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace CMS.Components.Pages.WebPages
{
    public partial class EditWebPage
    {
        IQueryable<Content> contents = Enumerable.Empty<Content>().AsQueryable();
        [SupplyParameterFromQuery]
        public int? WebPageId { get; set; }
        private int? ContentForEditing { get; set; } = null;

        public int ContentId { get; set; } // Fetch ContentId from the query

        bool StopEditing { get; set; } = false;

        bool editPageinformation { get; set; } = false;

        bool Create { get; set; } = false;

        public Content? Content { get; set; }

        ApplicationDbContext context = default!;


        protected override async Task OnInitializedAsync()
        {
            context = DbFactory.CreateDbContext();

            if (WebPageId.HasValue)
            {
                // Fetch content filtered by WebPageId
                contents = context.Contents.Where(c => c.WebPageId == WebPageId.Value);
            }
            else
            {
                // Fetch all content if no WebPageId is provided
                //contents = context.Contents;
            }
        }
        private void EditContent(Content content)
        {
            ContentForEditing = content.ContentId;
            ContentId = content.ContentId;
            Content = content;
            StopEditing = false;
        }
        private void AddContent()
        {
            Create = true;
            ContentForEditing = null;
            StopEditing = true;
        }
        private void PauseEditContent()
        {
            ContentForEditing = null;
            StopEditing = true;
        }
        private void EditPageinformation()
        {
            editPageinformation = true;
        }

        private void EditPageinformationDone()
        {
            editPageinformation = true;
            contents = context.Contents.Where(c => c.WebPageId == WebPageId);
            StateHasChanged();
        }
        private void CreateDone()
        {
            ContentForEditing = null;
            Create = false;
            contents = context.Contents.Where(c => c.WebPageId == WebPageId);
            StateHasChanged();
        }

        private void ResumeEditContent()
        {
            ContentForEditing = null;
            StopEditing = false;
        }

        public async ValueTask DisposeAsync() => await context.DisposeAsync();
    }
}
