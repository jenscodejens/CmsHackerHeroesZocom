using CMS.Data;
using CMS.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using CMS.Shared;
using System.Text.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace BlazorComponents.HtmlTemplates.InputFormsForTemplates
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
        //[CascadingParameter] public int? ContentId { get; set; }
        [Parameter] public string WebPageName { get; set; } = string.Empty;
        [Parameter] public string templateDropdown { get; set; } = string.Empty;
        [Parameter] public bool SaveBtnClicked { get; set; } // New parameter to handle save button state
        [Parameter] public bool MultiPageMode { get; set; } // Receive MultiPageMode parameter

        private bool saveSuccessful = false;
        private bool Update = false;
        private bool hasSaved = false; // Flag to track if save has been executed
        private string inputValue = string.Empty;
        private string inputKey = string.Empty;
        private string inpuItemtURL = string.Empty;
        private string inputValueContentName = string.Empty;
        private string oldKey = string.Empty;
        //private string inputItemValue = string.Empty;

        private InputStep currentStep = InputStep.ContentNameInput;
        private string currentLabelText = string.Empty;
        public Dictionary<string, string> Pages = new Dictionary<string, string>() { { "Länk saknas","Titel saknas"  } };
        private IQueryable<WebPage> webpages = Enumerable.Empty<WebPage>().AsQueryable();

        public string UserId { get; set; }

        protected override async Task OnInitializedAsync()
        {


            await GetUserID();
            //ToDo: Add user verification, altternatives: check if user are assigned to a website(multiple users assigned to a website, or multiple users per website+webpage+content,
            //if content can be edited by multiple users this will not be working:
            //if (content.UserId != UserId) 
            //{
            //    throw new InvalidOperationException($"You are not authorized to edit the content.");
            //}
            if (ContentId != null)
            {
                await LoadNavBarContent();
            }

           
            await RetrieveWebPages();

        }

        private async Task LoadNavBarContent()
        {
            if (ContentExists((int)ContentId))
            {
                var content = await ContentService.GetContentAsync((int)ContentId);
                if (content != null)
                {
                    GetNavbarContent(content);
                }


            }
        }

        private void GetNavbarContent(Content? content)
        {
            var textInputs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(content.ContentJson);
            if (textInputs != null)
            {
                foreach (var jsonContent in textInputs)
                {
                    var objectName = jsonContent.Key;
                    if (objectName != null)
                    {
                        GetNavBarParameters(content, jsonContent, objectName);
                    }
                    else
                    {
                        Console.WriteLine($"Data for contents name is invalid.");
                    }
                }
            }
        }

        private void GetNavBarParameters(Content? content, KeyValuePair<string, JsonElement> jsonContent, string objectName)
        {
            //Todo: used in multiple files make into a shared function.
            string ContentParameterName = "MenuItems";
            if (ContentParameterName == objectName.ToString())
            {
                GetMenyItems(content, jsonContent);
            }
            else
            {
                GetColorParameters(jsonContent, objectName);

            }
        }

        private void GetMenyItems(Content? content, KeyValuePair<string, JsonElement> jsonContent)
        {
            string jsonString = jsonContent.Value.GetRawText();
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

        private void GetColorParameters(KeyValuePair<string, JsonElement> jsonContent, string objectName)
        {
            if (objectName.ToString() == "Backgroundcolor")
            {
                BackgroundColor = ConvertJsonElement(jsonContent.Value).ToString();
            }
            else if (objectName.ToString() == "Textcolor")
            {
                TextColor = ConvertJsonElement(jsonContent.Value).ToString();
            }
            else
            {
                var error = ConvertJsonElement(jsonContent.Value).ToString();
                Console.WriteLine($"NavbarInputForm can not match value : {error}.");
            }
        }

        private async Task RetrieveWebPages()
        {
            //Todo: Add alertmessage

           

            //await using var context = DbFactory.CreateDbContext();
            //// Ensure WebPageId exists in WebPages table
            //var webPageExists = await context.WebPages.AnyAsync(wp => wp.WebPageId == WebPageId);
            //if (!webPageExists)
            //{
            //    throw new InvalidOperationException($"WebPageId {WebPageId} does not exist.");
            //}

            //var pages = await context.WebPages.ToListAsync();
            //var page = pages.FirstOrDefault(wp => wp.WebPageId == WebPageId);
            //var webpages1 = pages.Where(wp => wp.WebSiteId == page.WebSiteId);

            var websitesWebPages = await WebPageService.GetWebPagesFromSameWebSiteAsync(WebPageId);

            foreach (var page in websitesWebPages)
            {
                if (page.Title != null && !Pages.ContainsKey(page.Title))
                {
                    Pages.Add(page.Title, page.WebPageId.ToString());
                }
            }
        }

        //ToDo: Move to separate class used in several files.
        private async Task GetUserID()
        {
            var user = await GetCurrentUserService.GetCurrentUserAsync();
            UserId = user.Id;
        }



        //ToDo: Handle async method.
        protected override void OnParametersSet()
        {
            if (SaveBtnClicked && !hasSaved) // Check if SaveBtnClicked and save hasn't been executed yet
            {
                Save(); // Call the save method
                hasSaved = true; // Set the flag to prevent further saves
            }
        }

        //ToDo: Used in multipleFiles move to shared repository.
        private object? ConvertJsonElement(JsonElement jsonElement)
        {
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.String:
                    return jsonElement.GetString();
                case JsonValueKind.Number:
                    return jsonElement.GetDecimal();
                case JsonValueKind.True:
                case JsonValueKind.False:
                    return jsonElement.GetBoolean();
                case JsonValueKind.Object:
                    return jsonElement.GetRawText(); // Handle objects if needed
                case JsonValueKind.Array:
                    return jsonElement.ToString(); // Handle arrays if needed
                default:
                    return null;
            }
        }

        //ToDo: Used in multiple files move function to shared folder.
        bool ContentExists(int contentid)
        {
            using var context = DbFactory.CreateDbContext();
            return context.Contents.Any(e => e.ContentId == contentid);
        }
        private void AddMenuName()
        {

            inputValueContentName = inputValue; // Input NavBar name
            inputValue = string.Empty;
            currentStep = InputStep.AddItem;  
        }

        private void AddItem()
        {
            if (!string.IsNullOrEmpty(templateDropdown) && !string.IsNullOrEmpty(inputValue))
            {
                if (!MenuItems.ContainsKey(inputValue))
                {
                    MenuItems.Add(inputValue, templateDropdown);
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
            foreach (var item in MenuItems)
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
            if (MenuItems.ContainsKey(inputValue) && oldKey != inputValue)
            {
                    //ToDo: Alert message: You cannot use the same name for two items.
                   return; // Exit the method to prevent adding the same key   
            }

            if (MenuItems.ContainsKey(oldKey))
            {             
                string value = MenuItems[oldKey];              
                MenuItems.Remove(oldKey);              
                MenuItems[inputValue] = templateDropdown; // Maintain the same value while keeping insertion order              
                inputValue = string.Empty;
                currentStep = InputStep.Wait;
            }
            else
            { 
                MenuItems.Add(inputValue, templateDropdown); // Add new key-value
                inputValue = string.Empty;
                currentStep = InputStep.Wait;
            }
           
        }

        private void AbortItem()
        {
            inputValue = string.Empty;
            currentStep = InputStep.Wait;
        }

        private void DeleteItem()
        {
            MenuItems.Remove(oldKey);
            currentStep = InputStep.Wait;
        }
      
        private async Task Save()
        {
            await SaveToDatabase();
        }

        

        //Todo: Divide code into functions.
        private async Task SaveToDatabase()
        {

            if (!MenuItems.Any() || inputValueContentName == string.Empty)
            {
                if (inputValueContentName == string.Empty)
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

            NavBarTextInputJson textInputJson = ConvertMenuItemsToJson();

            var content = new Content();

            if (Update)
            {
                var updatetime = DateOnly.FromDateTime(DateTime.Now);
                content = new Content
                {
                    ContentName = inputValueContentName,
                    WebPageId = WebPageId,
                    ContentJson = Newtonsoft.Json.JsonConvert.SerializeObject(textInputJson), // Serialize the wrapper
                    TemplateId = TemplateId,
                    ContentId = (int)ContentId,
                    UserId = UserId,
                    LastUpdated = updatetime
                };
                //context.Contents.Update(content);
            }
            else
            {
                content = new Content
                {
                    ContentName = inputValueContentName,
                    WebPageId = WebPageId,
                    ContentJson = Newtonsoft.Json.JsonConvert.SerializeObject(textInputJson), // Serialize the wrapper
                    TemplateId = TemplateId,
                    UserId = UserId,
                    CreationDate = DateOnly.FromDateTime(DateTime.Now)
                };
                // context.Contents.Add(content);
            }

            if (ContentId.HasValue)
            {
                //ToDo: Secure handling of Id needs to be evaluated avoiding change of id through unallowed methods. 
                content.ContentId = ContentId.Value;
                await ContentService.UpdateContentAsync(content);
                saveSuccessful = true;
            }
            else
            {
                await ContentService.SaveContentAsync(content);
                saveSuccessful = true;
            }
        }

        private NavBarTextInputJson ConvertMenuItemsToJson()
        {
            var menuItemsWrapper = new MenuItemsWrapper("MenuItems", "Dictionary<string, string>", MenuItems);

            var textInputJson = new NavBarTextInputJson
            {
                MenuItems = menuItemsWrapper,
                Textcolor = TextColor,
                Backgroundcolor = BackgroundColor
            };
            return textInputJson;
        }

        private void Done()
        {
            NavigationManager.NavigateTo($"/contents?webpageid={WebPageId}");
        }

    }
}
