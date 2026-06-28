using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class PortfolioDbContext(DbContextOptions<PortfolioDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<PortfolioProfile> PortfolioProfiles => Set<PortfolioProfile>();
    public DbSet<PortfolioProject> PortfolioProjects => Set<PortfolioProject>();
    public DbSet<ProjectLink> ProjectLinks => Set<ProjectLink>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<TimelineItem> TimelineItems => Set<TimelineItem>();
    public DbSet<UploadedDocument> UploadedDocuments => Set<UploadedDocument>();
    public DbSet<ProjectImage> ProjectImages => Set<ProjectImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PortfolioProject>()
            .HasMany(project => project.ProjectLinks)
            .WithOne(projectLink => projectLink.PortfolioProject)
            .HasForeignKey(projectLink => projectLink.PortfolioProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PortfolioProject>()
            .HasMany(project => project.ProjectImages)
            .WithOne(projectImage => projectImage.PortfolioProject)
            .HasForeignKey(projectImage => projectImage.PortfolioProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}