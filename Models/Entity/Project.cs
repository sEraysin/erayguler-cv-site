using System.ComponentModel.DataAnnotations;

namespace MvcCv.Models.Entity;

public sealed class Project
{
    public int Id { get; set; }

    [Required, MaxLength(160)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(240)]
    public string Technologies { get; set; } = string.Empty;

    [Required, MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Url, MaxLength(240)]
    public string? Url { get; set; }

    public int DisplayOrder { get; set; }
}
