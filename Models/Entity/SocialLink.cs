using System.ComponentModel.DataAnnotations;

namespace MvcCv.Models.Entity;

public sealed class SocialLink
{
    public int Id { get; set; }

    [Required, MaxLength(60)]
    public string Name { get; set; } = string.Empty;

    [Required, Url, MaxLength(240)]
    public string Url { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string IconClass { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }
}
