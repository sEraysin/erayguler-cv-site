using System.ComponentModel.DataAnnotations;

namespace MvcCv.Models;

public sealed class LoginViewModel
{
    [Required(ErrorMessage = "Kullanici adi zorunludur.")]
    [StringLength(64, ErrorMessage = "Kullanici adi en fazla 64 karakter olabilir.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Sifre alani zorunludur.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}
