namespace Presentation.ViewModels.Admin;

public class AdminDashboardViewModel
{
    public int ProjectCount { get; set; }

    public int FeaturedProjectCount { get; set; }

    public int SkillCount { get; set; }

    public int TimelineItemCount { get; set; }

    public int ProjectImageCount { get; set; }

    public bool HasCurrentCv { get; set; }
}