using System.ComponentModel.DataAnnotations;

namespace MvcCv.Models.Entity;

public sealed class Experience
{
    public int Id { get; set; }

    [Required, MaxLength(140)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(160)]
    public string Company { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string DateRange { get; set; } = string.Empty;

    [Required, MaxLength(3000)]
    public string Description { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
}
