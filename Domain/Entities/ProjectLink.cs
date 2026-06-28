using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ProjectLink
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(2048)]
    [Url]
    public string Url { get; set; } = string.Empty;

    public int PortfolioProjectId { get; set; }

    public PortfolioProject? PortfolioProject { get; set; }

    public ProjectLinkType LinkType { get; set; }
}