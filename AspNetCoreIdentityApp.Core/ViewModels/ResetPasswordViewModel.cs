using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Alanı Zorunludur!")]
        [Display(Name = "Yeni Şifre :")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifrenin Tekrar Girilmesi Zorunludur!")]
        [Compare(nameof(Password), ErrorMessage = "Şifreler Uyuşmuyor!")]
        [Display(Name = "Yeni Şifre Tekrar :")]
        public string ConfirmPassword { get; set; }
    }
}
