using Domain.Entities;
using Domain.Enums;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data;

public static class PortfolioDbSeeder
{
    public static async Task SeedAsync(
        PortfolioDbContext portfolioDbContext,
        UserManager<ApplicationUser> userManager)
    {
        if (!portfolioDbContext.PortfolioProfiles.Any())
        {
            portfolioDbContext.PortfolioProfiles.Add(new PortfolioProfile
            {
                FirstName = "Fredric",
                LastName = "Domvall",
                ProfessionalTitle = ".NET Web Developer",
                AboutMe = "Student focused on ASP.NET Core, C#, CMS and modern web development.",
                EmailAddress = "fredric.domvall@gmail.com",
                LinkedInUrl = "",
                GitHubUrl = ""
            });
        }

        if (!portfolioDbContext.Skills.Any())
        {
            portfolioDbContext.Skills.AddRange(
                new Skill
                {
                    Name = "C#",
                    Level = SkillLevel.Advanced
                },
                new Skill
                {
                    Name = "ASP.NET Core MVC",
                    Level = SkillLevel.Advanced
                },
                new Skill
                {
                    Name = "Entity Framework Core",
                    Level = SkillLevel.Intermediate
                },
                new Skill
                {
                    Name = "SQL Server",
                    Level = SkillLevel.Intermediate
                });
        }

        if (!portfolioDbContext.PortfolioProjects.Any())
        {
            portfolioDbContext.PortfolioProjects.Add(
                new PortfolioProject
                {
                    Title = "Fredric Portfolio",
                    Description = "Personal portfolio and digital CV.",
                    TechnologiesUsed = "ASP.NET Core MVC, EF Core, SQL Server",
                    CreatedDate = DateTime.UtcNow,
                    IsFeatured = true,
                    Category = ProjectCategory.WebApplication
                });
        }

        if (!portfolioDbContext.UploadedDocuments.Any())
        {
            portfolioDbContext.UploadedDocuments.Add(
                new UploadedDocument
                {
                    FileName = "Fredric-Domvall-CV.pdf",
                    FilePath = "documents/Fredric-Domvall-CV.pdf",
                    UploadedDate = DateTime.UtcNow,
                    DocumentType = DocumentType.CurriculumVitae
                });
        }

        await portfolioDbContext.SaveChangesAsync();

        var adminEmail = "fredric.domvall@gmail.com";
        var adminPassword = "Admin123!";

        var existingAdminUser = await userManager.FindByEmailAsync(adminEmail);

        if (existingAdminUser is null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            await userManager.CreateAsync(adminUser, adminPassword);
        }
    }
}