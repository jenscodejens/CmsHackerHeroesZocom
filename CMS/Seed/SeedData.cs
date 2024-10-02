using Bogus;
using CMS.Data;
using CMS.Entities;
using CMS.Models;
using CMS.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CMS.Seed
{
    public static class SeedData
    {

        private static Faker faker = new Faker();
        private static Random random = new Random();
        public static async Task InitAsync(ApplicationDbContext context, ICreateUserService registerService, RoleManager<IdentityRole> roleManager)
        {

            if (context.WebSites.Any())
            {
                return;
            }

            //adds templates we have made in folder if we haven't any values in the template table
            if (!context.Templates.Any())
            {
                var templates = CreateTemplates();
                await context.Templates.AddRangeAsync(templates);
            }

            // Seed roles
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            for (int i = 0; i < 4; i++)
            {
                var fName = faker.Name.FirstName();
                var lName = faker.Name.LastName();
                var email = faker.Internet.Email(fName, lName, "zocom.se");
                var result = await registerService.CreateUser(email, "pSrkXHN6z8s%KHW@");
                Console.WriteLine(result);
            }

            var result1 = await registerService.CreateUser(@"test@test.com", "pSrkXHN6z8s%KHW@");
            Console.WriteLine(result1);

            var userId = context.Users.Where(u => EF.Functions.Like(u.Email, "test@test.com")).Select(u => u.Id).FirstOrDefault();
            if (userId == null)
            {
                return;
            }


            var menus = CreateWebSites(userId);

            await context.AddRangeAsync(menus);
            context.SaveChanges();

        }

        private static ICollection<WebSite> CreateWebSites(string OwnerId)
        {

            var list = new List<WebSite>();

            for (int i = 0; i < 10; i++)
            {

                var menu = new WebSite
                {
                    Title = faker.Lorem.Slug(2),
                    Description = faker.Lorem.Sentence(),
                    WebPages = CreateWebpages(random.Next(3, 8)),
                    UserId = OwnerId
                };

                list.Add(menu);
            }

            return list;
        }

        private static ICollection<WebPage> CreateWebpages(int count)
        {
            var list = new List<WebPage>();
            for (int i = 0; i < count; i++)
            {

                var webpage = new WebPage
                {
                    Header = faker.Lorem.Slug(2),
                    Body = faker.Lorem.Sentence(10),
                    Footer = faker.Lorem.Sentence(2),
                };

                list.Add(webpage);
            }

            return list;
        }

        private static ICollection<Content> CreateContentBlocks()
        {

            var list = new List<Content>
            {
              new Content
            {
                ContentName = faker.Lorem.Slug(3),
                           },

            new Content
            {

                ContentName ="https://picsum.photos/200/300?random=" + $"{random.Next(1,10000)}",

            },
             new Content
            {
                ContentName = faker.Lorem.Slug(10),

            }
            };

            return list;
        }

        private static ICollection<Template> CreateTemplates()
        {
            var list = new List<Template>
            {
                new Template
                {
                    TemplateType = "Bild Kort",
                    TemplatePath = "BlazorComponents.HtmlTemplates.TemplatesForComponents.ImageCardTemplate",
                    InputFormPath = "BlazorComponents.HtmlTemplates.InputFormsForTemplates.ImageCardInputForm"
                },
                 new Template
                {
                    TemplateType = "Bild",
                    TemplatePath = "BlazorComponents.HtmlTemplates.TemplatesForComponents.ImageTemplate",
                    InputFormPath = "BlazorComponents.HtmlTemplates.InputFormsForTemplates.ImageInputForm"
                },
                  new Template
                {
                    TemplateType = "Text(MarkDown)",
                    TemplatePath = "BlazorComponents.HtmlTemplates.TemplatesForComponents.TextMarkDownTemplate",
                    InputFormPath = "BlazorComponents.HtmlTemplates.InputFormsForTemplates.TextMarkdownInputForm"
                },
                    new Template
                {
                    TemplateType = "Text(Quill)",
                    TemplatePath = "BlazorComponents.HtmlTemplates.TemplatesForComponents.TextQuillTemplate",
                    InputFormPath = "BlazorComponents.HtmlTemplates.InputFormsForTemplates.TextQuillInputForm"
                },

                  new Template
                {
                    TemplateType = "Video",
                    TemplatePath = "BlazorComponents.HtmlTemplates.TemplatesForComponents.VideoTemplate",
                    InputFormPath = "BlazorComponents.HtmlTemplates.InputFormsForTemplates.VideoInputForm"
                },
                    new Template
                {
                    TemplateType = "SidFot",
                    TemplatePath = "BlazorComponents.HtmlTemplates.TemplatesForComponents.FooterTemplate",
                    InputFormPath = "BlazorComponents.HtmlTemplates.InputFormsForTemplates.DoubleInputForm"
                },
                    new Template
                {
                    TemplateType = "SidHuvud",
                    TemplatePath = "BlazorComponents.HtmlTemplates.TemplatesForComponents.HeaderTemplate",
                    InputFormPath = "BlazorComponents.HtmlTemplates.InputFormsForTemplates.DoubleInputForm"
                },
                    new Template
                {
                    TemplateType = "Länk",
                    TemplatePath = "BlazorComponents.HtmlTemplates.TemplatesForComponents.LinkTemplate",
                    InputFormPath = "BlazorComponents.HtmlTemplates.InputFormsForTemplates.DoubleInputForm"
                },
                    new Template
                {
                    TemplateType = "Navigation Bar",
                    TemplatePath = "BlazorComponents.HtmlTemplates.TemplatesForComponents.NavBarTemplate",
                    InputFormPath = "BlazorComponents.HtmlTemplates.InputFormsForTemplates.NavBarInputForm"
                },

            };

            return list;
        }

    }
}