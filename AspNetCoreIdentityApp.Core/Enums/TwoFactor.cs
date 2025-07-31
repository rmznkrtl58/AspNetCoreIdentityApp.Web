using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreIdentityApp.Core.Enums
{
    public enum TwoFactor
    {
        [Display(Name ="Hiçbiri")]
        None=0,
        [Display(Name = "Sms İle Kimlik Doğrulama")]
        Phone =1,
        [Display(Name ="Mail Adresi İle Kimlik Doğrulama")]
        Email=2,
        [Display(Name = "Microsoft/Google Authenticator İle Kimlik Doğrulama")]
        MicrosoftAndGoogle =3
    }
}
