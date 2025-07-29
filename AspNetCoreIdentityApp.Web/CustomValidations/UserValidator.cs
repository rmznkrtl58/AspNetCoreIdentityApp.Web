using AspNetCoreIdentityApp.Web.Entities;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.CustomValidations
{
    public class UserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var identityError = new List<IdentityError>();
            //out _ =>ifadesi benim kullanıcı adımdan gelen ilk karakterin int olup olmadığından true veya false durumu gelecek eğer ilk karakterim true olarak gelirse numerictir out _ ise bu numeric olan değerimi değişkene atma set etme demek
            var isNumeric = int.TryParse(user.UserName[0]!.ToString(), out _);
            if (isNumeric)
            {
                identityError.Add(new IdentityError()
                {
                    Code = "UsernameContainFirstLetterDigit",
                    Description = "Girilen Kullanıcı Adının İlk Karakteri Numeric Olamaz!"
                });
            }
            if (identityError.Any())
            {
                return Task.FromResult(IdentityResult.Failed(identityError.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
