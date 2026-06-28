using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ProjectImage
{
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string FilePath { get; set; } = string.Empty;

    [MaxLength(150)]
    public string AltText { get; set; } = string.Empty;

    public bool IsCoverImage { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime UploadedDate { get; set; }

    public int PortfolioProjectId { get; set; }

    public PortfolioProject? PortfolioProject { get; set; }
}