using System.ComponentModel.DataAnnotations;

namespace MvcCv.Models;

public sealed class ContactMessageInputModel
{
    [Required(ErrorMessage = "Ad soyad alanı zorunludur."), MaxLength(120)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta alanı zorunludur."), EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi yazın."), MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Konu alanı zorunludur."), MaxLength(160)]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mesaj alanı zorunludur."), MinLength(10, ErrorMessage = "Mesaj en az 10 karakter olmalıdır."), MaxLength(4000)]
    public string Body { get; set; } = string.Empty;
}
