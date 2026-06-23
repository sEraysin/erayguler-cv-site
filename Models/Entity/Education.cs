using System.ComponentModel.DataAnnotations;

namespace MvcCv.Models.Entity;

public sealed class Education
{
    public int Id { get; set; }

    [Required, MaxLength(160)]
    public string School { get; set; } = string.Empty;

    [Required, MaxLength(160)]
    public string Degree { get; set; } = string.Empty;

    [MaxLength(160)]
    public string Field { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Grade { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string DateRange { get; set; } = string.Empty;

    [MaxLength(1500)]
    public string Description { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
}
