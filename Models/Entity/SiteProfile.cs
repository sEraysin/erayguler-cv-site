using System.ComponentModel.DataAnnotations;

namespace MvcCv.Models.Entity;

public sealed class SiteProfile
{
    public int Id { get; set; }

    [Required, MaxLength(80)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string LastName { get; set; } = string.Empty;

    [Required, MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(180)]
    public string Location { get; set; } = string.Empty;

    [Required, Phone, MaxLength(30)]
    public string Phone { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(120)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(120)]
    public string Language { get; set; } = string.Empty;

    [Required, MaxLength(3000)]
    public string About { get; set; } = string.Empty;

    [MaxLength(240)]
    public string ImageUrl { get; set; } = "https://i.hizliresim.com/163rypx.png";
}
