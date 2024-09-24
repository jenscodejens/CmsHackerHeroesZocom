using CMS.Data;
using CMS.Models;
using CMS.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CMS.Seed
{
    public static class SeedWithoutWebsites
    {
        public static async Task InitAsync(ApplicationDbContext context, ICreateUserService registerService, RoleManager<IdentityRole> roleManager)
        {
            //adds templates we have made in folder if we haven't any values in the template table
            if (!context.Templates.Any())
            {
                var templates = CreateTemplates();
                await context.Templates.AddRangeAsync(templates);
            }

            // Check and seed roles if they don't exist
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Check and seed test user if they don't exist
            //var testEmail = @"test@test.com";
            //if (!context.Users.Any(u => EF.Functions.Like(u.Email, testEmail)))
            //{
            //    var result = await registerService.CreateUser(testEmail, "pSrkXHN6z8s%KHW@");
            //}
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
                   new Template
                {
                    TemplateType = "BodyImageCard",
                    TemplatePath = "Templates.Body.Body1",
                    InputFormPath = "Templates.InputForm.Body1InputForm"
                },
                   new Template
                {
                    TemplateType = "BodySingleImage",
                    TemplatePath = "Templates.Body.SingleImage",
                    InputFormPath = "Templates.InputForm.SingleImageInputForm"
                },
                   new Template
                {
                    TemplateType = "BodySingleVideo",
                    TemplatePath = "Templates.Body.SingleVideoInput",
                    InputFormPath = "Templates.InputForm.SingleVideoInputForm"
                }
            };
            return list;
        }
    }
}
