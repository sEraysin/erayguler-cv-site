using System.ComponentModel.DataAnnotations;

namespace MvcCv.Models.Entity;

public sealed class Certificate
{
    public int Id { get; set; }

    [Required, MaxLength(220)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Issuer { get; set; } = string.Empty;

    [MaxLength(80)]
    public string DateText { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
}
