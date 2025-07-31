using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class PasswordChangeViewModel
    {
        [Required(ErrorMessage = "Şifre Alanı Zorunludur!")]
        [Display(Name = "Eski Şifre :")]
        public string PasswordOld{ get; set; }
        [Required(ErrorMessage = "Şifre Alanı Zorunludur!")]
        [Display(Name = "Yeni Şifre :")]
        [MinLength(6,ErrorMessage ="Yeni Şifre Minimum 6 Karakter Olmalıdır!")]
        public string PasswordNew{ get; set; }
        [Required(ErrorMessage = "Şifrenin Tekrar Girilmesi Zorunludur!")]
        [Compare(nameof(PasswordNew), ErrorMessage = "Şifreler Uyuşmuyor!")]
        [Display(Name = "Yeni Şifre Tekrar :")]
        [MinLength(6, ErrorMessage = "Yeni Şifre Minimum 6 Karakter Olmalıdır!")]
        public string PasswordNewConfirm{ get; set; }
    }
}
