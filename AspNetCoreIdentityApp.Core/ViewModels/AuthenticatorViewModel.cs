using AspNetCoreIdentityApp.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class AuthenticatorViewModel
    {
        public string SharedKey { get; set; }
        public string AuthenticationUri { get; set; }
        [Display(Name ="Doğrulama Kodunuz: ")]
        [Required(ErrorMessage ="Doğrulama Kodunuzu Girmek Zorunludur!")]
        public string VerificationCode { get; set; }
        public TwoFactor TwoFactorType { get; set; }
    }
}
