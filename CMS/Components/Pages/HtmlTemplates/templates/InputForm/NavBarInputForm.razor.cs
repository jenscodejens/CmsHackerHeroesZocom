using CMS.Data;
using CMS.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Templates.SingleInput;



namespace Templates.InputForm
{
    public partial class NavBarInputForm
    {
        private enum InputStep
        {
            ContentNameInput,
            AddItem,
            Wait,
            Edit,
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

        [Parameter] public string templateDropdown { get; set; } = string.Empty;

        private string inputValue = string.Empty;
        private string inputKey = string.Empty;
        private string inpuItemtURL = string.Empty;
        private string inputValueContentName = string.Empty;
        private string oldKey = string.Empty;
        //private string inputItemValue = string.Empty;

        private InputStep currentStep = InputStep.ContentNameInput;
        private string currentLabelText = string.Empty;


        public Dictionary<string, string> MenyItems = new Dictionary<string, string>() { { "Länk saknas","Titel saknas"  } };
        public Dictionary<string, string> AddMenyItems = new Dictionary<string, string>();
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
                var pages = await context.WebPages.ToListAsync();
                var page  = pages.FirstOrDefault(wp => wp.WebPageId == WebPageId);
                var webpages1 = pages.Where(wp => wp.WebSiteId == page.WebSiteId);
            //.Where(wp => wp.WebPageId == WebPageId)
            //.Include(WebPageName);





            foreach (var site in webpages1)
            {
                if (site.WebPageName != null)
                {
                    MenyItems.Add(site.WebPageName, site.WebPageName);
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
        private void AddMenuName()
        {

            inputValueContentName = inputValue; // Input NavBar name
            inputValue = string.Empty;
            currentStep = InputStep.AddItem; // Hoppa till second input
                                             //currentLabelText = LabelText2; // Ändra label till den för second input  
        }

        private void AddItem()
        {
            if (!string.IsNullOrEmpty(templateDropdown) && !string.IsNullOrEmpty(inputValue))
            {
                if (!AddMenyItems.ContainsKey(inputValue))
                {
                    AddMenyItems.Add(inputValue, templateDropdown);
                    inputValue = string.Empty;
                    currentStep = InputStep.Wait;
                }
                else 
                {
                    //Todo: Set Alertmessage: You can not use the same name for two items.
                    currentStep = InputStep.AddItem;
                }
            }
            
        }
        private void NewItem()
        {          
                currentStep = InputStep.AddItem; 
        }

        private void Edit(string href)
        {
            foreach (var item in AddMenyItems)
            {
                //Todo: Set Alertmessage?: error is not found.
                if (item.Key == href)
                {
                    inputValue = item.Key;
                    oldKey = item.Key;
                    templateDropdown = item.Value;
                }


            }
            currentStep = InputStep.Edit;
        }

        private void UpdateItem()
        {
            //ToDo: Check, not 2 keys with the same values.
            // Check if the new key already exists
            if (AddMenyItems.ContainsKey(inputValue) && oldKey != inputValue)
            {


                    //ToDo: Alert message: You cannot use the same name for two items.

                   return; // Exit the method to prevent adding the same key
                
            }


            if (AddMenyItems.ContainsKey(oldKey))
            {
             
                string value = AddMenyItems[oldKey];

                
                AddMenyItems.Remove(oldKey);

                
                AddMenyItems[inputValue] = templateDropdown; // Maintain the same value while keeping insertion order

                
                inputValue = string.Empty;
                currentStep = InputStep.Wait;
            }
            else
            {
                
                AddMenyItems.Add(inputValue, templateDropdown); // Add new key-value
                inputValue = string.Empty;
                currentStep = InputStep.Wait;
            }
            //if (!AddMenyItems.ContainsKey(inputValue))
            //{
            //    AddMenyItems.Add(inputValue, templateDropdown);

            //    if (AddMenyItems.ContainsKey(oldKey))
            //    {
            //        string value = AddMenyItems[oldKey];

            //        AddMenyItems.Remove(oldKey);


            //        AddMenyItems[inputValue] = templateDropdown;

            //        inputValue = string.Empty;
            //        currentStep = InputStep.Wait;
            //    }
            //}
            //else 
            //{
            //    //Todo: Set Alertmessage: You can not use the same name for two items.
            //}


        }

        private void AbortItem()
        {
            inputValue = string.Empty;
            currentStep = InputStep.Wait;
        }

        private void DeleteItem()
        {
            AddMenyItems.Remove(oldKey);
            currentStep = InputStep.Wait;
        }
      
        private async Task Save()
        {
            await SaveToDatabase();
        }
                

        // Hårdkodat WebPageId, den icke-hårdkodade koden får du fixa
        private async Task SaveToDatabase()
        {
            //Todo:Test this:
            if (!AddMenyItems.Any()|| inputValueContentName == string.Empty)
            {
                if(inputValueContentName == string.Empty)
                {
                    //Todo: Add alertmessage: Menu name is mandatory.
                    currentStep = InputStep.ContentNameInput;
                    return;
                }
                else
                {
                    //Todo: Add alertmessage: The navbar needs at least one item.
                    currentStep = InputStep.Wait;
                    return;
                }
            }
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


        private void Done()
        {
            NavigationManager.NavigateTo($"/contents?webpageid={WebPageId}");
        }

    }
}
