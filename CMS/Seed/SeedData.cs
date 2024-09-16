using CMS.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Bogus;
using CMS.Services;
using CMS.Entities;
using CMS.Models;

namespace CMS.Seed
{
    public static class SeedData
    {

        private static Faker faker = new Faker();
        private static Random random = new Random();
        public static async Task InitAsync(ApplicationDbContext context, ICreateUserService registerService)
        {

            //if (context.WebSites.Any() && context.WebSites.Count() < 10)
            //{
            //    return;
            //}

            //adds templates we have made in folder if we haven't any values in the template table
            if (!context.Templates.Any())
            {
                var templates = CreateTemplates();
                await context.Templates.AddRangeAsync(templates);
            }

            //for (int i = 0; i < 4; i++)
            //{
            //    var fName = faker.Name.FirstName();
            //    var lName = faker.Name.LastName();
            //    var email = faker.Internet.Email(fName, lName, "zocom.se");
            //    var result = await registerService.CreateUser(email, "pSrkXHN6z8s%KHW@");
            //    Console.WriteLine(result);
            //}

            //var result1 = await registerService.CreateUser(@"test@test.com", "pSrkXHN6z8s%KHW@");
            //Console.WriteLine(result1);

            //var userId = context.Users.Where(u => EF.Functions.Like(u.Email, "test@test.com")).Select(u => u.Id).FirstOrDefault();
            //if (userId == null)
            //{
            //    return;
            //}

           
            //var menus = CreateWebSites(userId);

            //await context.AddRangeAsync(menus);
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
                    TemplateType = "Footer2",
                    TemplatePath = "Templates.SingleInput.Template2",
                    InputFormPath = "Templates.InputForm.SingleInputForm"
                },
                new Template
                {
                    TemplateType = "Footer3",
                    TemplatePath = "Templates.SingleInput.Template3",
                    InputFormPath = "Templates.InputForm.SingleInputForm"
                },
                new Template
                {
                    TemplateType = "Footer",
                    TemplatePath = "Templates.SingleInput.Template1",
                    InputFormPath = "Templates.InputForm.DoubleInputForm"
                },
                 new Template
                {
                    TemplateType = "Header",
                    TemplatePath = "Templates.SingleInput.Template1Header",
                    InputFormPath = "Templates.InputForm.DoubleInputForm"
                },
                  new Template
                {
                    TemplateType = "Link",
                    TemplatePath = "Templates.SingleInput.Template1Link",
                    InputFormPath = "Templates.InputForm.DoubleInputForm"
                },
                   new Template
                {
                    TemplateType = "NavBar",
                    TemplatePath = "Templates.SingleInput.Template1NavBar",
                    InputFormPath = "Templates.InputForm.NavBarInputForm"
                },
            };

            return list;
        }

    }

}
