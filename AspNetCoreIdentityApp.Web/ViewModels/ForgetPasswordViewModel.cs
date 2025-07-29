using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Mail Alanı Girilmesi Zorunludur!")]
        [EmailAddress(ErrorMessage = "E-Mail Formatı Yanlıştır!")]
        [Display(Name = "Email :")]
        public string Mail { get; set; }
    }
}
