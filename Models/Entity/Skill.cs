using System.ComponentModel.DataAnnotations;

namespace MvcCv.Models.Entity;

public sealed class Skill
{
    public int Id { get; set; }

    [Required, MaxLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string Category { get; set; } = string.Empty;

    [Range(0, 100)]
    public int Level { get; set; } = 80;

    public int DisplayOrder { get; set; }
}
