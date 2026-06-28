using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PortfolioRepository(PortfolioDbContext portfolioDbContext) : IPortfolioRepository
{
    public async Task<PortfolioProfile?> GetProfileAsync()
    {
        return await portfolioDbContext.PortfolioProfiles.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<PortfolioProject>> GetProjectsAsync()
    {
        return await portfolioDbContext.PortfolioProjects
            .Include(project => project.ProjectLinks)
            .Include(project => project.ProjectImages)
            .OrderByDescending(project => project.IsFeatured)
            .ThenByDescending(project => project.CreatedDate)
            .ToListAsync();
    }

    public async Task<PortfolioProject?> GetProjectByIdAsync(int id)
    {
        return await portfolioDbContext.PortfolioProjects
            .Include(project => project.ProjectLinks)
            .Include(project => project.ProjectImages)
            .FirstOrDefaultAsync(project => project.Id == id);
    }

    public async Task<IReadOnlyList<Skill>> GetSkillsAsync()
    {
        return await portfolioDbContext.Skills
            .OrderBy(skill => skill.Name)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<TimelineItem>> GetTimelineItemsAsync()
    {
        return await portfolioDbContext.TimelineItems
            .OrderByDescending(timelineItem => timelineItem.StartDate)
            .ToListAsync();
    }

    public async Task<UploadedDocument?> GetCurrentCvAsync()
    {
        return await portfolioDbContext.UploadedDocuments
            .Where(document => document.DocumentType == DocumentType.CurriculumVitae)
            .OrderByDescending(document => document.UploadedDate)
            .FirstOrDefaultAsync();
    }

    public async Task AddProjectAsync(PortfolioProject project)
    {
        await portfolioDbContext.PortfolioProjects.AddAsync(project);
        await portfolioDbContext.SaveChangesAsync();
    }

    public async Task UpdateProjectAsync(PortfolioProject project)
    {
        portfolioDbContext.PortfolioProjects.Update(project);
        await portfolioDbContext.SaveChangesAsync();
    }

    public async Task DeleteProjectAsync(int id)
    {
        var project = await portfolioDbContext.PortfolioProjects
            .FirstOrDefaultAsync(project => project.Id == id);

        if (project is null)
        {
            return;
        }

        portfolioDbContext.PortfolioProjects.Remove(project);
        await portfolioDbContext.SaveChangesAsync();
    }

    public async Task AddProjectLinkAsync(ProjectLink projectLink)
    {
        await portfolioDbContext.ProjectLinks.AddAsync(projectLink);
        await portfolioDbContext.SaveChangesAsync();
    }

    public async Task<ProjectLink?> GetProjectLinkByIdAsync(int id)
    {
        return await portfolioDbContext.ProjectLinks
            .FirstOrDefaultAsync(projectLink => projectLink.Id == id);
    }

    public async Task UpdateProjectLinkAsync(ProjectLink projectLink)
    {
        portfolioDbContext.ProjectLinks.Update(projectLink);
        await portfolioDbContext.SaveChangesAsync();
    }

    public async Task<Skill?> GetSkillByIdAsync(int id)
    {
        return await portfolioDbContext.Skills
            .FirstOrDefaultAsync(skill => skill.Id == id);
    }

    public async Task AddSkillAsync(Skill skill)
    {
        await portfolioDbContext.Skills.AddAsync(skill);
        await portfolioDbContext.SaveChangesAsync();
    }

    public async Task UpdateSkillAsync(Skill skill)
    {
        portfolioDbContext.Skills.Update(skill);
        await portfolioDbContext.SaveChangesAsync();
    }

    public async Task DeleteSkillAsync(int id)
    {
        var skill = await portfolioDbContext.Skills
            .FirstOrDefaultAsync(skill => skill.Id == id);

        if (skill is null)
        {
            return;
        }

        portfolioDbContext.Skills.Remove(skill);
        await portfolioDbContext.SaveChangesAsync();
    }

    public async Task<TimelineItem?> GetTimelineItemByIdAsync(int id)
    {
        return await portfolioDbContext.TimelineItems
            .FirstOrDefaultAsync(timelineItem => timelineItem.Id == id);
    }

    public async Task AddTimelineItemAsync(TimelineItem timelineItem)
    {
        await portfolioDbContext.TimelineItems.AddAsync(timelineItem);
        await portfolioDbContext.SaveChangesAsync();
    }

    public async Task UpdateTimelineItemAsync(TimelineItem timelineItem)
    {
        portfolioDbContext.TimelineItems.Update(timelineItem);
        await portfolioDbContext.SaveChangesAsync();
    }

    public async Task DeleteTimelineItemAsync(int id)
    {
        var timelineItem = await portfolioDbContext.TimelineItems
            .FirstOrDefaultAsync(timelineItem => timelineItem.Id == id);

        if (timelineItem is null)
        {
            return;
        }

        portfolioDbContext.TimelineItems.Remove(timelineItem);
        await portfolioDbContext.SaveChangesAsync();
    }

    public async Task UpdateProfileAsync(PortfolioProfile profile)
    {
        portfolioDbContext.PortfolioProfiles.Update(profile);
        await portfolioDbContext.SaveChangesAsync();
    }

    public async Task AddUploadedDocumentAsync(UploadedDocument uploadedDocument)
    {
        await portfolioDbContext.UploadedDocuments.AddAsync(uploadedDocument);
        await portfolioDbContext.SaveChangesAsync();
    }

    public async Task<ProjectImage?> GetProjectImageByIdAsync(int id)
    {
        return await portfolioDbContext.ProjectImages
            .FirstOrDefaultAsync(projectImage => projectImage.Id == id);
    }

    public async Task AddProjectImageAsync(ProjectImage projectImage)
    {
        if (projectImage.IsCoverImage)
        {
            var existingCoverImages = await portfolioDbContext.ProjectImages
                .Where(existingImage =>
                    existingImage.PortfolioProjectId == projectImage.PortfolioProjectId &&
                    existingImage.IsCoverImage)
                .ToListAsync();

            foreach (var existingCoverImage in existingCoverImages)
            {
                existingCoverImage.IsCoverImage = false;
            }
        }

        await portfolioDbContext.ProjectImages.AddAsync(projectImage);
        await portfolioDbContext.SaveChangesAsync();
    }

    public async Task UpdateProjectImageAsync(ProjectImage projectImage)
    {
        if (projectImage.IsCoverImage)
        {
            var existingCoverImages = await portfolioDbContext.ProjectImages
                .Where(existingImage =>
                    existingImage.PortfolioProjectId == projectImage.PortfolioProjectId &&
                    existingImage.Id != projectImage.Id &&
                    existingImage.IsCoverImage)
                .ToListAsync();

            foreach (var existingCoverImage in existingCoverImages)
            {
                existingCoverImage.IsCoverImage = false;
            }
        }

        portfolioDbContext.ProjectImages.Update(projectImage);
        await portfolioDbContext.SaveChangesAsync();
    }

    public async Task DeleteProjectImageAsync(int id)
    {
        var projectImage = await portfolioDbContext.ProjectImages
            .FirstOrDefaultAsync(projectImage => projectImage.Id == id);

        if (projectImage is null)
        {
            return;
        }

        portfolioDbContext.ProjectImages.Remove(projectImage);
        await portfolioDbContext.SaveChangesAsync();
    }

    public async Task<int> GetProjectCountAsync()
    {
        return await portfolioDbContext.PortfolioProjects.CountAsync();
    }

    public async Task<int> GetFeaturedProjectCountAsync()
    {
        return await portfolioDbContext.PortfolioProjects
            .CountAsync(project => project.IsFeatured);
    }

    public async Task<int> GetSkillCountAsync()
    {
        return await portfolioDbContext.Skills.CountAsync();
    }

    public async Task<int> GetTimelineItemCountAsync()
    {
        return await portfolioDbContext.TimelineItems.CountAsync();
    }

    public async Task<int> GetProjectImageCountAsync()
    {
        return await portfolioDbContext.ProjectImages.CountAsync();
    }
}