using CMS.Data;
using CMS.Entities;
using CMS.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;
using Newtonsoft.Json;
using CMS.Services;



namespace BlazorComponents.HtmlTemplates.InputFormsForTemplates
{
    public partial class VideoInputForm
    {

        [Inject] private IDbContextFactory<ApplicationDbContext> DbFactory { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        //[Parameter] public int TemplateId { get; set; }
        //[Parameter] public int WebPageId { get; set; }
        //[Parameter] public int? ContentId { get; set; }
        //[Parameter] public string ContentName { get; set; } = string.Empty;
        //[Parameter] public EventCallback<Dictionary<string, object>> OnSubmit { get; set; }

        // Video Properties
        //[Parameter] public string VideoUrl { get; set; } = string.Empty;
        //[Parameter] public string VideoWidth { get; set; } = "100%";  // Use meaningful default values
        //[Parameter] public string VideoHeight { get; set; } = "315px"; // You can adjust height accordingly
        //[Parameter] public string VideoAlignment { get; set; } = "center";

        private bool hasSaved = false; // Flag to track if save has been executed
        [Parameter] public bool SaveBtnClicked { get; set; } // New parameter to handle save button state
        [Parameter] public bool MultiPageMode { get; set; } // Receive MultiPageMode parameter

        private bool saveSuccessful = false;  // New flag to show the success message
        public string UserId { get; set; }

        private class videoInputs
        {
            public string VideoUrl { get; set; } = string.Empty;
            public string VideoWidth { get; set; } = string.Empty;
            public string VideoHeight { get; set; } = string.Empty;
            public string VideoAlignment { get; set; } = string.Empty;

    }

            protected override async Task OnInitializedAsync()
        {
            await using var context = DbFactory.CreateDbContext();
            LoadVideoContent(context);
            await GetUserID();

        }

        private async void LoadVideoContent(ApplicationDbContext context)
        {
            if (ContentId != null)
            {
                var Currentcontent = await ContentService.GetContentAsync((int)ContentId);

                if (Currentcontent != null)
                {

                    ContentName = Currentcontent.ContentName;

                    if (!string.IsNullOrEmpty(Currentcontent.ContentJson))
                    {
                        var contentData = JsonConvert.DeserializeObject<videoInputs>(Currentcontent.ContentJson);
                        //Todo verfication and alertmessage if conted not loaded.
                        VideoUrl = contentData.VideoUrl;
                        VideoWidth = contentData.VideoWidth;
                        VideoHeight = contentData.VideoHeight;
                        VideoAlignment = contentData.VideoAlignment;
                        await PreviewImage();
                    }
                }
                else
                {
                    Console.WriteLine($"Content data error (null or empty).");
                }

            }
        }

        protected override void OnParametersSet()
        {
            if (SaveBtnClicked && !hasSaved) // Check if SaveBtnClicked and save hasn't been executed yet
            {
                SaveToDatabase(); // Call the save method
                hasSaved = true; // Set the flag to prevent further saves
            }
        }

        //ToDo: move to separate class used in several files in the application.
        private async Task GetUserID()
        {
            var user = await GetCurrentUserService.GetCurrentUserAsync();
            if (user.Id == null)
            {
                throw new InvalidOperationException($"user Id {user.Id} does not exist.");
            }
            UserId = user.Id;
        }

        private async Task PreviewImage()
        {

            // Mapping section to pass form values to a parent component or handler
            var formValues = new Dictionary<string, object>
        {
            { "VideoUrl", VideoUrl },
            { "VideoWidth", VideoWidth },
            { "VideoHeight", VideoHeight },
            { "VideoAlignment", VideoAlignment },
        };

            await OnSubmit.InvokeAsync(formValues);

            // Navigate to the contents page
            //Done();
        }

        // Save all inputs to the database when the "Save" button is clicked
        private async Task SaveToDatabase()
        {

            var user = await GetCurrentUserService.GetCurrentUserAsync();

            var content = new Content();
            if (ContentId == null)
            {
                content = new Content
                {
                    UserId = user.Id,
                    ContentName = ContentName,
                    WebPageId = WebPageId,
                    CreationDate = DateOnly.FromDateTime(DateTime.Now),
                    LastUpdated = DateOnly.FromDateTime(DateTime.Now),
                    ContentJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        VideoUrl = VideoUrl,
                        VideoWidth = VideoWidth,
                        VideoHeight = VideoHeight,
                        VideoAlignment = VideoAlignment
                    }),
                    TemplateId = TemplateId
                };
            }
            else
            {
                if (content != null)
                {
                    // Update existing content
                    content.LastUpdated = DateOnly.FromDateTime(DateTime.Now);
                    content.ContentName = ContentName;
                    content.ContentJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        VideoUrl = VideoUrl,
                        VideoWidth = VideoWidth,
                        VideoHeight = VideoHeight,
                        VideoAlignment = VideoAlignment
                    });

                }
            }

            // Save or update content based on ContentId presence
            if (ContentId.HasValue)
            {
                content.ContentId = ContentId.Value;
                await ContentService.UpdateContentAsync(content);
            }
            else
            {
                await ContentService.SaveContentAsync(content);
            }

            saveSuccessful = true;

        }

        // Navigate to the contents page after saving
        private void Done()
        {
            NavigationManager.NavigateTo($"/contents?webpageid={WebPageId}");
        }
    }
}

