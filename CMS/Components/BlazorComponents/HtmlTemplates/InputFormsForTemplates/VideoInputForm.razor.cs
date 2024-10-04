using CMS.Data;
using CMS.Entities;
using CMS.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text.Json;

namespace BlazorComponents.HtmlTemplates.InputFormsForTemplates
{
    public partial class VideoInputForm
    {

        [Inject] private IDbContextFactory<ApplicationDbContext> DbFactory { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        [Parameter] public int TemplateId { get; set; }
        [Parameter] public int WebPageId { get; set; }
        [SupplyParameterFromQuery] public int? ContentId { get; set; }
        [Parameter] public string ContentName { get; set; } = string.Empty;
        [Parameter] public EventCallback<Dictionary<string, object>> OnSubmit { get; set; }

        // Video Properties
        [Parameter] public string VideoUrl { get; set; } = string.Empty;
        [Parameter] public string VideoWidth { get; set; } = "100%";  // Use meaningful default values
        [Parameter] public string VideoHeight { get; set; } = "315px"; // You can adjust height accordingly
        [Parameter] public string VideoAlignment { get; set; } = "center";

        private bool hasSaved = false; // Flag to track if save has been executed
        [Parameter] public bool SaveBtnClicked { get; set; } // New parameter to handle save button state
        [Parameter] public bool MultiPageMode { get; set; } // Receive MultiPageMode parameter

        private bool saveSuccessful = false;  // New flag to show the success message
        public string UserId { get; set; }

        protected override void OnParametersSet()
        {
            await using var context = DbFactory.CreateDbContext();

            var content = context.Contents.FirstOrDefault(C => C.ContentId == ContentId);
            await GetUserID();

            if (content != null)
            {
                var textInputs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(content.ContentJson);
                if (textInputs != null)
                {
                    foreach (var input in textInputs)
                    {
                        var test = input.Key;
                        if (test != null)
                        {
                            //Todo: used in multiple files make into a shared function.
                            string ContentParameterName = "VideoInput";
                            if (ContentParameterName == test.ToString())
                            {
                                string jsonString = input.Value.GetRawText();
                                var menuItemsWrapper = Newtonsoft.Json.JsonConvert.DeserializeObject<MenuItemsWrapper>(jsonString);
                                if (menuItemsWrapper != null)
                                {
                                    MenuItems = menuItemsWrapper.ToDictionary();
                                    currentStep = InputStep.Wait;
                                    TemplateId = content.TemplateId;
                                    inputValueContentName = content.ContentName;
                                    ContentId = content.ContentId;
                                    WebPageId = content.WebPageId;
                                    Update = true;
                                }
                            }
                            else
                            {

                                if (test.ToString() == "Backgroundcolor")
                                {
                                    BackgroundColor = ConvertJsonElement(input.Value).ToString();
                                }
                                else if (test.ToString() == "Textcolor")
                                {
                                    TextColor = ConvertJsonElement(input.Value).ToString();
                                }
                                else
                                {
                                    var error = ConvertJsonElement(input.Value).ToString();
                                    Console.WriteLine($"NavbarInputForm can not match value : {error}.");
                                }

                            }
                        }
                        else
                        {
                            Console.WriteLine($"Data for contents name is invalid.");
                        }
                    }
                }
            }


        
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

        // Save all inputs to the database when the "Save" button is clicked
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
            await using var context = DbFactory.CreateDbContext();
            var user = await GetCurrentUserService.GetCurrentUserAsync();

            // Ensure WebPageId exists in the WebPages table
            var webPageExists = await context.WebPages.AnyAsync(wp => wp.WebPageId == WebPageId);
            if (!webPageExists)
            {
                throw new InvalidOperationException($"WebPageId {WebPageId} does not exist.");
            }

            var content = new Content
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

            context.Contents.Add(content);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log detailed information for debugging
                Console.WriteLine($"Error: {ex.InnerException?.Message}");
                throw;
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

