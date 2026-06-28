using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class PortfolioProject
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(500)]
    public string TechnologiesUsed { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public bool IsFeatured { get; set; }

    public ProjectCategory Category { get; set; }

    public ICollection<ProjectLink> ProjectLinks { get; set; } = [];

    public ICollection<ProjectImage> ProjectImages { get; set; } = [];
}