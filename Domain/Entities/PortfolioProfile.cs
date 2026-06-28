using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class PortfolioProfile
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string ProfessionalTitle { get; set; } = string.Empty;

    [Required]
    [MaxLength(4000)]
    public string AboutMe { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string EmailAddress { get; set; } = string.Empty;

    [Url]
    [MaxLength(2048)]
    public string? LinkedInUrl { get; set; }

    [Url]
    [MaxLength(2048)]
    public string? GitHubUrl { get; set; }
}