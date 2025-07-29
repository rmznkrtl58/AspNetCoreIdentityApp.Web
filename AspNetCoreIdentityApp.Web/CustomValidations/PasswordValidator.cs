using AspNetCoreIdentityApp.Web.Entities;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.CustomValidations
{   //Şifre alanım ile ilgili Özel yazmış olduğum Doğrulama Yapılandırmaları
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {
            var identityErrors=new List<IdentityError>();
            if (password!.ToLower().Contains(user.UserName!.ToLower()))
            {
                identityErrors.Add(new IdentityError()
                {
                    Code = "PasswordContainUserName",
                    Description = "Şifre Alanı Kullanıcı Adını İçeremez!"
                });
            }
            if (password.StartsWith("12345"))
            {
                identityErrors.Add(new IdentityError()
                {
                    Code = "PasswordCannotStartWith12345",
                    Description = "Şifre Alanı Ardışık Sayı İçermez"
                });
            }
            if (identityErrors.Any()) 
            {
                return Task.FromResult(IdentityResult.Failed(identityErrors.ToArray()));            
            }
            return Task.FromResult(IdentityResult.Success);

        }
    }
}
