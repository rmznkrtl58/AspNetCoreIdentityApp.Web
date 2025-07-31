using AspNetCoreIdentityApp.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class TwoFactorLoginViewModel
    {
        [Display(Name ="Doğrulama Kodunuz...")]
        [Required(ErrorMessage ="Doğrulama Kodunu Girmek Zorunludur!")]
        [StringLength(8,ErrorMessage ="Doğrulama Kodunuz En Fazla 8 Haneli Olabilir!")]
        public string VerificationCode { get; set; }
        public bool IsRememberMe { get; set; }
        public bool IsRecoveryCode { get; set; }
        public TwoFactor TwoFactorType { get; set; }
    }
}
