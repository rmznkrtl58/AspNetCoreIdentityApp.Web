using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.Localization
{
    //Global Olarak Identityin attığı hataları türkçeleştirme işlemleri yaptığım yer
    public class LocalizationIdentityErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "DuplicateUserName",
                Description = $"({userName})Girdiğiniz Kullanıcı Adı Başka Bir Kullanıcı Tarafından Kullanılıyor"
            };
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError()
            {
                Code = "DuplicateEmail",
                Description = $"({email})Girdiğiniz E-posta Adresi Başka Bir Kullanıcı Tarafından Kullanılıyor"
            };
        }
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError()
            {
                Code = "PasswordTooShort",
                Description = $"(Girilen Şifre En Az 6 Karakter Olmalı!)-Girilen Karakter Sayısı:{length}"
            };
        }
    }
}
