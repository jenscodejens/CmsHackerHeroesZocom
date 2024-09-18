using CMS.Data;
using CMS.Entities;
using CMS.Migrations;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Templates.InputForm
{
    public partial class NavBarInputForm
    {
        private enum InputStep
        {
            ContentNameInput,
            AddItem,
            Stop,
            Done
        }

        [Inject] private IDbContextFactory<ApplicationDbContext> DbFactory { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        [Parameter] public int TemplateId { get; set; } // Receive the TemplateId
        [Parameter] public int WebPageId { get; set; } // Receive WebPageId from parameters

        [Parameter] public string Backgroundcolor { get; set; } = "grey";
        [Parameter] public string WebPageName { get; set; } = string.Empty;
        [Parameter] public string Textcolor { get; set; } = "black";
        [Parameter] public EventCallback<Dictionary<string, object>> OnSubmit { get; set; }


        private string inputValue = string.Empty;
        private string inputValueContentName = string.Empty;
        
        private InputStep currentStep = InputStep.ContentNameInput;
        private string currentLabelText = string.Empty;


        public Dictionary<string, string> MenyItems = new Dictionary<string, string>() { {"Empty","Empty"} };
    
        //public IEnumerable<Dictionary<string, string>>? IEnMenyItems;
        private IQueryable<WebPage> webpages = Enumerable.Empty<WebPage>().AsQueryable();
        //[SupplyParameterFromQuery]
        //private int? WebSiteId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await using var context = DbFactory.CreateDbContext();

            // Ensure WebPageId exists in WebPages table
            var webPageExists = await context.WebPages.AnyAsync(wp => wp.WebPageId == WebPageId);
            if (!webPageExists)
            {
                throw new InvalidOperationException($"WebPageId {WebPageId} does not exist.");
            }

            //if (WebSiteId.HasValue)
            //{
            //    // Fetch WebPages filtered by WebSiteId
            //    webpages = context.WebPages
            //        .Where(wp => wp.WebSiteId == WebSiteId.Value);

            //    foreach(var site in webpages) {
            //        MenyItems.Add(site.WebSite.Title, "https://Github.com");
            //    }

            //     IEnMenyItems = new List<Dictionary<string, string>>()
            //    { MenyItems };

            //}
            //if (WebPageId.HasValue)
            //{
            // Fetch WebPages filtered by WebSiteId
            //webpages
                var pages = context.WebPages.ToList();
                var page  = pages.FirstOrDefault(wp => wp.WebPageId == WebPageId);
                var webpages1 = pages.Where(wp => wp.WebSiteId == page.WebSiteId);
                //.Where(wp => wp.WebPageId == WebPageId)
                //.Include(WebPageName);




                
                foreach (var site in webpages1)
                {
                    if(site.WebPageName != null)
                    { 
                        MenyItems.Add(site.WebPageName, "https://Github.com");
                    }
                }

                //IEnMenyItems = new List<Dictionary<string, string>>()
                //{ MenyItems };

            //}
            //else
            //{
            //    throw new InvalidOperationException($"WebPageId {WebPageId}, WebSiteId{WebSiteId} is not found.");
            //}




        }

        // Är här man loopas igenom så rätt label visas och det sparas ned under rätt del av json objektet.
        private async Task HandleSubmit()
        {
            if (currentStep == InputStep.ContentNameInput)
            {
                inputValueContentName = inputValue; // spara first input
                inputValue = string.Empty;
                currentStep = InputStep.AddItem; // Hoppa till second input
                //currentLabelText = LabelText2; // Ändra label till den för second input
            }
            else if (currentStep == InputStep.AddItem)
            {
                //inputValue1 = inputValue;
                inputValue = string.Empty;
                //currentStep = InputStep.SecondInput;
                //currentLabelText = LabelText3;
            }
            else if (currentStep == InputStep.Stop)
            {
                //inputValue8 = inputValue;
                inputValue = string.Empty;
                currentStep = InputStep.Done;

                await SaveToDatabase();
            }
        }

        // Hårdkodat WebPageId, den icke-hårdkodade koden får du fixa
        private async Task SaveToDatabase()
        {
            await using var context = DbFactory.CreateDbContext();

            // Ensure WebPageId exists in WebPages table
            var webPageExists = await context.WebPages.AnyAsync(wp => wp.WebPageId == WebPageId);
            if (!webPageExists)
            {
                throw new InvalidOperationException($"WebPageId {WebPageId} does not exist.");
            }

            var content = new Content
            {
                ContentName = inputValueContentName,
                WebPageId = WebPageId,
                TextInputsJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {

                    //TextInput1 = inputValue1,
                    //TextInput2 = inputValue2,
                    //TextInput3 = inputValue3,
                    //TextInput4 = inputValue4,
                    //TextInput5 = inputValue5,
                    //TextInput6 = inputValue6,
                    //TextInput7 = inputValue7,
                    //TextInput8 = inputValue8,
                    //TextInput9 = inputValue9
                }),
                Backgroundcolor = Backgroundcolor,
                Textcolor = Textcolor,
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

            // mapping del så inputs hamnar där de ska
            var formValues = new Dictionary<string, object>
        {
            { "ContentName", inputValueContentName },
            { "TextInput9", WebPageName },
            //{ "TextInput1", inputValue1 },
            //{ "TextInput2", inputValue2 },
            //{ "TextInput3", inputValue3 },
            //{ "TextInput4", inputValue4 },
            //{ "TextInput5", inputValue5 },
            //{ "TextInput6", inputValue6 },
            //{ "TextInput7", inputValue7 },
            //{ "TextInput8", inputValue8 },
            { "Backgroundcolor", Backgroundcolor },
            { "Textcolor", Textcolor }

        };

            await OnSubmit.InvokeAsync(formValues);
        }


        // Borttaget se ovan om varför.
        // private void CancelTemplate()
        // {
        //     inputValue = string.Empty;
        //     inputValue1 = string.Empty;
        //     inputValue2 = string.Empty;
        //     currentStep = InputStep.FirstInput;
        //     currentLabelText = LabelText1;
        // }

        private void Done()
        {
            NavigationManager.NavigateTo($"/contents?webpageid={WebPageId}");
        }

        // private string GetPlaceholderText()
        // {
        //     return currentStep == InputStep.SecondInput ? "optional" : string.Empty;
        // }
    }
}
