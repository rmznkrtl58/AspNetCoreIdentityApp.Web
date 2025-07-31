using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class SignUpViewModel
    {
        public SignUpViewModel()
        {
            //default geçmem gerekiyor kayıt işleminde sorun çıkmasın diye
        }
        public SignUpViewModel(string username, string email, string phone, string password, string confirmPassword)
        {
            Username = username;
            Email = email;
            Phone = phone;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
        //display labelda gözükcek alanım
        [Display(Name ="Kullanıcı Adı :")]
        [Required(ErrorMessage ="Kullanıcı Adı Zorunludur!")]
        public string Username{ get; set; }
        [Required(ErrorMessage = "Mail Adresi Zorunludur!")]
        [EmailAddress(ErrorMessage ="Mail Adresi Formatında Giriniz!")]
        [Display(Name = "Mail Adresi :")]
        public string Email{ get; set; }
        [Required(ErrorMessage = "Telefon Numarası Zorunludur!")]
        [Display(Name = "Telefon :")]
        public string Phone{ get; set; }
        [Required(ErrorMessage = "Şifre Alanı Zorunludur!")]
        [Display(Name = "Şifre :")]
        public string Password{ get; set; }
        [Required(ErrorMessage = "Şifrenin Tekrar Girilmesi Zorunludur!")]
        [Compare(nameof(Password),ErrorMessage ="Şifreler Uyuşmuyor!")]
        [Display(Name = "Şifre Tekrar :")]
        public string ConfirmPassword { get; set; }
    }
}
