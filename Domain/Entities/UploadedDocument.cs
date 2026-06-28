using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class UploadedDocument
{
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string FilePath { get; set; } = string.Empty;

    public DateTime UploadedDate { get; set; }

    public DocumentType DocumentType { get; set; }
}