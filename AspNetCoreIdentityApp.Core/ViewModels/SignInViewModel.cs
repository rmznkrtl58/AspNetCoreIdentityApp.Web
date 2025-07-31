using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class SignInViewModel
    {   
        //Giriş İşlemi Sağlıklı Olabilmesi için defatult bir constructor kurdum.
        public SignInViewModel()
        {
            
        }
        public SignInViewModel(string mail, string password)
        {
            Mail = mail;
            Password = password;
        }
        [Required(ErrorMessage ="Mail Alanı Girilmesi Zorunludur!")]
        [EmailAddress(ErrorMessage ="E-Mail Formatı Yanlıştır!")]
        [Display(Name ="Email :")]
        public string Mail{ get; set; }
        [Required(ErrorMessage = "Şifre Alanı Girilmesi Zorunludur!")]
        [Display(Name = "Şifre :")]
        public string Password{ get; set; }
        [Display(Name = "Beni Hatırla :")]
        public bool RememberMe { get; set; }
    }
}
