using Domain.Entities;

namespace Application.Interfaces;

public interface IPortfolioRepository
{
    Task<PortfolioProfile?> GetProfileAsync();
    Task<IReadOnlyList<PortfolioProject>> GetProjectsAsync();
    Task<PortfolioProject?> GetProjectByIdAsync(int id);
    Task<IReadOnlyList<Skill>> GetSkillsAsync();
    Task<IReadOnlyList<TimelineItem>> GetTimelineItemsAsync();
    Task<UploadedDocument?> GetCurrentCvAsync();
    Task AddProjectAsync(PortfolioProject project);
    Task UpdateProjectAsync(PortfolioProject project);
    Task DeleteProjectAsync(int id);
    Task AddProjectLinkAsync(ProjectLink projectLink);
    Task<ProjectLink?> GetProjectLinkByIdAsync(int id);
    Task UpdateProjectLinkAsync(ProjectLink projectLink);
    Task<Skill?> GetSkillByIdAsync(int id);
    Task AddSkillAsync(Skill skill);
    Task UpdateSkillAsync(Skill skill);
    Task DeleteSkillAsync(int id);
    Task<TimelineItem?> GetTimelineItemByIdAsync(int id);
    Task AddTimelineItemAsync(TimelineItem timelineItem);
    Task UpdateTimelineItemAsync(TimelineItem timelineItem);
    Task DeleteTimelineItemAsync(int id);
    Task UpdateProfileAsync(PortfolioProfile profile);
    Task AddUploadedDocumentAsync(UploadedDocument uploadedDocument);
    Task<ProjectImage?> GetProjectImageByIdAsync(int id);
    Task AddProjectImageAsync(ProjectImage projectImage);
    Task UpdateProjectImageAsync(ProjectImage projectImage);
    Task DeleteProjectImageAsync(int id);
    Task<int> GetProjectCountAsync();
    Task<int> GetFeaturedProjectCountAsync();
    Task<int> GetSkillCountAsync();
    Task<int> GetTimelineItemCountAsync();
    Task<int> GetProjectImageCountAsync();
}